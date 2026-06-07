module Duets.Agents.LanguageModel

open Duets.Common

open System
open System.IO
open System.Threading
open FSharp.Control
open LLama
open LLama.Common
open LLama.Native
open LLama.Sampling

type private LanguageModelState =
    { Weights: LLamaWeights
      Executor: StatelessExecutor }

[<RequireQualifiedAccess>]
module internal ResponseParser =
    let private thoughtStart = "<|channel>thought"
    let private thoughtEnd = "<channel|>"

    let private responseTerminators =
        [ "<turn|>"
          "<end_of_turn>"
          "<|turn>user"
          "<|turn>system"
          "<|turn>model"
          "<|tool_response>"
          "<tool_response|>"
          "<|tool_call>"
          "<tool_call|>"
          "<|end_of_text|>"
          "<eos>"
          "</s>" ]

    let private controlTokens =
        thoughtStart
        :: thoughtEnd
           :: [ "<|channel>"
                "<channel|>"
                "<|think|>"
                "<|turn>"
                "<turn|>"
                "<|tool>"
                "<tool|>"
                "<|tool_call>"
                "<tool_call|>"
                "<|tool_response>"
                "<tool_response|>"
                "<|end_of_text|>"
                "<eos>"
                "<bos>"
                "</s>"
                "<start_of_turn>"
                "<end_of_turn>" ]

    let private controlTokenPrefixes =
        controlTokens
        |> List.collect (fun token ->
            [ 1 .. token.Length - 1 ]
            |> List.map (fun length -> token.Substring(0, length)))
        |> List.distinct
        |> List.sortByDescending (fun prefix -> prefix.Length)

    let rec private stripThoughtBlocks (text: string) =
        let thoughtStartIndex =
            text.IndexOf(thoughtStart, StringComparison.Ordinal)

        if thoughtStartIndex < 0 then
            text
        else
            let beforeThought = text.Substring(0, thoughtStartIndex)

            let thoughtEndIndex =
                text.IndexOf(
                    thoughtEnd,
                    thoughtStartIndex + thoughtStart.Length,
                    StringComparison.Ordinal
                )

            if thoughtEndIndex < 0 then
                beforeThought
            else
                let afterThought =
                    text.Substring(thoughtEndIndex + thoughtEnd.Length)

                beforeThought + stripThoughtBlocks afterThought

    let private truncateAtTerminator (text: string) =
        responseTerminators
        |> List.choose (fun terminator ->
            let index = text.IndexOf(terminator, StringComparison.Ordinal)

            if index < 0 then
                None
            else
                Some index)
        |> List.sort
        |> List.tryHead
        |> Option.map (fun index -> text.Substring(0, index))
        |> Option.defaultValue text

    let private removeControlTokens text =
        controlTokens
        |> List.fold (fun (response: string) token -> response.Replace(token, "")) text

    let private holdTrailingControlTokenPrefix (text: string) =
        controlTokenPrefixes
        |> List.tryFind (fun prefix ->
            text.EndsWith(prefix, StringComparison.Ordinal))
        |> Option.map (fun prefix -> text.Substring(0, text.Length - prefix.Length))
        |> Option.defaultValue text

    let private normalizeWhitespace text =
        text
        |> String.replace @"\s+" " "
        |> String.replace @"^\s+" ""

    let internal streamingVisibleText text =
        text
        |> truncateAtTerminator
        |> stripThoughtBlocks
        |> removeControlTokens
        |> normalizeWhitespace
        |> holdTrailingControlTokenPrefix

    let internal completeVisibleText text =
        text |> streamingVisibleText |> String.trim

let private gemmaSamplingPipeline () =
    // Settings taken from the Gemma 4 model card.
    new DefaultSamplingPipeline(
        Temperature = 1f,
        TopK = 64,
        TopP = 0.95f,
        MinP = 0f
    )

let private inferenceParams =
    InferenceParams(
        SamplingPipeline = gemmaSamplingPipeline (),
        DecodeSpecialTokens = true,
        AntiPrompts =
            [ "<turn|>"
              "<end_of_turn>"
              "<|turn>user"
              "<|turn>system"
              "<|tool_response>" ]
    )

let private warmupInferenceParams =
    InferenceParams(
        SamplingPipeline = gemmaSamplingPipeline (),
        DecodeSpecialTokens = true,
        MaxTokens = 1
    )

type private LanguageModelAgentMessage =
    | Initialize of AsyncReplyChannel<unit>
    | StreamMessage of prompt: string * AsyncReplyChannel<AsyncSeq<String>>

let private createPrompt prompt =
    $"""<|turn>user
{prompt |> String.trim}<turn|>
<|turn>model
"""

let private warmUp (executor: StatelessExecutor) =
    let rawAsyncEnumerable: Collections.Generic.IAsyncEnumerable<string> =
        executor.InferAsync(
            createPrompt "Give me an A",
            warmupInferenceParams,
            CancellationToken.None
        )

    rawAsyncEnumerable
    |> AsyncSeq.ofAsyncEnum
    |> AsyncSeq.iter ignore
    |> Async.RunSynchronously

/// Agent in charge of loading and streaming language model responses.
type LanguageModelAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop state =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Initialize(channel) ->
                        try
                            // Force to load now instead of after the first inference.
                            NativeApi.llama_empty_call ()

                            let modelPath =
                                Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "models",
                                    "gemma-4-E2B_q4_0-it.gguf"
                                )

                            let parameters =
                                ModelParams(modelPath, GpuLayerCount = 5)

                            let model = LLamaWeights.LoadFromFile(parameters)
                            let executor =
                                StatelessExecutor(
                                    model,
                                    parameters,
                                    ApplyTemplate = false
                                )

                            let newState =
                                { Weights = model
                                  Executor = executor }

                            warmUp executor

                            channel.Reply()
                            return! loop (Some newState)
                        with ex ->
                            printfn
                                $"Error initializing LanguageModelAgent: %s{ex.Message}"

                            channel.Reply()
                            return! loop None

                    | StreamMessage(prompt, channel) ->
                        let executor = state.Value.Executor

                        let rawAsyncEnumerable
                            : Collections.Generic.IAsyncEnumerable<string> =
                            executor.InferAsync(
                                createPrompt prompt,
                                inferenceParams,
                                CancellationToken.None
                            )

                        let mutable rawResponse = ""
                        let mutable emittedResponse = ""

                        rawAsyncEnumerable
                        |> AsyncSeq.ofAsyncEnum
                        |> AsyncSeq.map (fun token ->
                            rawResponse <- rawResponse + token

                            let visibleResponse =
                                ResponseParser.streamingVisibleText rawResponse

                            let newText =
                                if
                                    visibleResponse.Length
                                    <= emittedResponse.Length
                                then
                                    ""
                                else
                                    visibleResponse.Substring(
                                        emittedResponse.Length
                                    )

                            emittedResponse <- visibleResponse
                            newText)
                        |> channel.Reply

                        return! loop state
                }

            loop None

    member _.Initialize() =
        agent.PostAndReply(fun channel -> Initialize(channel))

    member _.StreamMessage(context) =
        agent.PostAndReply(fun channel -> StreamMessage(context, channel))

let agent = LanguageModelAgent()

/// Initializes the language model agent.
let initialize = agent.Initialize

/// Streams a message from the language model given a prompt.
let streamMessage prompt = agent.StreamMessage prompt
