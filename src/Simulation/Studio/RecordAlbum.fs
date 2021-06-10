module Simulation.Studio.RecordAlbum

open Common
open Entities
open Simulation.Queries

type AlbumRecordingError =
    NotEnoughMoney of bandBalance: int<dd> * studioBill: int<dd>

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

let private recordAlbum' state studio band (UnreleasedAlbum album) =
    recordTrackList state studio album.TrackList
    |> Album.from album.Name
    |> Result.unwrap
    |> fun album ->
        Ok(UnreleasedAlbum album, AlbumRecorded(band, UnreleasedAlbum album))

let private generatePayment state studio (band: Band) (UnreleasedAlbum album) =
    let bandAccount = Band band.Id
    let bandBalance = Bank.balanceOf state bandAccount

    let studioBill =
        studio.PricePerSong * List.length album.TrackList

    if bandBalance >= studioBill then
        Ok
        <| MoneyTransferred(bandAccount, (Outgoing studioBill))
    else
        Error <| NotEnoughMoney(bandBalance, studioBill)

/// Applies the improvement in quality given by the producer of the given studio
/// and attempts to generate an album from the name and track list, applying the
/// validations of an album. Also checks the band's bank account and generates
/// the payment to the studio.
let recordAlbum state studio band unreleasedAlbum =
    recordAlbum' state studio band unreleasedAlbum
    |> Result.bind
        (fun (album, prevEffect) ->
            match generatePayment state studio band album with
            | Ok effect -> Ok(album, [ prevEffect; effect ])
            | Error error -> Error error)
