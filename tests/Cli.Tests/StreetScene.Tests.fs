module Duets.Cli.Tests.StreetScene

open FsUnit
open NUnit.Framework

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Components.Scene.Street
open Duets.Cli.Text
open Duets.Common
open Duets.Data.Scenes
open Duets.Data.World.Cities
open Duets.Entities

let private parseOk text =
    match Template.parse text with
    | Ok template -> template
    | Error error -> failwith error

let private skyline =
    parseOk
        """
[art]
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
████████████████████████████████████████
"""

let private bar =
    World.Place.create "Bar" 50<quality> PlaceType.Bar Layouts.barRoomLayout "zone" "street"

let private lighting =
    { DayMoment = Midday
      Weather = WeatherCondition.Sunny }

let private build skin skyline =
    StreetScene.buildRow skin lighting 40 [ bar, true, [] ] true true ([], []) skyline 42

let private row y (grid: SceneGrid.Grid) =
    System.String [| for x in 0 .. grid.Width - 1 -> grid.Cells[y, x].Glyph |]

[<Test>]
let ``buildRow makes the sky taller to fit the skyline`` () =
    let skin = CitySkin.forCity Prague
    let withoutSkyline = build skin None
    let withSkyline = build skin (Some skyline)

    withSkyline.Height |> should be (greaterThan withoutSkyline.Height)
    withSkyline.Height |> should equal (skyline.Height + 1 + 2)

[<Test>]
let ``buildRow keeps the skyline out of the buildings and their gaps`` () =
    let grid = build (CitySkin.forCity Prague) (Some skyline)
    let groundRow = grid.Height - 2
    let facadeTop = groundRow - FacadeRenderer.heightOf (CitySkin.forCity Prague) bar
    let left = (40 - Facades.width) / 2

    row (facadeTop - 1) grid |> should equal (String.replicate 40 "█")

    grid.Cells[groundRow - 1, left - 1].Glyph |> should equal ' '
    grid.Cells[groundRow - 1, left + Facades.width].Glyph |> should equal ' '

[<Test>]
let ``drawTraffic does not draw anything in cities without vehicles`` () =
    let grid = SceneGrid.create 80 1
    let skin = { CitySkin.forCity Prague with Vehicles = [] }

    Furniture.drawTraffic grid skin lighting 0 42

    SceneGrid.toMarkup grid |> should equal [ "" ]

[<Test>]
let ``drawTraffic draws one vehicle every 40 columns during the day`` () =
    let grid = SceneGrid.create 80 1

    let skin =
        { CitySkin.forCity NewYork with
            Vehicles = [ "CAR", "white" ] }

    Furniture.drawTraffic grid skin lighting 0 42

    let text = row 0 grid
    (text.Length - text.Replace("CAR", "").Length) / 3 |> should equal 2

[<Test>]
let ``drawTraffic halves the traffic at night`` () =
    let grid = SceneGrid.create 80 1

    let skin =
        { CitySkin.forCity NewYork with
            Vehicles = [ "CAR", "white" ] }

    Furniture.drawTraffic grid skin { lighting with DayMoment = Night } 0 42

    let text = row 0 grid
    (text.Length - text.Replace("CAR", "").Length) / 3 |> should equal 1

[<Test>]
let ``drawProps stands the props on top of the ground`` () =
    let grid = SceneGrid.create 10 5
    let skin = { CitySkin.forCity NewYork with StreetProps = [ [ 'a', "white"; 'b', "white" ] ] }

    (* Use every column so that at least one gets a prop. *)
    Furniture.drawProps grid skin lighting [ 0 .. 9 ] 4 42

    let columns =
        [ 0 .. 9 ] |> List.filter (fun x -> grid.Cells[3, x].Glyph = 'a')

    columns |> should not' (be Empty)

    columns
    |> List.iter (fun x -> grid.Cells[2, x].Glyph |> should equal 'b')

[<Test>]
let ``every city has vehicles and street props`` () =
    Union.allCasesOf<CityId> ()
    |> List.filter (fun cityId ->
        let skin = CitySkin.forCity cityId
        skin.Vehicles.IsEmpty || skin.StreetProps.IsEmpty)
    |> should be Empty

let private hotel =
    World.Place.create
        "Hotel Name"
        50<quality>
        (PlaceType.Hotel { PricePerNight = 100m<dd> })
        Layouts.hotelLayout
        "zone"
        "street"

[<Test>]
let ``heightOf adds the extra floors and roof of the city`` () =
    let height cityId =
        FacadeRenderer.heightOf (CitySkin.forCity cityId) hotel

    height NewYork |> should equal (height Berlin + 1 + 2)
    height LosAngeles |> should equal (height Berlin - 1)

[<Test>]
let ``draw adds a vertical sign with the name of the place`` () =
    let skin = CitySkin.forCity Tokyo
    let grid = SceneGrid.create Facades.width 20

    FacadeRenderer.draw grid skin lighting 0 19 hotel true []

    let column =
        System.String [| for y in 0..19 -> grid.Cells[y, Facades.width - 2].Glyph |]

    column |> should haveSubstring "HOTEL"

[<Test>]
let ``street caption ends with a sentence about the city without places`` () =
    let street = World.Street.create "Street" StreetType.OneWay
    let caption = Caption.create Prague street 0 lighting 0 []

    SceneCaptions.city Prague SceneCaptions.Day
    |> List.exists (fun (sentence: string) -> caption.EndsWith(sentence))
    |> should equal true

[<Test>]
let ``every city has caption sentences for every phase`` () =
    let phases =
        [ SceneCaptions.Dawn
          SceneCaptions.Day
          SceneCaptions.Dusk
          SceneCaptions.Dark ]

    Union.allCasesOf<CityId> ()
    |> List.collect (fun cityId -> phases |> List.map (fun phase -> cityId, phase))
    |> List.filter (fun (cityId, phase) -> SceneCaptions.city cityId phase |> List.isEmpty)
    |> should be Empty
