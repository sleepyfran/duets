namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Common
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module WorldCommands =
    /// Command which teleports the character to a given city.
    let teleport =
        { Name = "teleport"
          Description = "Teleports you to another city instantly"
          Handler =
            (fun _ ->
                let destination =
                    Queries.World.allCities
                    |> showOptionalChoicePrompt
                        "Where to? :^)"
                        Generic.cancel
                        (fun city -> Generic.cityName city.Id)

                match destination with
                | Some city ->
                    let destinationAirport =
                        Queries.World.placeIdsByTypeInCity
                            city.Id
                            PlaceTypeIndex.Airport
                        |> List.head (* All cities must have an airport. *)

                    Navigation.travelTo
                        city.Id
                        destinationAirport
                        (State.get ())
                    |> Effect.apply

                    Scene.WorldAfterMovement
                | None -> Scene.Cheats) }

    /// Command which teleports the character to any place inside the current
    /// city, picking first the zone and then the place.
    let beamMeUp =
        { Name = "beam me up"
          Description =
            "Teleports you to any place in the current city by zone, ignoring opening hours and rentals"
          Handler =
            (fun _ ->
                let state = State.get ()
                let city = Queries.World.currentCity state

                let selectedZone =
                    city.Zones
                    |> List.ofMapValues
                    |> List.sortBy _.Name
                    |> showOptionalChoicePrompt
                        "Which zone? :^)"
                        Generic.cancel
                        _.Name

                let selectedPlace =
                    selectedZone
                    |> Option.bind (fun zone ->
                        zone.Streets.Nodes
                        |> List.ofMapValues
                        |> List.collect _.Places
                        |> List.sortBy _.Name
                        |> showOptionalChoicePrompt
                            $"Where in {zone.Name}? :^)"
                            Generic.cancel
                            (fun place ->
                                $"""{place.Name} {$"({Text.World.World.placeTypeName' place})" |> Styles.faded}"""))

                match selectedPlace with
                | Some place ->
                    let currentCoords = Queries.World.currentCoordinates state

                    WorldMoveToPlace(
                        Diff(
                            currentCoords,
                            (city.Id, place.Id, place.Rooms.StartingNode)
                        )
                    )
                    |> Effect.apply

                    Scene.WorldAfterMovement
                | None -> Scene.Cheats) }

    /// Command which teleports the character to any concert venue in the world,
    /// ignoring opening hours.
    let backstagePass =
        { Name = "backstage pass"
          Description =
            "Teleports you to any concert venue in the world, ignoring opening hours"
          Handler =
            (fun _ ->
                let state = State.get ()

                let capacityOf place =
                    match place.PlaceType with
                    | PlaceType.ConcertSpace concertSpace ->
                        concertSpace.Capacity
                    | _ -> 0

                let venuesByCountry =
                    Queries.World.allCities
                    |> List.collect (fun city ->
                        Queries.World.placesByTypeInCity
                            city.Id
                            PlaceTypeIndex.ConcertSpace
                        |> List.map (fun place -> city.Id, place))
                    |> List.groupBy (fst >> Queries.World.countryOf)
                    |> List.sortBy (fst >> Generic.countryName)
                    |> List.map (fun (country, venues) ->
                        Generic.countryName country,
                        venues |> List.sortByDescending (snd >> capacityOf))

                let venueText (cityId, place) =
                    let details =
                        $"({Generic.cityName cityId}, capacity: {capacityOf place})"

                    $"{place.Name} {Styles.faded details}"

                let selectedVenue =
                    showGroupedOptionalChoicePrompt
                        "Which venue? :^)"
                        Generic.cancel
                        ""
                        venueText
                        venuesByCountry
                        []

                match selectedVenue with
                | Some(cityId, place) ->
                    let currentCoords = Queries.World.currentCoordinates state

                    WorldMoveToPlace(
                        Diff(
                            currentCoords,
                            (cityId, place.Id, place.Rooms.StartingNode)
                        )
                    )
                    |> Effect.apply

                    Scene.WorldAfterMovement
                | None -> Scene.Cheats) }

    /// Command which changes the weather of the current city.
    let weatherman =
        { Name = "weatherman"
          Description = "Changes the weather in the current city"
          Handler =
            (fun _ ->
                let state = State.get ()
                let cityId, _, _ = Queries.World.currentCoordinates state
                let currentWeather = Queries.World.currentWeather state

                let selectedWeather =
                    Union.allCasesOf<WeatherCondition> ()
                    |> showOptionalChoicePrompt
                        "What weather do you want? :^)"
                        Generic.cancel
                        (fun weather ->
                            let name = $"{Emoji.weather weather} %A{weather}"

                            if weather = currentWeather then
                                $"{name} (Current)" |> Styles.highlight
                            else
                                name)

                match selectedWeather with
                | Some weather when weather <> currentWeather ->
                    WeatherChanged(cityId, Diff(currentWeather, weather))
                    |> Effect.apply
                | _ -> ()

                Scene.Cheats) }
