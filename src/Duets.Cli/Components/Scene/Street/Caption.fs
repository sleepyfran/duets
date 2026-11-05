[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Caption

open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities
open Duets.Entities.World.Place.Type

let private pick (random: System.Random) (options: string list) =
    options |> List.item (random.Next(options.Length))

let private placeSentence (random: System.Random) phase (places: (Place * bool) list) =
    let about placeTypes sentences =
        places
        |> List.filter (fun (place, isOpen) ->
            isOpen && List.contains (toIndex place.PlaceType) placeTypes)
        |> function
            | [] -> None
            | candidates ->
                let place, _ = candidates |> List.item (random.Next(candidates.Length))
                Styles.place place.Name |> sentences |> pick random |> Some

    let coffee () = about [ PlaceTypeIndex.Cafe ] SceneCaptions.coffee
    let food () = about [ PlaceTypeIndex.Restaurant ] SceneCaptions.food

    let nightlife () =
        about
            [ PlaceTypeIndex.ConcertSpace
              PlaceTypeIndex.Bar
              PlaceTypeIndex.Casino ]
            SceneCaptions.nightlife

    match places with
    | [] -> None
    | places when places |> List.forall (snd >> not) ->
        pick random SceneCaptions.allClosed |> Some
    | _ ->
        match phase with
        | SceneCaptions.Dawn -> coffee ()
        | SceneCaptions.Day -> coffee () |> Option.orElseWith food
        | SceneCaptions.Dusk -> food () |> Option.orElseWith nightlife
        | SceneCaptions.Dark -> nightlife ()

/// Creates one line of flavor text for the street. The caption only depends
/// on the street, the time of the day, the weather, how many people are
/// around and which places are open, so it stays the same on every visit
/// with the same conditions.
let create
    (street: Street)
    part
    (lighting: Lighting)
    peopleCount
    (places: (Place * bool) list)
    =
    let random =
        seedOf
            $"{street.Id}{part}{lighting.DayMoment}{lighting.Weather}{peopleCount}"
        |> System.Random

    let phase = SceneCaptions.phaseOf lighting.DayMoment

    let opening =
        SceneCaptions.atmosphere lighting.Weather phase |> pick random

    let closing = SceneCaptions.activity peopleCount |> pick random

    match placeSentence random phase places with
    | Some sentence -> $"{opening}{closing} {sentence}"
    | None -> $"{opening}{closing}"
