module Cli.View.Common

open Cli.View.Actions
open Cli.View.TextConstants
open Entities
open Simulation.Queries

/// Creates a list of choices from a map of unfinished songs.
let unfinishedSongsSelectorOf state (band: Band) =
    Songs.unfinishedSongsByBand state band.Id
    |> Map.toList
    |> List.map
        (fun (songId, ((UnfinishedSong us), _, currentQuality)) ->
            let (SongId id) = songId

            { Id = id.ToString()
              Text = Literal $"{us.Name} (Quality: {currentQuality}%%)" })

/// Returns the unfinished song that was selected in the choice prompt.
let unfinishedSongFromSelection state (band: Band) (selection: Choice) =
    selection.Id
    |> System.Guid.Parse
    |> SongId
    |> Songs.unfinishedSongByBandAndSongId state band.Id
    |> Option.get

/// Returns the full selected member.
let memberFromSelection (band: Band) (selection: Choice) =
    selection.Id
    |> System.Guid.Parse
    |> CharacterId
    |> fun id ->
        List.find (fun (c: CurrentMember) -> c.Character.Id = id) band.Members

/// Creates a list of choices from all available genres.
let genreOptions =
    Database.genres ()
    |> List.map (fun genre -> { Id = genre; Text = Literal genre })

/// Creates a list of choices from all available instruments.
let instrumentOptions =
    Database.roles ()
    |> List.map
        (fun role ->
            { Id = role.ToString()
              Text = Literal(role.ToString()) })

/// Returns the associated color given the level of a skill or the quality
/// of a song.
let colorForLevel level =
    match level with
    | level when level < 30 -> Spectre.Console.Color.Red
    | level when level < 60 -> Spectre.Console.Color.Orange1
    | level when level < 80 -> Spectre.Console.Color.Green
    | _ -> Spectre.Console.Color.Blue

/// Choice handler for optional prompts that calls the backHandler if the
/// Back option was selected or the choiceHandler if a choice was selected.
let basicOptionalChoiceHandler backHandler choiceHandler choice =
    seq {
        match choice with
        | Back -> yield backHandler
        | Choice choice -> yield! choiceHandler choice
    }

/// Choice handler for optional prompts with the back option pointing to the
/// main menu.
let mainMenuOptionalChoiceHandler handler choice =
    basicOptionalChoiceHandler
        (Scene <| MainMenu Savegame.Available)
        handler
        choice

/// Choice handler for optional prompts with the back option pointing to the
/// rehearsal room.
let rehearsalRoomOptionalChoiceHandler handler choice =
    basicOptionalChoiceHandler (Scene RehearsalRoom) handler choice
