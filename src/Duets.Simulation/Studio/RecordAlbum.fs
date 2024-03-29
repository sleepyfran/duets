module Duets.Simulation.Studio.RecordAlbum

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Time

let private productionQualityImprovement state studio =
    Queries.Skills.characterSkillWithLevel
        state
        studio.Producer.Id
        SkillId.MusicProduction
    |> fun (_, productionLevel) -> (float productionLevel) * 0.2

let private recordSong state studio (finishedSong: Finished<Song>) =
    let (Finished(song, quality)) = finishedSong

    (float quality)
    |> (+) (productionQualityImprovement state studio)
    |> int
    |> Math.clamp 0 100
    |> fun improvedQuality -> Recorded(song, improvedQuality * 1<quality>)

let private generatePaymentForOneSong state studio (band: Band) =
    let bandAccount = Band band.Id
    expense state bandAccount studio.PricePerSong

let private generateEffectsAfterBilling state studio band effects =
    let billingResult = generatePaymentForOneSong state studio band

    let timeEffects =
        StudioInteraction.CreateAlbum(studio, [])
        |> Interaction.Studio
        |> Queries.InteractionTime.timeRequired
        |> AdvanceTime.advanceDayMoment' state

    match billingResult with
    | Ok billingEffects -> Ok(effects @ billingEffects @ timeEffects)
    | Error error -> Error error

/// Applies the improvement in quality given by the producer of the given studio
/// and attempts to generate an album from the name and track list, applying the
/// validations of an album. Also checks the band's bank account and generates
/// the payment to the studio.
let startAlbum state studio band albumName initialSong =
    let (Recorded(song, quality)) = recordSong state studio initialSong
    let album = Recorded(song.Id, quality) |> Album.from band albumName

    [ AlbumStarted(band, UnreleasedAlbum album) ]
    |> generateEffectsAfterBilling state studio band

/// Applies the improvement in quality given by the producer of the studio and
/// attempts to add a song to the given unreleased album. Generates a payment
/// to the studio if the band has enough money, otherwise doesn't let them record
/// the song.
let recordSongForAlbum
    state
    studio
    (band: Band)
    (UnreleasedAlbum album)
    (song: Finished<Song>)
    =
    let recordedSong = recordSong state studio song
    let trackList = Queries.Albums.trackList state album

    let updatedAlbum =
        trackList @ [ recordedSong ] |> Album.updateTrackList album

    [ AlbumUpdated(band, UnreleasedAlbum updatedAlbum) ]
    |> generateEffectsAfterBilling state studio band
    |> Result.map (fun effects -> updatedAlbum, effects)
