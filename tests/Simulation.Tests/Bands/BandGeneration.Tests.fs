module Duets.Simulation.Tests.Bands.Generation

open Aether
open Aether.Operators
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common.Operators
open Duets.Entities
open Duets.Simulation

let state =
    let genreMarkets =
        Duets.Data.Genres.all
        |> List.map (fun genre ->
            genre, { MarketPoint = 4.0; Fluctuation = 1.0 })
        |> Map.ofList

    { State.generateOne State.defaultOptions with
        GenreMarkets = genreMarkets }

[<TestFixture>]
type ``addInitialBandsToState``() =
    let simulatedBands =
        state
        |> Bands.Generation.addInitialBandsToState
        |> Optic.get (Lenses.State.bands_ >-> Lenses.Bands.simulatedBands_)

    [<Test>]
    member _.``generates 150 bands in total``() =
        simulatedBands |> should haveCount 150

    [<Test>]
    member _.``should have 50 bands of low fame level``() =
        simulatedBands
        |> Map.filter (fun _ band -> band.Fans >=< (400, 4000))
        |> should haveCount 50

    [<Test>]
    member _.``should generate 50 bands of low-medium fame level``() =
        simulatedBands
        |> Map.filter (fun _ band -> band.Fans >=< (4400, 40000))
        |> should haveCount 50

    [<Test>]
    member _.``should generate 25 bands of medium fame level``() =
        simulatedBands
        |> Map.filter (fun _ band -> band.Fans >=< (40001, 400000))
        |> should haveCount 25

    [<Test>]
    member _.``should generate 15 bands of medium-high fame level``() =
        simulatedBands
        |> Map.filter (fun _ band -> band.Fans >=< (400000, 2000000))
        |> should haveCount 15

    [<Test>]
    member _.``should generate 10 bands of high fame level``() =
        simulatedBands
        |> Map.filter (fun _ band -> band.Fans >=< (2000000, 4000000))
        |> should haveCount 10
