namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Data.World
open Duets.Simulation

module World =
    /// Returns all cities available in the game world.
    let allCities =
        World.get |> Optic.get Lenses.World.cities_ |> List.ofMapValues

    /// Returns a specific city given its ID.
    let cityById cityId =
        World.get
        |> Optic.get (Lenses.World.city_ cityId)
        |> Option.get (* Not finding a city by its ID is a problem in city creation. *)

    /// Returns the current world coordinates the character is in currently.
    let currentCoordinates state = state.CurrentPosition

    /// Returns the city in which the character is in currently.
    let currentCity state =
        let cityId, _, _ = state.CurrentPosition
        cityId |> cityById

    /// Returns a place inside a given city by its ID.
    let placeInCityById cityId placeId =
        let city = cityById cityId
        let zoneId, streetId, placeId = Map.find placeId city.PlaceIndex
        let city = cityById cityId
        let zone = city.Zones |> Map.find zoneId

        let street = zone.Streets.Nodes |> Map.find streetId

        street.Places |> List.find (fun p -> p.Id = placeId)

    /// Returns a zone inside a given city by its ID.
    let zoneInCityById cityId zoneId =
        let city = cityById cityId
        city.Zones |> Map.find zoneId

    /// Returns a zone inside the current city by its ID.
    let zoneInCurrentCityById state zoneId =
        let cityId, _, _ = state.CurrentPosition
        zoneInCityById cityId zoneId

    /// Returns a place inside of the current city given its ID.
    let placeInCurrentCityById state placeId =
        let cityId, _, _ = state.CurrentPosition
        placeInCityById cityId placeId

    /// Retrieves the room for the given city, place and room ID.
    let roomById cityId placeId roomId =
        let place = placeInCityById cityId placeId
        Map.find roomId place.Rooms.Nodes

    /// Retrieves a street from the given city.
    let streetById cityId streetId =
        let city = cityById cityId
        Map.find streetId city.StreetIndex

    /// Retrieves a street from the current city.
    let streetInCurrentCity streetId state =
        let city = currentCity state
        streetById city.Id streetId

    /// Returns all the places in the current city, organized by their place type.
    let allPlacesInCurrentCity state =
        let cityId, _, _ = state.CurrentPosition
        let city = cityById cityId

        city.PlaceByTypeIndex
        |> Map.map (fun _ placeIds ->
            placeIds |> List.map (placeInCityById cityId))

    /// Returns the place in which the character is in currently.
    let currentPlace state =
        let _, placeId, _ = state.CurrentPosition
        placeInCurrentCityById state placeId

    /// Returns the current room in which the character is in currently.
    let currentRoom state =
        let cityId, placeId, roomId = state.CurrentPosition
        roomById cityId placeId roomId

    /// Returns a list of IDs of the places with the given type inside of the
    /// given city.
    let placeIdsByTypeInCity cityId placeType =
        cityById cityId
        |> Optic.get (
            Lenses.World.City.placeByTypeIndex_ >-> Map.key_ placeType
        )
        |> Option.defaultValue []

    /// Returns a list of places with the given type inside of a city.
    let placesByTypeInCity cityId placeType =
        placeIdsByTypeInCity cityId placeType
        |> List.map (placeInCityById cityId)

    /// Returns the distance between the given cities.
    let distanceBetween = World.distanceBetween

    /// Returns whether the given place is currently open or not.
    let placeCurrentlyOpen place currentTime =
        let currentDayMoment = Calendar.Query.dayMomentOf currentTime

        match place.OpeningHours with
        | PlaceOpeningHours.AlwaysOpen -> true
        | PlaceOpeningHours.OpeningHours(daysOfWeekOpen, dayMomentsOpen) ->
            (daysOfWeekOpen |> List.contains currentTime.DayOfWeek
             && dayMomentsOpen |> List.contains currentDayMoment)

    /// Like `placeCurrentlyOpen` but implicitly passing the current time.
    let placeCurrentlyOpen' state place =
        Queries.Calendar.today state |> placeCurrentlyOpen place

    /// Returns a list of directions that are available from the given node.
    let availableDirections id (graph: Graph<'a>) =
        Optic.get (Lenses.World.Graph.nodeConnections_ id) graph
        |> Option.defaultValue Map.empty
        |> List.ofSeq
        |> List.map (fun keyValue -> (keyValue.Key, keyValue.Value))

    /// Returns all the NPCs that are currently in the same coordinates as the
    /// character.
    let peopleInCurrentPlace state =
        let room = currentRoom state

        (* Prevent private rooms from being populated. *)
        match room.RoomType with
        | RoomType.Bedroom -> [], []
        | _ ->
            state.PeopleInCurrentPosition
            |> List.partition (fun person ->
                Relationship.withCharacter person.Id state |> Option.isSome)

    /// Returns all connected places to the current street that match the given
    /// query. If the place is not a street, nothing is returned.
    let matchingPlacesInCurrentStreet (query: string) state =
        let place = currentPlace state

        match place.PlaceType with
        | Street ->
            let street = streetInCurrentCity place.Id state

            street.Places
            |> List.filter (fun connectedPlace ->
                String.diacriticInsensitiveContains connectedPlace.Name query)
        | _ -> []
