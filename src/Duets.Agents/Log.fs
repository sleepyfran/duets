module Duets.Agents.Log


open Duets.Common
open Duets.Entities

type LogAgentMessage =
    | AppendEffect of Effect
    | AppendMessage of string

let private appendToLog (str: string) =
    str |> (fun content -> $"{content}\n") |> Files.append (Files.logPath ())

let private appendEffectToLog (effect: Effect) =
    effect |> Serializer.serialize |> appendToLog

/// Agent in charge of writing to the activity log. See `Savegame.fs` for the
/// reasoning behind have this operation and the savegame operations in an
/// agent.
type private LogAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | AppendEffect effect -> appendEffectToLog effect
                    | AppendMessage message -> appendToLog message

                    return! loop ()
                }

            loop ()

    member this.Append effect = effect |> AppendEffect |> agent.Post
    member this.Append str = str |> AppendMessage |> agent.Post

let private logAgent = LogAgent()

/// Attempts to write the given effect to the activity log.
let appendEffect (effect: Effect) = logAgent.Append effect

/// Attempts to write the given message to the activity log.
let appendMessage (msg: string) = logAgent.Append msg
