module Duets.Agents.Savegame

open System
open Duets.Common
open Duets.Entities

type SavegameState =
    | Available
    | NotAvailable
    | Incompatible

type Settings = { SavegamePath: string }

type SettingsSaveResult =
    | Success
    | Failure of Exception

/// Attempts to read the settings from the file and returns them if available.
let private loadSettingsFromFile () : Settings option =
    Files.settingsPath () |> Files.readAll |> Option.bind Serializer.deserialize

let private savegameFile () =
    loadSettingsFromFile ()
    |> Option.map (fun settings -> Files.savegameFile settings.SavegamePath)
    |> Option.defaultValue (Files.savegamePath ())

/// Attempts to read the savegame from the file and sets the state with it,
/// returning whether it was available or not.
let private loadStateFromSavegame () =
    savegameFile ()
    |> Files.readAll
    |> Option.bind Serializer.deserialize
    |> Option.map State.set
    |> Option.map (fun _ -> Available)
    |> Option.defaultValue NotAvailable

/// Attempts to write the given state into the savegame file.
let private writeSavegame (state: State) =
    state |> Serializer.serialize |> Files.write (savegameFile ())

/// Attempts to write the given settings into the settings file.
let private writeSettings (settings: Settings) =
    settings |> Serializer.serialize |> Files.write (Files.settingsPath ())

type SavegameAgentMessage =
    | Read of AsyncReplyChannel<SavegameState>
    | ReadSettings of AsyncReplyChannel<Settings option>
    | Write of State
    | WriteSync of State * AsyncReplyChannel<unit>
    | WriteSettings of Settings option * AsyncReplyChannel<SettingsSaveResult>

/// Agent in charge of writing and loading the savegame from a file.
/// The reason behind having these operations in an agent is that, since we need
/// to execute the saving constantly, doing it asynchronously is a must to not
/// block the UI and an agent provides this plus the safety of knowing that all
/// these operations will still be done in just one thread which means that the
/// saving will never be done while reading, no multiple writes, etc.
type private SavegameAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Read channel ->
                        try
                            loadStateFromSavegame () |> channel.Reply
                        with _ ->
                            channel.Reply Incompatible
                    | ReadSettings channel ->
                        try
                            loadSettingsFromFile () |> channel.Reply
                        with _ ->
                            channel.Reply None
                    | Write state -> writeSavegame state
                    | WriteSync(state, channel) ->
                        writeSavegame state
                        channel.Reply()
                    | WriteSettings(settings, channel) ->
                        try
                            match settings with
                            | Some settings -> writeSettings settings
                            | None -> Files.settingsPath () |> Files.delete

                            channel.Reply Success
                        with exn ->
                            channel.Reply(Failure exn)

                    return! loop ()
                }

            loop ()

    member this.Read() = agent.PostAndReply Read
    member this.ReadSettings() = agent.PostAndReply ReadSettings
    member this.Write state = state |> Write |> agent.Post

    member this.WriteSync state =
        agent.PostAndReply(fun channel -> WriteSync(state, channel))

    member this.WriteSettings settings =
        agent.PostAndReply(fun channel -> WriteSettings(settings, channel))

let private savegameAgent = SavegameAgent()

/// Attempts to load the savegame from a file and returns whether it was present
/// or not. If present, sets the state of the storage to the one loaded from
/// the savegame file.
let load = savegameAgent.Read

/// Attempts to load the settings of the game and return them, if any. Otherwise
/// returns None if the settings don't exist or couldn't be parsed.
let settings = savegameAgent.ReadSettings

/// Attempts to write the given settings into the settings file if the settings
/// is Some, otherwise removes the settings file.
let saveSettings = savegameAgent.WriteSettings

/// Attempts to write the current state into the savegame file doing so in a
/// separate thread.
let save () = savegameAgent.Write(State.get ())

/// Attempts to write the current state into the savegame file, waiting for the
/// process to finish.
let saveSync () = savegameAgent.WriteSync(State.get ())
