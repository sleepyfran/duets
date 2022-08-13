module Simulation.Songs.Practice

open Aether
open Common
open Entities

type PracticeSongResult =
    | SongImproved of effect: Effect
    | SongAlreadyImprovedToMax of song: FinishedSongWithQuality

/// Adds 20 of practice to the given song if it hasn't reached the maximum already.
/// This returns a SongPracticed effect which also advances time accordingly.
let practiceSong band (finishedSong: FinishedSongWithQuality) =
    let (FinishedSong song, quality) = finishedSong

    if song.Practice >= 100<practice> then
        SongAlreadyImprovedToMax finishedSong
    else
        let updatedPractice =
            song.Practice + 20<practice>
            |> Math.clamp 0<practice> 100<practice>

        let updatedSong = Optic.set Lenses.Song.practice_ updatedPractice song

        let songWithPractice = (FinishedSong updatedSong, quality)

        SongPracticed(band, songWithPractice)
        |> SongImproved
