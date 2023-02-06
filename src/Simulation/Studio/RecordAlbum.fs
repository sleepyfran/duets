module Simulation.Studio.RecordAlbum

open Common
open Entities
open Simulation.Queries
open Simulation.Bank.Operations
open Simulation.Time

let private productionQualityImprovement state studio =
    Skills.characterSkillWithLevel
        state
        studio.Producer.Id
        SkillId.MusicProduction
    |> fun (_, productionLevel) -> (float productionLevel) * 0.2

let private recordSong state studio (finishedSong: FinishedSongWithQuality) =
    let song, quality = finishedSong

    (float quality)
    |> (+) (productionQualityImprovement state studio)
    |> int
    |> Math.clamp 0 100
    |> fun improvedQuality -> RecordedSong(song, improvedQuality * 1<quality>)

let private generatePaymentForOneSong state studio (band: Band) =
    let bandAccount = Band band.Id
    expense state bandAccount studio.PricePerSong

let private generateEffectsAfterBilling state studio band effects =
    let billingResult = generatePaymentForOneSong state studio band
    let timeEffects = AdvanceTime.advanceDayMoment' state 2<dayMoments>

    match billingResult with
    | Ok billingEffects -> Ok(timeEffects @ effects @ billingEffects)
    | Error error -> Error error

/// Applies the improvement in quality given by the producer of the given studio
/// and attempts to generate an album from the name and track list, applying the
/// validations of an album. Also checks the band's bank account and generates
/// the payment to the studio.
let startAlbum state studio band albumName initialSong =
    let album = recordSong state studio initialSong |> Album.from albumName

    [ AlbumStarted(band, UnreleasedAlbum album) ]
    |> generateEffectsAfterBilling state studio band

/// Applies the improvement in quality given by the producer of the studio and
/// attempts to add a song to the given unreleased album. Generates a payment
/// to the studio if the band has enough money, otherwise doesn't let them record
/// the song.
let recordSongForAlbum state studio band (UnreleasedAlbum album) song =
    let updatedAlbum = album |> Album.addSong song

    [ AlbumUpdated(band, UnreleasedAlbum updatedAlbum) ]
    |> generateEffectsAfterBilling state studio band
