module Duets.Simulation.Tests.Cinema.Queries

open NUnit.Framework
open FsUnit

open Duets.Data
open Duets.Entities
open Duets.Simulation.Queries

let private makeDate year season =
    { Day = 1<days>
      Season = season
      Year = year
      DayMoment = Morning }

// ── currentMovie ─────────────────────────────────────────────────────────────

[<Test>]
let ``currentMovie returns Some for a valid date`` () =
    Cinema.currentMovie (makeDate 2024<years> Spring)
    |> should not' (equal None)

[<Test>]
let ``currentMovie is deterministic: same date always returns the same movie``
    ()
    =
    let date = makeDate 2023<years> Summer
    Cinema.currentMovie date |> should equal (Cinema.currentMovie date)

[<Test>]
let ``currentMovie all four seasons in a year return distinct movies`` () =
    let movies =
        [ Spring; Summer; Autumn; Winter ]
        |> List.map (makeDate 2024<years> >> Cinema.currentMovie)

    movies |> List.distinct |> should haveLength 4

[<Test>]
let ``currentMovie returns a different movie for each consecutive year in the same season``
    ()
    =
    let springMovieForYear year =
        Cinema.currentMovie (makeDate year Spring)

    springMovieForYear 2024<years>
    |> should not' (equal (springMovieForYear 2025<years>))

    springMovieForYear 2025<years>
    |> should not' (equal (springMovieForYear 2026<years>))

    springMovieForYear 2026<years>
    |> should not' (equal (springMovieForYear 2027<years>))

[<Test>]
let ``currentMovie covers all movies across a full rotation`` () =
    // With the deterministic modulo algorithm, each season advances the index
    // by 1 position, so (movieCount / 4) + 2 years covers every movie exactly once.
    let allMovies = Movies.all
    let totalMovies = List.length allMovies
    let yearsNeeded = totalMovies / 4 + 2
    let seasons = [ Spring; Summer; Autumn; Winter ]

    let seen =
        [ for y in 0 .. yearsNeeded - 1 -> (2000 + y) * 1<years> ]
        |> List.collect (fun year ->
            seasons |> List.choose (Cinema.currentMovie << makeDate year))
        |> List.map (fun m -> m.Title)
        |> List.distinct

    seen |> should haveLength totalMovies

[<Test>]
let ``currentMovie returns a movie with quality between 1 and 10`` () =
    match Cinema.currentMovie (makeDate 2024<years> Autumn) with
    | Some movie ->
        movie.Quality |> should be (greaterThanOrEqualTo 1)
        movie.Quality |> should be (lessThanOrEqualTo 10)
    | None -> Assert.Fail "Expected a movie to be returned"

[<Test>]
let ``currentMovie changes every season across multiple years`` () =
    // Gather all (movie, next movie) adjacent pairs across 10 years and confirm
    // no two consecutive seasons show the same film.
    let seasons = [ Spring; Summer; Autumn; Winter ]

    let movies =
        [ for y in 2020..2029 -> y * 1<years> ]
        |> List.collect (fun year ->
            seasons |> List.choose (Cinema.currentMovie << makeDate year))

    movies
    |> List.pairwise
    |> List.forall (fun (a, b) -> a <> b)
    |> should be True

// ── ticketPrice ──────────────────────────────────────────────────────────────

[<Test>]
let ``ticketPrice returns a positive amount`` () =
    World.cityById LosAngeles
    |> Cinema.ticketPrice
    |> should be (greaterThan 0m<dd>)

[<Test>]
let ``ticketPrice scales proportionally with cost of living`` () =
    let baseCity = World.cityById London

    let cheapCity =
        { baseCity with
            CostOfLiving = 1.0<costOfLiving> }

    let expensiveCity =
        { baseCity with
            CostOfLiving = 2.0<costOfLiving> }

    Cinema.ticketPrice expensiveCity
    |> should equal (Cinema.ticketPrice cheapCity * 2m)

[<Test>]
let ``ticketPrice is higher for more expensive cities`` () =
    let newYork = World.cityById NewYork
    let madrid = World.cityById Madrid

    Cinema.ticketPrice newYork
    |> should be (greaterThan (Cinema.ticketPrice madrid))
