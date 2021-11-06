module Log

open Common
open Entities

type SavegameAgentMessage = Append of Effect

let private appendEffect (effect: Effect) =
    effect
    |> Serializer.serialize
    |> fun content -> $"{content}\n"
    |> Files.append (Files.logPath ())

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
                    | Append effect -> appendEffect effect

                    return! loop ()
                }

            loop ()

    member this.Append effect = effect |> Append |> agent.Post

let private logAgent = LogAgent()

/// Attempts to write the given effect to the activity log.
let append = logAgent.Append
