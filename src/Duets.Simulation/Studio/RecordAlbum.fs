module Duets.Simulation.Studio.RecordAlbum

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Time

let private productionQualityImprovement state studio selectedProducer =
    let selectedProducerId =
        match selectedProducer with
        | SelectedProducer.PlayableCharacter ->
            let character = Queries.Characters.playableCharacter state
            character.Id
        | SelectedProducer.StudioProducer -> studio.Producer.Id

    Queries.Skills.characterSkillWithLevel
        state
        selectedProducerId
        SkillId.MusicProduction
    |> fun (_, productionLevel) -> (float productionLevel) * 0.2

let private recordSong
    state
    studio
    selectedProducer
    (finishedSong: Finished<Song>)
    =
    let (Finished(song, quality)) = finishedSong

    (float quality)
    |> (+) (productionQualityImprovement state studio selectedProducer)
    |> int
    |> Math.clamp 0 100
    |> fun improvedQuality -> Recorded(song, improvedQuality * 1<quality>)

let private generatePaymentForOneSong
    state
    studio
    selectedProducer
    (band: Band)
    =
    let bandAccount = Band band.Id

    let totalPrice =
        match selectedProducer with
        | SelectedProducer.PlayableCharacter ->
            studio.PricePerSong (* Includes only the recording fee. *)
        | SelectedProducer.StudioProducer ->
            studio.PricePerSong
            * 2m (* Includes the recording fee and the production fee. *)

    expense state bandAccount totalPrice

let private generateEffectsAfterBilling
    state
    studio
    selectedProducer
    band
    effects
    =
    let billingResult =
        generatePaymentForOneSong state studio selectedProducer band

    match billingResult with
    | Ok billingEffects -> Ok(effects @ billingEffects)
    | Error error -> Error error

/// Applies the improvement in quality given by the producer of the given studio
/// and attempts to generate an album from the name and track list, applying the
/// validations of an album. Also checks the band's bank account and generates
/// the payment to the studio.
let startAlbum state studio selectedProducer band albumName initialSong =
    let (Recorded(song, quality)) =
        recordSong state studio selectedProducer initialSong

    let album = Recorded(song.Id, quality) |> Album.from band albumName

    let unreleasedAlbum =
        { Album = album
          SelectedProducer = selectedProducer }

    [ AlbumStarted(band, unreleasedAlbum) ]
    |> generateEffectsAfterBilling state studio selectedProducer band

/// Applies the improvement in quality given by the producer of the studio and
/// attempts to add a song to the given unreleased album. Generates a payment
/// to the studio if the band has enough money, otherwise doesn't let them record
/// the song.
let recordSongForAlbum
    state
    studio
    (band: Band)
    (unreleasedAlbum: UnreleasedAlbum)
    (song: Finished<Song>)
    =
    let recordedSong =
        recordSong state studio unreleasedAlbum.SelectedProducer song

    let trackList = Queries.Albums.trackList state unreleasedAlbum.Album

    let updatedAlbum =
        trackList @ [ recordedSong ]
        |> Album.updateTrackList unreleasedAlbum.Album

    let updatedUnreleasedAlbum =
        { unreleasedAlbum with
            Album = updatedAlbum }

    [ AlbumSongAdded(band, updatedUnreleasedAlbum, recordedSong)
      AlbumUpdated(band, updatedUnreleasedAlbum) ]
    |> generateEffectsAfterBilling
        state
        studio
        unreleasedAlbum.SelectedProducer
        band
    |> Result.map (fun effects -> updatedAlbum, effects)
