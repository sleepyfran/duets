module Simulation.Studio.RecordAlbum

open Common
open Entities
open Simulation.Queries

type AlbumRecordingError =
    | NameTooShort
    | NameTooLong
    | NoSongsSelected
    | NotEnoughMoney of bandBalance: int<dd> * studioBill: int<dd>

let private productionQualityImprovement state studio =
    let (Producer (producer)) = studio.Producer

    Skills.characterSkillWithLevel state producer.Id MusicProduction
    |> fun (_, productionLevel) -> (float productionLevel) * 0.2

let private recordTrackList state studio trackList =
    List.map
        (fun (song, quality) ->
            (float quality)
            |> (+) (productionQualityImprovement state studio)
            |> int
            |> Math.clamp 0 100
            |> fun improvedQuality ->
                RecordedSong(song, improvedQuality * 1<quality>))
        trackList

let private recordAlbum' state studio band albumName trackList =
    let albumResult =
        recordTrackList state studio trackList
        |> Album.from albumName

    match albumResult with
    | Error error ->
        match error with
        | Album.CreationError.NameTooShort -> NameTooShort
        | Album.CreationError.NameTooLong -> NameTooLong
        | Album.CreationError.NoSongsSelected -> NoSongsSelected
        |> Error
    | Ok album -> Ok <| AlbumRecorded(band, album)

let private generatePayment state studio (band: Band) trackList prevEffect =
    let bandAccount = Band band.Id
    let bandBalance = Bank.balanceOf state bandAccount

    let studioBill =
        studio.PricePerSong * List.length trackList

    if bandBalance >= studioBill then
        Ok [ prevEffect
             MoneyTransferred(bandAccount, (Outgoing studioBill)) ]
    else
        Error <| NotEnoughMoney(bandBalance, studioBill)

/// Applies the improvement in quality given by the producer of the given studio
/// and attempts to generate an album from the name and track list, applying the
/// validations of an album. Also checks the band's bank account and generates
/// the payment to the studio.
let recordAlbum
    state
    studio
    band
    albumName
    (trackList: FinishedSongWithQuality list)
    =
    recordAlbum' state studio band albumName trackList
    |> Result.bind (generatePayment state studio band trackList)
