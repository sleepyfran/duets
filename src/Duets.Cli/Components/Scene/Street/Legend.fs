[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Legend

open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Entities.World.Place.Type
open Duets.Simulation
open Spectre.Console

let private highlightText highlight =
    let glyph, style = Highlights.markerOf highlight

    let text =
        match highlight with
        | Highlights.Rented -> World.streetPlaceRented
        | Highlights.ConcertToday _ -> World.streetPlaceConcertToday
        | Highlights.ConcertScheduled concert ->
            World.streetPlaceConcertScheduled concert.Date
        | Highlights.Workplace -> World.streetPlaceWorkplace

    $"[{style}]{glyph} {text}[/]"

/// Lists the places of the street with their type, whether they're open and
/// why they're relevant to the player, if they are.
let showPlaces (places: (Place * bool * Highlights.Highlight list) list) =
    places
    |> List.iter (fun (place, isOpen, highlights) ->
        let icon = place.PlaceType |> toIndex |> SceneStyles.placeIcon

        let highlightsText =
            match highlights with
            | [] -> ""
            | highlights ->
                highlights
                |> List.map highlightText
                |> String.concat (Styles.faded ", ")
                |> fun text -> $""" {Styles.faded "·"} {text}"""

        World.placeNameWithOpeningInfo (World.placeNameWithType place) isOpen
        |> fun name ->
            let icon = icon |> string |> Markup.Escape |> Styles.faded
            $"  {icon} {name}{highlightsText}"
        |> showMessage)

/// Lists where the player can go from this part of the street.
let showDirections
    state
    (place: Place)
    (street: Street)
    (interactions: InteractionWithMetadata list)
    =
    let continuations =
        interactions
        |> List.choose (fun interaction ->
            match interaction.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Move(direction,
                                                            roomId)) ->
                Some(direction, roomId)
            | _ -> None)
        |> List.map (fun (direction, roomId) ->
            let part =
                match street.Type with
                | StreetType.Split(_, splits) -> $" ({int roomId + 1}/{splits})"
                | StreetType.OneWay -> ""

            let arrow = SceneStyles.directionArrow direction
            $"{arrow} {World.directionName direction}{part}"
            |> Styles.direction)

    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    let connectingStreets =
        zone.Streets.Connections
        |> Map.tryFind place.Id
        |> Option.defaultValue Map.empty
        |> Map.toList
        |> List.choose (fun (direction, streetId) ->
            zone.Streets.Nodes
            |> Map.tryFind streetId
            |> Option.map (fun connectedStreet ->
                let arrow =
                    SceneStyles.directionArrow direction |> Styles.direction

                $"{arrow} {connectedStreet.Name |> Styles.place}"))

    if not continuations.IsEmpty then
        World.streetContinues (Generic.listOf continuations id) |> showMessage

    if not connectingStreets.IsEmpty then
        World.streetConnectsTo (Generic.listOf connectingStreets id)
        |> showMessage
