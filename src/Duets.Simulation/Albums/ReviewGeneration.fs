module Duets.Simulation.Albums.ReviewGeneration

open Aether
open Duets.Common
open Duets.Common.Operators
open Duets.Entities
open Duets.Simulation

let private generateRymReview albumQuality =
    let score =
        if albumQuality >=< (50, 80) then
            65
        else
            (float albumQuality) - (float albumQuality * 0.15)
            |> Math.ceilToNearest

    Album.Review.create RateYourMusic score

let private generateSputnikReview albumQuality =
    let positiveReview = RandomGen.choice [ true; false ]

    let score =
        if positiveReview then
            float albumQuality + 0.5
        else
            float albumQuality - 0.8
        |> Math.ceilToNearest

    Album.Review.create SputnikMusic score

let private generateMetacriticReview albumQuality =
    let randomVariance = RandomGen.genBetween -10 10

    let score = albumQuality + randomVariance

    Album.Review.create Metacritic score

let private generatePitchforkReview albumQuality =
    let randomVariance = RandomGen.genBetween 1 10

    let score = albumQuality + randomVariance

    Album.Review.create Pitchfork score

let private generateReviewsForAlbum (band: Band) releasedAlbum =
    let albumQuality = Queries.Albums.quality releasedAlbum.Album

    [ generateRymReview albumQuality
      generateSputnikReview albumQuality
      generateMetacriticReview albumQuality
      generatePitchforkReview albumQuality ]
    |> List.fold
        (fun acc review ->
            let addReview reviews = review :: reviews

            Optic.map Lenses.Album.reviews_ addReview acc)
        releasedAlbum
    |> Tuple.two band
    |> AlbumReviewsReceived

let private generateReviewsForBandAlbums state bandId albums =
    let band = Queries.Bands.byId state bandId
    let fanBase = Queries.Bands.totalFans' band

    let minimumFanBaseForReviews =
        Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>

    if fanBase >= minimumFanBaseForReviews then
        albums
        |> List.fold
            (fun acc album ->
                if List.isEmpty album.Reviews then
                    generateReviewsForAlbum band album :: acc
                else
                    acc)
            []
    else
        []

/// Retrieves all albums released in the past three days that don't have any reviews
/// yet and generates them based on the score of the album and the source of the
/// review, only if the band has the minimum amount of fame required to have
/// reviews generated for them.
let generateReviewsForLatestAlbums (state: State) =
    Queries.Albums.releaseInLast state 3<days>
    |> Map.fold
        (fun acc bandId albums ->
            acc @ generateReviewsForBandAlbums state bandId albums)
        []

/// Retrieves all albums released by bands that have the minimum amount of fame
/// and generates the reviews for all their albums.
let generateReviewsForBand state bandId =
    Queries.Albums.releasedByBand state bandId
    |> generateReviewsForBandAlbums state bandId
