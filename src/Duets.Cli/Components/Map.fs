[<AutoOpen>]
module rec Duets.Cli.Components.Map

open Duets.Agents
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

let private placeWithOpenInfo place =
    let currentlyOpen = Queries.World.placeCurrentlyOpen' (State.get ()) place

    match currentlyOpen with
    | true -> World.placeWithZone place
    | false -> Styles.faded $"{place.Name} ({place.Zone.Name}) - Closed"

let private showOpeningHours place =
    match place.OpeningHours with
    | PlaceOpeningHours.OpeningHours(daysOfWeek, dayMoments) ->
        let openingDays =
            match daysOfWeek with
            | days when days = Calendar.everyDay -> "Everyday"
            | days when days = Calendar.weekday -> "Monday to Friday"
            | _ -> Generic.listOf daysOfWeek Generic.dayName

        let openingHours = Generic.listOf dayMoments Generic.dayMomentName

        $"{Styles.place place.Name} opens {Styles.time openingDays} @ {openingHours}"
        |> showMessage
    | _ ->
        (* Obviously if it's always open this shouldn't happen :) *)
        ()

let private showPlaceChoice placesInCity places =
    let selectedPlace =
        showOptionalChoicePrompt
            Command.mapChoosePlace
            Generic.back
            placeWithOpenInfo
            places

    match selectedPlace with
    | Some place -> Some place
    | None -> showPlaceTypeChoice placesInCity

let private showPlaceTypeChoice
    (placesInCity: Map<PlaceTypeIndex, Place list>)
    =
    let availablePlaceTypes =
        placesInCity |> List.ofSeq |> List.map (fun kvp -> kvp.Key)

    showOptionalChoicePrompt
        Command.mapChoosePlaceTypePrompt
        Generic.cancel
        World.placeTypeName
        availablePlaceTypes
    |> Option.bind (fun placeType ->
        placesInCity |> Map.find placeType |> showPlaceChoice placesInCity)

let private moveToPlace availablePlaces (destination: Place) =
    let currentPlace = State.get () |> Queries.World.currentPlace
    let navigationResult = Navigation.moveTo destination.Id (State.get ())

    if currentPlace.Zone.Id <> destination.Zone.Id then
        $"You take the public transport to get to {destination.Name}..."
    else
        $"You walk to {destination.Name}..."
    |> showMessage

    wait 2000<millisecond>

    match navigationResult with
    | Ok effect -> [ effect ]
    | Error PlaceEntranceError.CannotEnterOutsideOpeningHours ->
        showSeparator None

        Styles.error
            $"{destination.Name} is currently closed. Try again during their opening hours"
        |> showMessage

        showOpeningHours destination

        showSeparator None

        askForPlace availablePlaces
    | Error PlaceEntranceError.CannotEnterWithoutRental ->
        showSeparator None

        Styles.error "You cannot enter this place without renting it first"
        |> showMessage

        Styles.information
            "Try to use your phone to rent it out and come back again afterwards"
        |> showMessage

        showSeparator None

        askForPlace availablePlaces

let private askForPlace availablePlaces =
    let selectedPlace = availablePlaces |> showPlaceTypeChoice

    match selectedPlace with
    | Some place -> moveToPlace availablePlaces place
    | None -> []

/// Shows a list of all the place types in the current city and, upon selecting
/// one type, shows all the places of that specific type in the current city.
/// If the user selects one of the places, it will attempt to move the character
/// to that location and return the effects associated with it, respecting the
/// place opening hours and any other policies it might have.
let showMap () =
    Queries.World.allPlacesInCurrentCity (State.get ()) |> askForPlace

/// Shows the map, forcing the user to make a choice.
let showMapUntilChoice () =
    match showMap () with
    | effects when effects.Length > 0 -> effects
    | _ -> showMapUntilChoice ()
