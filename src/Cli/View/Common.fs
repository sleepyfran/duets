module Cli.View.Common

open Cli.View.TextConstants
open Cli.View.Actions
open Entities
open Simulation.Queries

/// Creates a list of choices from the unfinished songs of the given band.
let unfinishedSongsSelectorOf state (band: Band) =
    Songs.unfinishedByBand state band.Id
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
    |> Songs.unfinishedByBandAndSongId state band.Id
    |> Option.get

/// Creates a list of choices from the finished songs of the given band.
let finishedSongsSelectorOf state (band: Band) =
    Songs.finishedByBand state band.Id
    |> Map.toList
    |> List.map
        (fun (songId, ((FinishedSong fs), quality)) ->
            let (SongId id) = songId

            { Id = id.ToString()
              Text =
                  Literal
                      $"{fs.Name} (Quality: {quality}%%, Length: {fs.Length.Minutes}:{fs.Length.Seconds})" })

/// Returns the finished song that was selected in the choice prompt.
let finishedSongFromSelection state (band: Band) (selection: Choice) =
    selection.Id
    |> System.Guid.Parse
    |> SongId
    |> Songs.finishedByBandAndSongId state band.Id
    |> Option.get

/// Returns the unfinished songs that were selected in the multi choice prompt.
let finishedSongsFromSelection state (band: Band) (selection: Choice list) =
    selection
    |> List.map (finishedSongFromSelection state band)

/// Creates a list of choices from the unreleased albums of the given band.
let unreleasedAlbumsSelectorOf state (band: Band) =
    Albums.unreleasedByBand state band.Id
    |> Map.toList
    |> List.map
        (fun ((AlbumId albumId), (UnreleasedAlbum album)) ->
            { Id = albumId.ToString()
              Text = Literal album.Name })

/// Returns the unreleased album that was selected in the choice prompt.
let unreleasedAlbumFromSelection state (band: Band) (selection: Choice) =
    selection.Id
    |> System.Guid.Parse
    |> AlbumId
    |> Albums.unreleasedByBandAndAlbumId state band.Id
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
let rehearsalRoomOptionalChoiceHandler space rooms handler choice =
    basicOptionalChoiceHandler
        (Scene(Scene.RehearsalRoom(space, rooms)))
        handler
        choice

/// Choice handler for optional prompts with the back option pointing to the
/// map.
let mapOptionalChoiceHandler handler choice =
    basicOptionalChoiceHandler (Scene World) handler choice

/// Choice handler for optional prompts with the back option pointing to the
/// world scene.
let worldOptionalChoiceHandler handler choice =
    basicOptionalChoiceHandler (Scene World) handler choice

/// Choice handler for optional prompts with the back option pointing to the
/// phone scene.
let phoneOptionalChoiceHandler handler choice =
    basicOptionalChoiceHandler (Scene Phone) handler choice
