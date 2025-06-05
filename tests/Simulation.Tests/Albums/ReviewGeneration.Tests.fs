module Duets.Simulation.Tests.Albums.ReviewGeneration

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Albums.ReviewGeneration

let album = Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 1.0

let addAlbumReleasedDaysAgo' days album state =
    let today = Queries.Calendar.today state
    let daysAgo = today |> Calendar.Ops.addDays (-days)
    let album = { album with ReleaseDate = daysAgo }
    addReleasedAlbum dummyBand.Id album state

let rec addAlbumReleasedDaysAgo days state =
    addAlbumReleasedDaysAgo' days album state

let addAlbumWithNoReviews state = addAlbumReleasedDaysAgo 3<days> state

let addAlbumWithReviews state =
    let today = Queries.Calendar.today state
    let releaseDate = today |> Calendar.Ops.addDays -5<days>

    let album =
        { album with
            ReleaseDate = releaseDate
            Reviews = [ { Reviewer = Metacritic; Score = 80 } ] }

    addReleasedAlbum dummyBand.Id album state

let addAlbumWithQuality quality =
    let unreleasedAlbum =
        { Album =
            Recorded(dummySong.Id, quality * 1<quality>)
            |> Album.from dummyBand "Testing"
          SelectedProducer = SelectedProducer.StudioProducer }

    let releasedAlbum =
        Album.Released.fromUnreleased unreleasedAlbum dummyToday 1.0

    addAlbumReleasedDaysAgo' 3<days> releasedAlbum

[<Test>]
let ``generateReviews should return empty if band has not released any albums``
    ()
    =
    State.generateOne
        { State.defaultOptions with
            BandFansMin =
                Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
            BandFansMax = 10000<fans> }
    |> generateReviewsForLatestAlbums
    |> should haveLength 0

[<Test>]
let ``generateReviews should return empty if band does not have the minimum required fans``
    ()
    =
    State.generateN
        { State.defaultOptions with
            BandFansMin = 0<fans>
            BandFansMax =
                Config.MusicSimulation.minimumFanBaseForReviews * 1<fans> }
        50
    |> List.iter (fun state ->
        state
        |> addReleasedAlbum dummyBand.Id album
        |> generateReviewsForLatestAlbums
        |> should haveLength 0)

[<Test>]
let ``generateReviews should return empty if band does not have any albums released three days ago``
    ()
    =
    [ 4; 1; 10; 15; 2 ]
    |> List.iter (fun days ->
        State.generateOne
            { State.defaultOptions with
                BandFansMin =
                    Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
                BandFansMax = 10000<fans> }
        |> addAlbumReleasedDaysAgo (days * 1<days>)
        |> generateReviewsForLatestAlbums
        |> should haveLength 0)

[<Test>]
let ``generateReviews should return empty if the band's albums already have reviews``
    ()
    =
    State.generateN
        { State.defaultOptions with
            BandFansMin =
                Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
            BandFansMax = 10000<fans> }
        50
    |> List.iter (fun state ->
        state
        |> addAlbumWithReviews
        |> generateReviewsForLatestAlbums
        |> should haveLength 0)

[<Test>]
let ``generateReviews should return effects if the day was three days ago regardless of the time``
    ()
    =
    [ EarlyMorning; Morning; Afternoon; Evening; Night ]
    |> List.iter (fun dayMoment ->
        let releaseDate =
            dummyToday
            |> Calendar.Ops.addDays -3<days>
            |> Calendar.Transform.changeDayMoment dayMoment

        State.generateOne
            { State.defaultOptions with
                BandFansMin =
                    Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
                BandFansMax = 10000<fans> }
        |> addReleasedAlbum
            dummyBand.Id
            { album with ReleaseDate = releaseDate }
        |> generateReviewsForLatestAlbums
        |> should haveLength 1)

[<Test>]
let ``generateReviews should return effects for each album released three days ago without reviews``
    ()
    =
    State.generateOne
        { State.defaultOptions with
            BandFansMin =
                Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
            BandFansMax = 10000<fans> }
    |> addAlbumWithNoReviews
    |> generateReviewsForLatestAlbums
    |> should haveLength 1

let private testReviewScore reviewerId assertFn quality =
    let effects =
        State.generateOne
            { State.defaultOptions with
                BandFansMin =
                    Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>
                BandFansMax = 10000<fans> }
        |> addAlbumWithQuality quality
        |> generateReviewsForLatestAlbums

    let review =
        match List.head effects with
        | AlbumReviewsReceived(_, releasedAlbum) -> releasedAlbum.Reviews
        | _ -> failwith "How did we end up here?"
        |> List.find (fun review -> review.Reviewer = reviewerId)

    review.Score |> assertFn

[<Test>]
let ``generateReviews should generate RateYourMusic review with a 65 if quality is between 50 and 80``
    ()
    =
    [ 50; 75; 65; 66; 51; 80 ]
    |> List.iter (
        testReviewScore RateYourMusic (fun score -> score |> should equal 65)
    )

[<Test>]
let ``generateReviews should generate RateYourMusic review with a score around the album quality``
    ()
    =
    [ 20, 17; 5, 5; 99, 85; 100, 85; 81, 69 ]
    |> List.iter (fun (quality, expected) ->
        testReviewScore
            RateYourMusic
            (fun score -> score |> should equal expected)
            quality)

[<Test>]
let ``generateReviews should generate Sputnik review with a score around the album quality, randomly adjusted by 0.5 - 0.8 points``
    ()
    =
    [ 20; 30; 1; 85; 90; 100; 34; 65; 55 ]
    |> List.iter (fun quality ->
        testReviewScore SputnikMusic (should (equalWithin 0.8) quality) quality)

[<Test>]
let ``generateReviews should generate Metacritic review with a score around the album quality, randomly adjusted by 10 points``
    ()
    =
    [ 20; 30; 1; 85; 90; 100; 34; 65; 55 ]
    |> List.iter (fun quality ->
        testReviewScore Metacritic (should (equalWithin 10) quality) quality)

[<Test>]
let ``generateReviews should generate Pitchfork review with a score around the album quality, randomly adjusted by 1 - 10 points``
    ()
    =
    [ 20; 30; 1; 85; 90; 100; 34; 65; 55 ]
    |> List.iter (fun quality ->
        testReviewScore Pitchfork (should (equalWithin 10) quality) quality)
