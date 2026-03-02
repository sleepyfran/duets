module Duets.UI.Components.StreamingText

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Media
open FSharp.Control
open Duets.Agents

/// Creates a component that calls the language model with the given prompt
/// and progressively renders each token as it arrives, mimicking a typewriter
/// effect. The streaming starts immediately on mount.
let create (prompt: string) : IView =
    Component.create (
        $"StreamingText-{abs (prompt.GetHashCode())}",
        fun ctx ->
            let accumulated = ctx.useState ""

            ctx.useEffect (
                handler =
                    (fun _ ->
                        let mutable text = ""

                        async {
                            let stream = LanguageModel.streamMessage prompt

                            do!
                                stream
                                |> AsyncSeq.iterAsync (fun token ->
                                    async {
                                        text <- text + token
                                        accumulated.Set text
                                    })
                        }
                        |> Async.Start),
                triggers = [ EffectTrigger.AfterInit ]
            )

            TextBlock.create [
                TextBlock.text accumulated.Current
                TextBlock.textWrapping TextWrapping.Wrap
            ]
    )
    :> IView
