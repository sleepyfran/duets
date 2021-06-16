module Simulation.Tests.Studio.ReleaseAlbum

open FSharp.Data.UnitSystems.SI.UnitNames
open FsUnit
open NUnit.Framework
open Test.Common

open Entities
open Simulation.Studio.ReleaseAlbum

let band =
    { dummyBand with
          Fame = 50
          Genre = "Test" }

let state =
    { dummyState with
          GenreMarkets =
              [ ("Test", { MarketPoint = 3.0; Fluctuation = 1.0 }) ]
              |> Map.ofList }

let single = { dummyAlbum with Type = Single }
let ep = { dummyAlbum with Type = EP }
let lp = { dummyAlbum with Type = LP }

[<Test>]
let ``releaseAlbum should create effect with correct streams and hype for single``
    ()
    =
    releaseAlbum state band (UnreleasedAlbum single)
    |> should
        equal
        (AlbumReleased(
            band,
            { Album = single
              ReleaseDate = dummyToday
              MaxDailyStreams = 3000000
              Hype = 1.0 }
        ))

[<Test>]
let ``releaseAlbum should create effect with correct streams and hype for EP``
    ()
    =
    releaseAlbum state band (UnreleasedAlbum ep)
    |> should
        equal
        (AlbumReleased(
            band,
            { Album = ep
              ReleaseDate = dummyToday
              MaxDailyStreams = 2100000
              Hype = 1.0 }
        ))

[<Test>]
let ``releaseAlbum should create effect with correct streams and hype for LP``
    ()
    =
    releaseAlbum state band (UnreleasedAlbum lp)
    |> should
        equal
        (AlbumReleased(
            band,
            { Album = lp
              ReleaseDate = dummyToday
              MaxDailyStreams = 3000000
              Hype = 1.0 }
        ))

[<Test>]
let ``releaseAlbum should properly handle low fame and scores`` () =
    let lowFameBand = { band with Fame = 1 }

    let lowScoreTrackList =
        [ RecordedSong(
            FinishedSong
                { Id = SongId <| Identity.create ()
                  Name = "Test 1"
                  Length = 100<second>
                  VocalStyle = Instrumental
                  Genre = "Test" },
            2<quality>
          )
          RecordedSong(
              FinishedSong
                  { Id = SongId <| Identity.create ()
                    Name = "Test 2"
                    Length = 100<second>
                    VocalStyle = Instrumental
                    Genre = "Test" },
              1<quality>
          ) ]

    let lowScoreAlbum =
        { lp with
              TrackList = lowScoreTrackList }

    releaseAlbum state lowFameBand (UnreleasedAlbum lowScoreAlbum)
    |> should
        equal
        (AlbumReleased(
            lowFameBand,
            { Album = lowScoreAlbum
              ReleaseDate = dummyToday
              MaxDailyStreams = 2400
              Hype = 1.0 }
        ))
