module Duets.Data.Tests.Scenes

open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Data
open Duets.Data.Scenes
open Duets.Data.World.Cities
open Duets.Entities

let private parseOk text =
    match Template.parse text with
    | Ok template -> template
    | Error error -> failwith error

let private validTemplate =
    """
# A comment before any section.
[art]
┌──88──┐
│ p  ii│
└─0000─┘

[legend]
┌─┐│└┘ = $wall
default = grey50

[slots]
8 = exit north else ─
0 = exit street else ─
p = player
i = item sleepable
"""

let private bar =
    World.Place.create
        "Café Ñandú"
        50<quality>
        PlaceType.Bar
        Layouts.barRoomLayout
        "zone"
        "street"

[<Test>]
let ``parse reads the art, legend and slots`` () =
    let template = parseOk validTemplate

    template.Art |> should equal [ "┌──88──┐"; "│ p  ii│"; "└─0000─┘" ]
    template.Width |> should equal 8
    template.Height |> should equal 3
    template.Legend |> Map.find '┐' |> should equal "$wall"
    template.DefaultStyle |> should equal (Some "grey50")

    template.Slots
    |> Map.find '8'
    |> should equal
        { Kind = SlotKind.Exit(ExitTarget.Room North)
          Fallback = '─' }

    template.Slots
    |> Map.find 'i'
    |> should equal
        { Kind = SlotKind.Item ItemCategory.Sleepable
          Fallback = ' ' }

[<Test>]
let ``parse pads the art lines to the same width`` () =
    let template = parseOk "[art]\nab\nabcd\n"

    template.Art |> should equal [ "ab  "; "abcd" ]

[<Test>]
let ``parse reads prop tiers and alternatives`` () =
    let template = parseOk "[art]\nAAA\n[slots]\nA = prop _ ▤ | ▤▤ | ▣▤▣ ▤▣▤"

    template.Slots
    |> Map.find 'A'
    |> _.Kind
    |> should
        equal
        (SlotKind.Prop [ [ ""; "▤" ]; [ "▤▤" ]; [ "▣▤▣"; "▤▣▤" ] ])

[<Test>]
let ``parse fails when a slot is not used in the art`` () =
    Template.parse "[art]\nabc\n[slots]\nz = npc"
    |> Result.isError
    |> should equal true

[<Test>]
let ``parse fails with an unknown slot kind`` () =
    Template.parse "[art]\nabc\n[slots]\na = dragon"
    |> Result.isError
    |> should equal true

[<Test>]
let ``parse fails without art`` () =
    Template.parse "[legend]\na = red"
    |> Result.isError
    |> should equal true

[<Test>]
let ``parse fails with content outside of a section`` () =
    Template.parse "hello\n[art]\nabc" |> Result.isError |> should equal true

[<Test>]
let ``runsOf groups consecutive cells of a row`` () =
    let template = parseOk validTemplate

    Template.runsOf template '0'
    |> should equal [ { X = 2; Y = 2; Length = 4 } ]

    Template.runsOf template 'i'
    |> should equal [ { X = 5; Y = 1; Length = 2 } ]

[<Test>]
let ``cellsOf returns every cell with the marker`` () =
    let template = parseOk validTemplate

    Template.cellsOf template '8' |> should equal [ (3, 0); (4, 0) ]

[<Test>]
let ``tierFor spreads the quality evenly between the tiers`` () =
    Template.tierFor 0<quality> 3 |> should equal 0
    Template.tierFor 33<quality> 3 |> should equal 0
    Template.tierFor 34<quality> 3 |> should equal 1
    Template.tierFor 100<quality> 3 |> should equal 2
    Template.tierFor 100<quality> 1 |> should equal 0
    Template.tierFor 150<quality> 2 |> should equal 1

[<Test>]
let ``toKey creates lowercase keys without diacritics`` () =
    toKey "NewYork" |> should equal "new-york"
    toKey "Café Ñandú" |> should equal "cafe-nandu"
    toKey "O2 Arena" |> should equal "o2-arena"
    toKey "RehearsalRoom" |> should equal "rehearsal-room"

