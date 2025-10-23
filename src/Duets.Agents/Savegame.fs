module Duets.Agents.Savegame

open System
open Duets.Common
open Duets.Data
open Duets.Data.Savegame.Types
open Duets.Entities

/// Settings that we allow the player to customize.
type Settings = { SavegamePath: string }

/// Contents of the savegame file, which contains a version for migration
/// purposes and the actual data.
type SavegameContents = { Version: int; Data: State }

/// Current state of the savegame, which can be available if the savegame could
/// be parsed correctly, not available if there's no savegames available and
/// incompatible if the contents of the savegame could not be properly interpreted
type SavegameState =
    | Available of version: int
    | NotAvailable
    | Incompatible of reason: MigrationError

/// Result of saving the settings, which can be successful or fail with an exception.
type SettingsSaveResult =
    | Success
    | Failure of Exception

/// Attempts to read the settings from the file and returns them if available.
let private loadSettingsFromFile () : Settings option =
    Files.settingsPath () |> Files.readAll |> Option.bind Serializer.deserialize

/// Attempts to read the savegame file from the path specified in the settings.
let private savegameFile () =
    loadSettingsFromFile ()
    |> Option.map (fun settings -> Files.savegameFile settings.SavegamePath)
    |> Option.defaultValue (Files.savegamePath ())

/// Attempts to read the savegame from the file and sets the state with it,
/// returning whether it was available or not.
let private readSavegameFile () = savegameFile () |> Files.readAll

/// Attempts to parse the given savegame contents.
let private parseSavegame contents =
    Serializer.deserialize contents
    |> Option.tap (fun savegame -> State.set savegame.Data)
    |> Option.map (fun savegame -> Available(savegame.Version))
    |> Option.defaultValue NotAvailable

/// Attempts to write the given state into the savegame file.
let private writeSavegame version (state: State) =
    let savegame = { Version = version; Data = state }
    savegame |> Serializer.serialize |> Files.write (savegameFile ())

/// Attempts to write the given settings into the settings file.
let private writeSettings (settings: Settings) =
    settings |> Serializer.serialize |> Files.write (Files.settingsPath ())

type SavegameAgentMessage =
    | Read of AsyncReplyChannel<SavegameState>
    | ReadSettings of AsyncReplyChannel<Settings option>
    | Write of State
    | WriteSync of State * AsyncReplyChannel<unit>
    | WriteSettings of Settings option * AsyncReplyChannel<SettingsSaveResult>

type SavegameAgentState = { Version: int }

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
            let rec loop agentState =
                let tryGetVersion () =
                    match agentState with
                    | Some state -> state.Version
                    | None -> Savegame.Migrations.lastSavegameVersion

                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Read channel ->
                        let state =
                            readSavegameFile ()
                            |> Option.map (fun content ->
                                // Attempt to first apply migrations if needed.
                                let result =
                                    Savegame.Migrations.applyMigrations content

                                match result with
                                | Ok(contents) -> parseSavegame contents
                                | Error(error) -> Incompatible(error))
                            |> Option.defaultValue NotAvailable

                        channel.Reply state

                        // Use the latest version we've got from the savegame.
                        match state with
                        | Available(version) ->
                            return! loop (Some({ Version = version }))
                        | _ -> return! loop agentState
                    | ReadSettings channel ->
                        try
                            loadSettingsFromFile () |> channel.Reply
                        with _ ->
                            channel.Reply None
                    | Write state ->
                        let version = tryGetVersion ()
                        writeSavegame version state
                    | WriteSync(state, channel) ->
                        let version = tryGetVersion ()
                        writeSavegame version state
                        channel.Reply()
                    | WriteSettings(settings, channel) ->
                        try
                            match settings with
                            | Some settings -> writeSettings settings
                            | None -> Files.settingsPath () |> Files.delete

                            channel.Reply Success
                        with exn ->
                            channel.Reply(Failure exn)

                    return! loop agentState
                }

            loop None

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
