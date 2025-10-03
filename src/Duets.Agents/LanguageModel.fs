module Duets.Agents.LanguageModel

open System
open System.IO
open System.Threading
open FSharp.Control
open LLama
open LLama.Common
open LLama.Native
open LLama.Sampling

type private LanguageModelState =
    { Executor: InteractiveExecutor
      PreviousChatHistory: ChatHistory option }

let private inferenceParams =
    InferenceParams(
        SamplingPipeline =
            // Settings taken from model card: https://huggingface.co/unsloth/gemma-3-270m-it-GGUF
            new DefaultSamplingPipeline(
                Temperature = 1f,
                TopK = 64,
                TopP = 0.95f,
                MinP = 0f
            ),
        AntiPrompts = [ "<end_of_turn>" ]
    )

type private SavegameAgentMessage =
    | Initialize of AsyncReplyChannel<unit>
    | StreamMessage of prompt: string * AsyncReplyChannel<AsyncSeq<String>>

/// Agent in charge of writing and loading the stats of the game.
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
                            NativeLibraryConfig.All.WithLogCallback(fun _ _ ->
                                ())
                            |> ignore

                            // Force to load now instead of after the first inference.
                            NativeApi.llama_empty_call ()

                            let modelPath =
                                Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "models",
                                    "model.gguf"
                                )

                            let parameters =
                                ModelParams(modelPath, GpuLayerCount = 5)

                            let model = LLamaWeights.LoadFromFile(parameters)
                            let context = model.CreateContext(parameters)
                            let executor = InteractiveExecutor(context)

                            let newState =
                                { Executor = executor
                                  PreviousChatHistory = None }

                            channel.Reply()
                            return! loop (Some newState)
                        with ex ->
                            printfn
                                $"Error initializing LanguageModelAgent: %s{ex.Message}"

                            channel.Reply()
                            return! loop None

                    | StreamMessage(prompt, channel) ->
                        let executor = state.Value.Executor
                        let chatHistory = ChatHistory()

                        let session = ChatSession(executor, chatHistory)

                        let rawAsyncEnumerable
                            : Collections.Generic.IAsyncEnumerable<string> =
                            session.ChatAsync(
                                ChatHistory.Message(AuthorRole.User, prompt),
                                inferenceParams,
                                CancellationToken.None
                            )

                        rawAsyncEnumerable
                        |> AsyncSeq.ofAsyncEnum
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
