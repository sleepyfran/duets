[<AutoOpen>]
module rec Duets.Cli.Components.Map

open Duets.Agents
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

type private PlaceSpecialProperty =
    | Rented
    | ConcertScheduled

let private placesWithSpecialProperties state =
    let band = Queries.Bands.currentBand state
    let currentDate = Queries.Calendar.today state

    let scheduledConcert =
        Queries.Concerts.scheduleForDay state band.Id currentDate
        |> Option.map (fun scheduledConcert ->
            let concert = Concert.fromScheduled scheduledConcert

            [ concert.VenueId, PlaceSpecialProperty.ConcertScheduled ]
            |> Map.ofList)
        |> Option.defaultValue Map.empty

    let rentedPlaces =
        Queries.Rentals.all state
        |> List.collect (fun rental ->
            let _, placeId = rental.Coords
            [ placeId, PlaceSpecialProperty.Rented ])
        |> Map.ofList

    Map.merge rentedPlaces scheduledConcert


let private placeWithOpenInfo (city: City) (place: Place) specialProperties =
    let state = State.get ()

    let currentlyOpen = Queries.World.placeCurrentlyOpen' state place

    let placeSpecialProperties = specialProperties |> Map.tryFind place.Id

    let placeDetails =
        match placeSpecialProperties with
        | Some Rented -> $"""{place.Name} ({"Rented" |> Styles.highlight})"""
        | Some ConcertScheduled ->
            $"""{place.Name} ({"Concert scheduled" |> Styles.highlight})"""
        | None -> place.Name

    World.placeNameWithOpeningInfo placeDetails currentlyOpen

let private showPlaceChoice city placesInCity places =
    let specialProperties = placesWithSpecialProperties (State.get ())

    let selectedPlace =
        showCancellableChoicePrompt
            Command.mapChoosePlace
            Generic.back
            (fun place -> placeWithOpenInfo city place specialProperties)
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
    let navigationResult = Navigation.moveTo destination.Id (State.get ())

    $"You walk to {destination.Name}..." |> showMessage

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

let private sortConcertSpaces state =
    let band = Queries.Bands.currentBand state
    let currentDate = Queries.Calendar.today state

    let scheduledConcert =
        Queries.Concerts.scheduleForDay state band.Id currentDate

    match scheduledConcert with
    | Some scheduledConcert ->
        let concert = Concert.fromScheduled scheduledConcert

        changePlace
            PlaceTypeIndex.ConcertSpace
            (List.sortByDescending (fun (place: Place) ->
                place.Id = concert.VenueId))
    | None -> id

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
        |> sortConcertSpaces state

    allAvailablePlaces |> askForPlace currentCity

/// Shows the map, forcing the user to make a choice.
let showMapUntilChoice () =
    match showMap () with
    | effects when effects.Length > 0 -> effects
    | _ -> showMapUntilChoice ()
