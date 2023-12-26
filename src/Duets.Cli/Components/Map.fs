[<AutoOpen>]
module rec Duets.Cli.Components.Map

open Duets.Agents
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

let private placeWithOpenInfo (city: City) (place: Place) =
    let rentedPlaces = Queries.Rentals.allAsMap (State.get ())
    let hasPlaceRented = rentedPlaces |> Map.containsKey (city.Id, place.Id)
    let currentlyOpen = Queries.World.placeCurrentlyOpen' (State.get ()) place

    let placeDetails =
        match hasPlaceRented with
        | true ->
            $"""{World.placeWithZone place} ({"Rented" |> Styles.highlight})"""
        | false -> World.placeWithZone place

    World.placeNameWithOpeningInfo placeDetails currentlyOpen

let private showPlaceChoice city placesInCity places =
    let selectedPlace =
        showCancellableChoicePrompt
            Command.mapChoosePlace
            Generic.back
            (placeWithOpenInfo city)
            places

    match selectedPlace with
    | Some place -> Some place
    | None -> showPlaceTypeChoice city placesInCity

let private showPlaceTypeChoice
    city
    (placesInCity: Map<PlaceTypeIndex, Place list>)
    =
    let availablePlaceTypes = placesInCity |> List.ofSeq |> List.map (_.Key)

    showCancellableChoicePrompt
        Command.mapChoosePlaceTypePrompt
        Generic.cancel
        World.placeTypeName
        availablePlaceTypes
    |> Option.bind (fun placeType ->
        placesInCity |> Map.find placeType |> showPlaceChoice city placesInCity)

let private moveToPlace city availablePlaces (destination: Place) =
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

        World.placeClosedError destination |> showMessage
        World.placeOpeningHours destination |> showMessage

        showSeparator None

        askForPlace city availablePlaces
    | Error PlaceEntranceError.CannotEnterWithoutRental ->
        showSeparator None

        Styles.error "You cannot enter this place without renting it first"
        |> showMessage

        Styles.information
            "Try to use your phone to rent it out and come back again afterwards"
        |> showMessage

        showSeparator None

        askForPlace city availablePlaces

let private askForPlace city availablePlaces =
    let selectedPlace = availablePlaces |> showPlaceTypeChoice city

    match selectedPlace with
    | Some place -> moveToPlace city availablePlaces place
    | None -> []

let private changePlace idxType fn (places: Map<PlaceTypeIndex, Place list>) =
    Map.change idxType (Option.bind (fn >> Option.ofList)) places

let private filterRentedHomePlaces (currentCity: City) rentedPlaces =
    changePlace
        PlaceTypeIndex.Home
        (List.filter (fun (place: Place) ->
            rentedPlaces |> Map.containsKey (currentCity.Id, place.Id)))

let private sortHotels (currentCity: City) rentedPlaces =
    changePlace
        PlaceTypeIndex.Hotel
        (List.sortByDescending (fun (place: Place) ->
            rentedPlaces |> Map.containsKey (currentCity.Id, place.Id)))

/// Shows a list of all the place types in the current city and, upon selecting
/// one type, shows all the places of that specific type in the current city.
/// If the user selects one of the places, it will attempt to move the character
/// to that location and return the effects associated with it, respecting the
/// place opening hours and any other policies it might have.
let showMap () =
    let state = State.get ()
    let rentedPlaces = Queries.Rentals.allAsMap state
    let currentCity = Queries.World.currentCity state

    (* Filter out homes that are not rented to not pollute the map. *)
    let allAvailablePlaces =
        Queries.World.allPlacesInCurrentCity state
        |> filterRentedHomePlaces currentCity rentedPlaces
        |> sortHotels currentCity rentedPlaces

    allAvailablePlaces |> askForPlace currentCity

/// Shows the map, forcing the user to make a choice.
let showMapUntilChoice () =
    match showMap () with
    | effects when effects.Length > 0 -> effects
    | _ -> showMapUntilChoice ()
