module Cli.View.Scenes.RehearsalRoom.ImproveSong

open System
open Cli.View.Actions
open Cli.View.TextConstants
open Entities.Song
open Mediator.Mutation
open Mediator.Mutations.Songs
open Mediator.Query
open Mediator.Queries.Storage

let rec improveSong () =
  seq {
    let currentBand = query CurrentBandQuery

    let unfinishedSongsOptions =
      query <| UnfinishedSongByBandQuery currentBand.Id
      |> List.map
           (fun ((UnfinishedSong us), _, currentQuality) ->
             let (SongId id) = us.Id

             { Id = id.ToString()
               Text = Literal $"{us.Name} (Quality: {currentQuality}%%)" })

    if unfinishedSongsOptions.Length = 0 then
      yield Message <| TextConstant ImproveSongNoSongAvailable
      yield (Scene RehearsalRoom)
    else
      yield
        Prompt
          { Title = TextConstant ImproveSongSelection
            Content =
              ChoicePrompt(
                unfinishedSongsOptions,
                processSongSelection currentBand
              ) }
  }

and processSongSelection band selection =
  seq {
    let selectedSong =
      selection.Id
      |> Guid.Parse
      |> SongId
      |> UnfinishedSongByIdQuery band.Id
      |> query
      |> Option.get

    let songStatus =
      selectedSong |> ImproveSongMutation |> mutate

    match songStatus with
    | CanBeImproved quality ->
        yield
          Message
          <| Literal
               $"You've improved the song. It now has a quality of {quality}%%"
    | ReachedMaxQuality quality ->
        yield
          Message
          <| Literal
               $"Your band has decided that the song does not need any further improvements. It has a quality of {
                                                                                                                    quality
               }%%. You can add it to the band's repertoire from the 'Finish song' option"

    yield (Scene RehearsalRoom)
  }
