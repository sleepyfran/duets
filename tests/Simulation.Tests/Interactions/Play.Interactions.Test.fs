module Duets.Simulation.Tests.Interactions.Play

open Duets.Entities
open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Data
open Duets.Simulation
open Duets.Simulation.Interactions

[<Test>]
let ``playing on anything other than a game console, dartboard or a billboard returns ActionNotPossible``
    ()
    =
    [ Items.Furniture.Bed.ikeaBed
      Items.Furniture.Stove.lgStove
      Items.Electronics.Tv.lgTv ]
    |> List.iter (fun item ->
        Items.perform dummyState (fst item) ItemInteraction.Play
        |> Result.unwrapError
        |> should equal Items.ActionNotPossible)

[<Test>]
let ``playing with a video-game console returns a video-game result`` () =
    Items.perform
        dummyState
        (fst Items.Electronics.GameConsole.xbox)
        ItemInteraction.Play
    |> Result.unwrap
    |> List.filter (function
        | PlayResult(PlayResult.VideoGame) -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``playing in a dartboard has a 50% chance of returning a winning dart result``
    ()
    =
    use _ = changeToStaticRandom 40

    Items.perform
        dummyState
        (fst Items.Electronics.Dartboard.dartboard)
        ItemInteraction.Play
    |> Result.unwrap
    |> List.filter (function
        | PlayResult(PlayResult.Darts(SimpleResult.Win)) -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``playing in a dartboard has a 50% chance of returning a losing dart result``
    ()
    =
    use _ = changeToStaticRandom 100

    Items.perform
        dummyState
        (fst Items.Electronics.Dartboard.dartboard)
        ItemInteraction.Play
    |> Result.unwrap
    |> List.filter (function
        | PlayResult(PlayResult.Darts(SimpleResult.Lose)) -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``playing in a billiard has a 50% chance of returning a winning pool result``
    ()
    =
    use _ = changeToStaticRandom 40

    Items.perform
        dummyState
        (fst Items.Furniture.BilliardTable.sonomaTable)
        ItemInteraction.Play
    |> Result.unwrap
    |> List.filter (function
        | PlayResult(PlayResult.Pool(SimpleResult.Win)) -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``playing in a billiard has a 50% chance of returning a losing pool result``
    ()
    =
    use _ = changeToStaticRandom 100

    Items.perform
        dummyState
        (fst Items.Furniture.BilliardTable.sonomaTable)
        ItemInteraction.Play
    |> Result.unwrap
    |> List.filter (function
        | PlayResult(PlayResult.Pool(SimpleResult.Lose)) -> true
        | _ -> false)
    |> should haveLength 1
