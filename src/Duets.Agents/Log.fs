module Duets.Agents.Log


open Duets.Common
open Duets.Entities

type LogAgentMessage =
    | AppendEffect of Effect
    | AppendMessage of string
    | DumpToFile of AsyncReplyChannel<unit>

let private writeToLog (messages: string list) =
    messages |> String.concat "\n" |> Files.write (Files.logPath ())

/// Agent in charge of writing to the activity log. See `Savegame.fs` for the
/// reasoning behind have this operation and the savegame operations in an
/// agent.
type private LogAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop messages =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | AppendEffect effect ->
                        return!
                            (effect |> Serializer.serialize) :: messages |> loop
                    | AppendMessage message ->
                        return! $"{message}\n" :: messages |> loop
                    | DumpToFile channel ->
                        try
                            writeToLog messages
                        with _ ->
                            (*
                            The log file is just for debugging purposes, so
                            we don't want to fail if the log file is not
                            writable.
                            *)
                            ()

                        channel.Reply()

                    return! loop messages
                }

            loop []

    member this.Append effect = effect |> AppendEffect |> agent.Post
    member this.Append str = str |> AppendMessage |> agent.Post
    member this.DumpToFile() = agent.PostAndReply DumpToFile

let private logAgent = LogAgent()

/// Attempts to write the given effect to the activity log.
let appendEffect (effect: Effect) = logAgent.Append effect

/// Attempts to write the given message to the activity log.
let appendMessage (msg: string) = logAgent.Append msg

/// Dumps the contents of the activity log to a file.
let dumpToFileSync () = logAgent.DumpToFile()