[<Test>]
let ``candidateKeys go from the most specific to the most generic`` () =
    candidateKeys NewYork bar RoomType.Bar
    |> should
        equal
        [ "places/new-york/cafe-nandu/bar"
          "cities/new-york/bar.bar"
          "cities/new-york/bar"
          "rooms/bar.bar"
          "rooms/bar" ]

[<Test>]
let ``candidateKeys ignore the cuisine of restaurants`` () =
    candidateKeys Prague bar (RoomType.Restaurant Czech)
    |> List.last
    |> should equal "rooms/restaurant"

[<Test>]
let ``tryResolve picks the most specific template available`` () =
    let template art = parseOk $"[art]\n{art}"

    let templates =
        [ "rooms/bar", template "default"
          "cities/new-york/bar", template "city"
          "places/new-york/cafe-nandu/bar", template "place" ]
        |> Map.ofList

    let resolveIn templates cityId =
        tryResolve templates cityId bar RoomType.Bar
        |> Option.map (_.Art >> List.head)

    resolveIn templates NewYork |> should equal (Some "place")

    resolveIn (templates |> Map.remove "places/new-york/cafe-nandu/bar") NewYork
    |> should equal (Some "city")

    resolveIn templates Prague |> should equal (Some "default")
    resolveIn Map.empty Prague |> should equal None

[<Test>]
let ``all templates of the game are valid`` () =
    (* Throws if any template is broken. *)
    Scenes.all.Value |> Map.isEmpty |> should equal false

[<Test>]
let ``every room type except streets has a default template`` () =
    let place =
        World.Place.create
            "Test"
            50<quality>
            PlaceType.Home
            Layouts.homeLayout
            "zone"
            "street"

    let roomTypes =
        Union.allCasesOf<RoomType> ()
        |> List.filter ((<>) RoomType.Street)
        |> List.append [ RoomType.Restaurant Italian ]

    roomTypes
    |> List.filter (fun roomType ->
        Scenes.resolve Prague place roomType |> Option.isNone)
    |> should be Empty

[<Test>]
let ``tryResolveSkyline prefers the zone over the city`` () =
    let template art = parseOk $"[art]\n{art}"
    let zone = World.Zone.create "Lower Manhattan"

    let templates =
        [ "skylines/new-york", template "city"
          "skylines/new-york.lower-manhattan", template "zone" ]
        |> Map.ofList

    let resolveIn templates cityId zone =
        tryResolveSkyline templates cityId zone |> Option.map (_.Art >> List.head)

    resolveIn templates NewYork zone |> should equal (Some "zone")

    resolveIn templates NewYork (World.Zone.create "Brooklyn")
    |> should equal (Some "city")

    resolveIn templates Prague zone |> should equal None

[<Test>]
let ``tryResolveSkyline uses the airport one in zones with an airport`` () =
    let template art = parseOk $"[art]\n{art}"

    let templates =
        [ "skylines/london", template "city"
          "skylines/airport", template "airport" ]
        |> Map.ofList

    let resolveIn zone =
        tryResolveSkyline templates London zone
        |> Option.map (_.Art >> List.head)

    resolveIn London.Heathrow.zone |> should equal (Some "airport")

    resolveIn (World.Zone.create "Camden") |> should equal (Some "city")

[<Test>]
let ``every city has a skyline`` () =
    Union.allCasesOf<CityId> ()
    |> List.filter (fun cityId ->
        Scenes.resolveSkyline cityId (World.Zone.create "Zone") |> Option.isNone)
    |> should be Empty

[<Test>]
let ``every zone skyline matches a zone of its city`` () =
    let zoneKeys =
        Duets.Data.World.World.get.Cities
        |> List.ofMapValues
        |> List.collect (fun city ->
            city.Zones
            |> List.ofMapValues
            |> List.map (fun zone -> skylineKeys city.Id zone |> List.head))
        |> Set.ofList

    Scenes.all.Value
    |> Map.keys
    |> Seq.filter (fun key -> key.StartsWith "skylines/" && key.Contains '.')
    |> Seq.filter (fun key -> not (zoneKeys.Contains key))
    |> List.ofSeq
    |> should be Empty

