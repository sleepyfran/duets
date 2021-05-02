module Cli.View.Common

open Cli.View.Actions
open Cli.View.TextConstants
open Entities
open Simulation.Songs.Queries
open Storage.Database

/// Creates a list of choices from a map of unfinished songs.
let unfinishedSongsSelectorOf (band: Band) =
  unfinishedSongsByBand band.Id
  |> Map.toList
  |> List.map
       (fun (songId, ((UnfinishedSong us), _, currentQuality)) ->
         let (SongId id) = songId

         { Id = id.ToString()
           Text = Literal $"{us.Name} (Quality: {currentQuality}%%)" })

/// Returns the unfinished song that was selected in the choice prompt.
let unfinishedSongFromSelection (band: Band) (selection: Choice) =
  selection.Id
  |> System.Guid.Parse
  |> SongId
  |> unfinishedSongByBandAndSongId band.Id
  |> Option.get

/// Creates a list of choices from all available genres.
let genreOptions =
  genres ()
  |> List.map (fun genre -> { Id = genre; Text = Literal genre })

/// Creates a list of choices from all available instruments.
let instrumentOptions =
  roles ()
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
