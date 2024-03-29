module Duets.Simulation.State.Songs

open Aether
open Duets.Entities

let private applyToUnfinished bandId op =
    let unfinishedSongLens = Lenses.FromState.Songs.unfinishedByBand_ bandId

    Optic.map unfinishedSongLens op

let private applyToFinished bandId op =
    let finishedSongLens = Lenses.FromState.Songs.finishedByBand_ bandId

    Optic.map finishedSongLens op

let addUnfinished (band: Band) (unfinishedSong: Unfinished<Song>) =
    let song = Song.fromUnfinished unfinishedSong

    let addUnfinishedSong = Map.add song.Id unfinishedSong
    applyToUnfinished band.Id addUnfinishedSong

let addFinished
    (band: Band)
    (finishedSong: Finished<Song>)
    (currentDate: Date)
    =
    let song = Song.fromFinished finishedSong

    let addFinishedSong =
        finishedSong
        |> Song.Finished.attachMetadata false currentDate
        |> Map.add song.Id

    applyToFinished band.Id addFinishedSong

let removeUnfinished (band: Band) songId =
    let removeUnfinishedSong = Map.remove songId

    applyToUnfinished band.Id removeUnfinishedSong

let removeFinished (band: Band) songId =
    let removeFinishedSong = Map.remove songId

    applyToFinished band.Id removeFinishedSong

let updateFinished (band: Band) (updatedSong: Finished<Song>) =
    let song = Song.fromFinished updatedSong

    let updateFinishedSong =
        Map.change song.Id (function
            | Some(FinishedWithMetadata(_, recorded, finishDate)) ->
                FinishedWithMetadata(updatedSong, recorded, finishDate) |> Some
            | None -> None)

    applyToFinished band.Id updateFinishedSong

let updateRecordedStatus (band: Band) songId recorded =
    let updateFinishedSong =
        Map.change songId (function
            | Some(FinishedWithMetadata(song, _, finishDate)) ->
                FinishedWithMetadata(song, recorded, finishDate) |> Some
            | None -> None)

    applyToFinished band.Id updateFinishedSong
