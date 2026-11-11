module Duets.Data.Tests.Genres

open FsUnit
open NUnit.Framework

open Duets.Data
open Duets.Entities

[<Test>]
let ``every genre has at least one core instrument`` () =
    Genres.all
    |> List.iter (fun genre ->
        (Genres.instrumentsFor genre).Core |> should not' (be Empty))

[<Test>]
let ``no instrument is both core and common in the same genre`` () =
    Genres.all
    |> List.iter (fun genre ->
        let instruments = Genres.instrumentsFor genre

        Set.intersect (set instruments.Core) (set instruments.Common)
        |> should be Empty)

[<Test>]
let ``unknown genres use a rock band lineup`` () =
    (Genres.instrumentsFor "Polka").Core
    |> should equal [ Guitar; Bass; Drums; Vocals ]

[<Test>]
let ``genres are unique and sorted`` () =
    Genres.all |> should equal (Genres.all |> List.distinct |> List.sort)

[<Test>]
let ``no genre lists the same instrument twice`` () =
    Genres.all
    |> List.iter (fun genre ->
        let instruments = Genres.instrumentsFor genre
        let allInstruments = instruments.Core @ instruments.Common

        allInstruments |> should equal (List.distinct allInstruments))

[<Test>]
let ``radio stations only play existing genres`` () =
    Duets.Simulation.Queries.World.allCities
    |> List.collect (fun city ->
        Duets.Simulation.Queries.World.placesByTypeInCity
            city.Id
            PlaceTypeIndex.RadioStudio)
    |> List.iter (fun place ->
        match place.PlaceType with
        | PlaceType.RadioStudio studio ->
            Genres.all |> should contain studio.MusicGenre
        | _ -> ())
