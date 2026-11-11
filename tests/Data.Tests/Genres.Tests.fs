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
let ``unknown genres consider every instrument core`` () =
    (Genres.instrumentsFor "Polka").Core |> should equal Roles.all
