namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module rec GpsCommand =
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

                $"{prefix} {World.pathAction city.Id direction}" |> showMessage)

            let estimatedTravelTime = TravelTime.byPublicTransport directions

            $"Estimated travel time: ~{estimatedTravelTime |> Styles.time} minutes"
            |> showMessage

            Route.follow destination

            Command.gpsFollowTip |> showMessage
            showContinuationPrompt ()

    /// Creates a command that guides the player to other places inside the
    /// current city.
    let get =
        { Name = "gps"
          Description = Command.gpsDescription
          Handler =
            fun args ->
                match args with
                | [ "stop" ] ->
                    match Route.current (State.get ()) with
                    | Some _ ->
                        Route.stop ()
                        Command.gpsStopped |> showMessage
                    | None -> Command.gpsNoActiveRoute |> showMessage

                    Scene.World
                | _ ->
                    let currentCity = Queries.World.currentCity (State.get ())

                    currentCity.Id |> Command.gpsCurrentCity |> showMessage

                    Command.gpsTip |> showMessage
                    lineBreak ()

                    match showMap () with
                    | Some place ->
                        showDirectionsToPlace currentCity place
                        lineBreak ()
                    | None -> ()

                    Scene.WorldAfterMovement }