let private parseLandmarkOk text =
    match Landmark.parse text with
    | Ok landmark -> landmark
    | Error error -> failwith error

let private isLandmarkError text =
    match Landmark.parse text with
    | Ok _ -> false
    | Error _ -> true

[<Test>]
let ``Landmark.parse reads every section`` () =
    let landmark =
        parseLandmarkOk
            """
# A comment.
[art]
 ▲
███

[legend]
★ = gold1
default = grey30

[options]
layer = front

[sidewalk]
─★─

[road]
┬┬┬
~~~

[props]
♣
│ ║

[captions]
First caption.
Second caption.
"""

    landmark.Art |> Option.map _.Art |> should equal (Some [ " ▲ "; "███" ])
    landmark.Layer |> should equal LandmarkLayer.Front
    landmark.Sidewalk |> should equal (Some "─★─")
    landmark.Road |> should equal [ "┬┬┬"; "~~~" ]
    landmark.Props |> should equal [ "♣│"; "║" ]
    landmark.Captions |> should equal [ "First caption."; "Second caption." ]
    Landmark.styleOf landmark '★' |> should equal (Some "gold1")
    Landmark.styleOf landmark '─' |> should equal (Some "grey30")

[<Test>]
let ``Landmark.parse disables traffic only when the road is replaced`` () =
    (parseLandmarkOk "[road]\n~~").Traffic |> should equal false
    (parseLandmarkOk "[sidewalk]\n★").Traffic |> should equal true
    (parseLandmarkOk "[road]\n~~\n[options]\ntraffic = yes").Traffic |> should equal true

[<Test>]
let ``Landmark.parse fails with invalid landmarks`` () =
    isLandmarkError "[captions]\n" |> should equal true
    isLandmarkError "[sidewalk]\n★\n★" |> should equal true
    isLandmarkError "[road]\n~~\n[options]\nlayer = middle" |> should equal true
    isLandmarkError "[road]\n~~\n[options]\nspeed = fast" |> should equal true

[<Test>]
let ``tryResolveLandmarks follows the order of the tags`` () =
    let landmark caption = parseLandmarkOk $"[captions]\n{caption}"

    let landmarks =
        [ "landmarks/first", landmark "first"
          "landmarks/second", landmark "second" ]
        |> Map.ofList

    let street =
        World.Street.create "Street" StreetType.OneWay
        |> World.Street.tag "second"
        |> World.Street.tag "missing"
        |> World.Street.tag "first"

    tryResolveLandmarks landmarks street
    |> List.collect _.Captions
    |> should equal [ "second"; "first" ]

let private allStreets () =
    Duets.Data.World.World.get.Cities
    |> List.ofMapValues
    |> List.collect (fun city ->
        city.Zones
        |> List.ofMapValues
        |> List.collect (fun zone -> zone.Streets.Nodes |> List.ofMapValues))

[<Test>]
let ``landmarks are not loaded as templates`` () =
    Scenes.all.Value
    |> Map.keys
    |> Seq.filter (fun key -> key.StartsWith "landmarks/")
    |> List.ofSeq
    |> should be Empty

[<Test>]
let ``every tag of a street has a landmark`` () =
    allStreets ()
    |> List.collect (fun street -> street.SceneTags)
    |> List.distinct
    |> List.filter (fun tag -> Scenes.landmarks.Value.ContainsKey(landmarkKey tag) |> not)
    |> should be Empty

[<Test>]
let ``every landmark is used by a street`` () =
    let usedKeys =
        allStreets ()
        |> List.collect (fun street -> street.SceneTags |> List.map landmarkKey)
        |> Set.ofList

    Scenes.landmarks.Value
    |> Map.keys
    |> Seq.filter (fun key -> not (usedKeys.Contains key))
    |> List.ofSeq
    |> should be Empty
