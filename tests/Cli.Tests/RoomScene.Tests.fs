module Duets.Cli.Tests.RoomScene

open FsUnit
open NUnit.Framework

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Components.Scene.Room
open Duets.Cli.Text
open Duets.Data.Scenes
open Duets.Entities

let private parseOk text =
    match Template.parse text with
    | Ok template -> template
    | Error error -> failwith error

let private template =
    parseOk
        """
[art]
┌──88──www─┐
│ p  nnnn  │
│ cccccccc │
│   TT ii  │
└───0000───┘

[legend]
default = grey50

[slots]
8 = exit north else ─
0 = exit street else ─
w = window
p = player
n = npc
c = crowd
T = prop a | b | c
i = item
"""

let private windowless =
    parseOk
        """
[art]
┌────┐
│ p  │
└────┘

[slots]
p = player
"""

let private context: RoomRenderer.RoomContext =
    { Skin = CitySkin.forCity Prague
      Lighting =
        { DayMoment = Morning
          Weather = WeatherCondition.Sunny }
      Quality = 50<quality>
      Seed = 42
      KnownPeople = 0
      UnknownPeople = 0
      BandMembers = 0
      Items = []
      Exits = []
      Crowd = 0.0
      ConcertOngoing = false
      Cuisine = None }

let private glyphs (grid: SceneGrid.Grid) =
    [ for y in 0 .. grid.Height - 1 do
          for x in 0 .. grid.Width - 1 do
              grid.Cells[y, x].Glyph ]

let private count glyph grid =
    glyphs grid |> List.filter ((=) glyph) |> List.length

let private row y (grid: SceneGrid.Grid) =
    System.String [| for x in 0 .. grid.Width - 1 -> grid.Cells[y, x].Glyph |]

[<Test>]
let ``build draws the player`` () =
    let grid = RoomRenderer.build context template

    count SceneStyles.People.player grid |> should equal 1

[<Test>]
let ``build draws known and unknown people`` () =
    let grid =
        RoomRenderer.build
            { context with
                KnownPeople = 1
                UnknownPeople = 2 }
            template

    count SceneStyles.People.known grid |> should equal 1
    count SceneStyles.People.unknown grid |> should equal 2

[<Test>]
let ``build caps people to the available spots`` () =
    let grid = RoomRenderer.build { context with UnknownPeople = 20 } template

    count SceneStyles.People.unknown grid |> should equal 4

[<Test>]
let ``build draws the fallback when there are no exits`` () =
    let grid = RoomRenderer.build context template

    (row 0 grid).Substring(0, 7) |> should equal "┌──────"
    row 4 grid |> should equal "└──────────┘"

[<Test>]
let ``build draws arrows only for the available exits`` () =
    let grid =
        RoomRenderer.build
            { context with
                Exits = [ ExitTarget.Street ] }
            template

    (row 0 grid).Substring(0, 7) |> should equal "┌──────"
    count SceneStyles.Room.streetExit grid |> should equal 1

[<Test>]
let ``build fills the crowd based on how full the venue is`` () =
    let emptyGrid = RoomRenderer.build context template
    let fullGrid = RoomRenderer.build { context with Crowd = 1.0 } template

    row 2 emptyGrid |> should equal "│          │"
    row 2 fullGrid |> should not' (haveSubstring "   ")

[<Test>]
let ``build picks props based on the quality of the place`` () =
    let low = RoomRenderer.build { context with Quality = 0<quality> } template
    let high = RoomRenderer.build { context with Quality = 100<quality> } template

    row 3 low |> should haveSubstring "a"
    row 3 high |> should haveSubstring "c"

[<Test>]
let ``build is stable for the same context`` () =
    let busy =
        { context with
            KnownPeople = 1
            UnknownPeople = 2
            Crowd = 0.5 }

    RoomRenderer.build busy template
    |> SceneGrid.toMarkup
    |> should equal (RoomRenderer.build busy template |> SceneGrid.toMarkup)

[<Test>]
let ``caption is stable for the same context`` () =
    Caption.create context template RoomType.Bar
    |> should equal (Caption.create context template RoomType.Bar)

[<Test>]
let ``caption describes the crowd while playing on stage`` () =
    let concert =
        { context with
            ConcertOngoing = true
            Crowd = 1.0 }

    let caption = Caption.create concert windowless RoomType.Stage

    SceneCaptions.crowd 1.0 |> should contain caption

[<Test>]
let ``caption mentions the weather only in rooms with windows`` () =
    let rainy =
        { context with
            Lighting =
                { context.Lighting with
                    Weather = WeatherCondition.Rainy } }

    let windowSentences =
        SceneCaptions.throughWindow WeatherCondition.Rainy SceneCaptions.Day

    let mentionsWeather caption =
        windowSentences
        |> List.exists (fun (sentence: string) ->
            (caption: string).EndsWith(sentence))

    Caption.create rainy template RoomType.Bar
    |> mentionsWeather
    |> should equal true

    Caption.create rainy windowless RoomType.Bar
    |> mentionsWeather
    |> should equal false
