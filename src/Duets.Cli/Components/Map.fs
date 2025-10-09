[<AutoOpen>]
module rec Duets.Cli.Components.Map

open Duets.Agents
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation

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


let private placeWithInfo (city: City) (place: Place) specialProperties =
    let state = State.get ()

    let currentlyOpen = Queries.World.placeCurrentlyOpen' state place

    let placeSpecialProperties = specialProperties |> Map.tryFind place.Id

    let zone = Queries.World.zoneInCityById city.Id place.ZoneId
    let zonePrefix = $"{zone.Name |> Styles.place},"

    let placeDetails =
        match placeSpecialProperties with
        | Some Rented ->
            $"""{zonePrefix} {place.Name} ({"Rented" |> Styles.highlight})"""
        | Some ConcertScheduled ->
            $"""{zonePrefix} {place.Name} ({"Concert scheduled" |> Styles.highlight})"""
        | None -> $"{zonePrefix} {place.Name}"

    World.placeNameWithOpeningInfo placeDetails currentlyOpen

let private showPlaceChoice city placesInCity places =
    let specialProperties = placesWithSpecialProperties (State.get ())

    let selectedPlace =
        showCancellableChoicePrompt
            Command.mapChoosePlace
            Generic.back
            (fun place -> placeWithInfo city place specialProperties)
            places

    match selectedPlace with
    | Some place -> Some place
    | None -> showPlaceTypeChoice city placesInCity

let private showPlaceTypeChoice
    city
    (placesInCity: Map<PlaceTypeIndex, Place list>)
    =
    let availablePlaceTypes = placesInCity |> List.ofSeq |> List.map _.Key

    showCancellableChoicePrompt
        Command.mapChoosePlaceTypePrompt
        Generic.cancel
        World.placeTypeName
        availablePlaceTypes
    |> Option.bind (fun placeType ->
        placesInCity |> Map.find placeType |> showPlaceChoice city placesInCity)

let private changePlace idxType fn (places: Map<PlaceTypeIndex, Place list>) =
    Map.change idxType (Option.bind (fn >> Option.ofList)) places

let private filterRentedHomePlaces (currentCity: City) rentedPlaces =
    changePlace
        PlaceTypeIndex.Home
        (List.filter (fun (place: Place) ->
            rentedPlaces |> Map.containsKey (currentCity.Id, place.Id)))

let private filterStreets =
    changePlace PlaceTypeIndex.Street (List.filter (fun _ -> false))

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
/// Returns the selected place if one was chosen, or None if the user cancelled.
let showMap () =
    let state = State.get ()
    let rentedPlaces = Queries.Rentals.allAsMap state
    let currentCity = Queries.World.currentCity state

    (* Filter out homes that are not rented to not pollute the map. *)
    let allAvailablePlaces =
        Queries.World.allPlacesInCurrentCity state
        |> filterRentedHomePlaces currentCity rentedPlaces
        |> filterStreets
        |> sortHotels currentCity rentedPlaces
        |> sortConcertSpaces state

    allAvailablePlaces |> showPlaceTypeChoice currentCity
