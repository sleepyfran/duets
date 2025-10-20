[<AutoOpen>]
module Duets.Cli.Components.Message

open FSharp.Control
open Spectre.Console

/// Renders a message into the screen.
let showMessage text = text |> AnsiConsole.MarkupLine

/// Renders an inline message into the screen.
let showInlineMessage text = text |> AnsiConsole.Markup

/// Streams a message from an async iterator, applying the given function for
/// styling on each token.
let streamStyled styleFn iter =
    iter
    |> AsyncSeq.iter (styleFn >> showInlineMessage)
    |> Async.RunSynchronously

/// Streams a message from an async iterator, normally used to show messages
/// coming from the language model.
let streamMessage iter =
    let mutable accumulated = ""

    AnsiConsole
        .Live(Markup(""))
        .Start(fun ctx ->
            iter
            |> AsyncSeq.iter (fun token ->
                accumulated <- accumulated + token

                // Updating the target with an empty string would cause an
                // exception to be thrown, so skip an update if we have nothing.
                if accumulated = "" then
                    ()
                else
                    ctx.UpdateTarget(Markup(accumulated)))
            |> Async.RunSynchronously)

/// Renders a path into the screen.
let showPath path =
    let mutable path = TextPath path

    path.RootStyle <- Style(foreground = Color.Green)
    path.SeparatorStyle <- Style(foreground = Color.Green)
    path.StemStyle <- Style(foreground = Color.Blue)
    path.LeafStyle <- Style(foreground = Color.Yellow)

    AnsiConsole.Write path
    AnsiConsole.WriteLine()

/// Renders an exception into the screen.
let showException = AnsiConsole.WriteException
