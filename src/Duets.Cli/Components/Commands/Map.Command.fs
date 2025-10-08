namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module rec MapCommand =
    /// Shows directions from the current place to the destination.
    let private showDirectionsToPlace (city: City) (destination: Place) =
        let currentPlace = Queries.World.currentPlace (State.get ())

        let directionsToPlace =
            Pathfinding.directionsToNode city.Id currentPlace.Id destination.Id

        let directions = directionsToPlace |> Option.defaultValue []

        match directions with
        | [] -> ()
        | directions ->
            $"Directions to {destination.Name |> Styles.place}:"
            |> Styles.header
            |> showMessage

            showSeparator None

            directions
            |> List.indexed
            |> List.iter (fun (index, direction) ->
                let isLast = index = (List.length directions - 1)
                let prefix = if isLast then "└─" else "├─"

                match direction with
                | Pathfinding.GoOut(fromPlace, toStreet) ->
                    $"""{prefix} {Styles.action "Leave"} {fromPlace.Name |> Styles.place} and {Styles.action "walk"} to {toStreet.Name |> Styles.place}"""
                | Pathfinding.Enter(fromStreet, toPlace) ->
                    $"""{prefix} {Styles.action "Enter"} {toPlace.Name |> Styles.place} from {fromStreet.Name |> Styles.place}"""
                | Pathfinding.TakeMetro(fromStation, toStation, throughLine) ->
                    let fromStation =
                        Queries.World.placeInCityById
                            city.Id
                            fromStation.PlaceId

                    let toStation =
                        Queries.World.placeInCityById city.Id toStation.PlaceId

                    $"""{prefix} {Styles.action "Take the metro"} from {fromStation.Name |> Styles.place} to {toStation.Name |> Styles.place} through the {Styles.line throughLine} line"""
                | Pathfinding.Walk(fromStreet, toStreet, throughDirection) ->
                    $"""{prefix} {Styles.action "Walk"} from {fromStreet.Name |> Styles.place} to {toStreet.Name |> Styles.place} through the {World.World.directionName throughDirection |> Styles.direction}"""
                |> showMessage)

        let estimatedTravelTime = TravelTime.travelTime directions

        $"Estimated travel time: ~{estimatedTravelTime |> Styles.time} minutes"
        |> showMessage

    /// Creates a command that allows the player to get directions to other
    /// places inside the current city.
    let get =
        { Name = "map"
          Description = Command.mapDescription
          Handler =
            fun _ ->
                let currentCity = Queries.World.currentCity (State.get ())

                currentCity.Id |> Command.mapCurrentCity |> showMessage

                Command.mapTip |> showMessage
                lineBreak ()

                match showMap () with
                | Some place ->
                    showDirectionsToPlace currentCity place
                    lineBreak ()
                | None -> ()

                Scene.WorldAfterMovement }
