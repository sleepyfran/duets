[<RequireQualifiedAccess>]
module Duets.Cli.Components.Route

open Duets.Entities
open Duets.Simulation.Navigation

type private ActiveRoute =
    { CityId: CityId
      Destination: Place
      Directions: Pathfinding.PathAction list }

type private RouteAgentMsg =
    | SetRoute of ActiveRoute option
    | GetRoute of AsyncReplyChannel<ActiveRoute option>

type private RouteAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop route =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | SetRoute newRoute -> return! loop newRoute
                    | GetRoute channel ->
                        channel.Reply route
                        return! loop route
                }

            loop None

    member public this.Set route = SetRoute route |> agent.Post

    member public this.Get() = agent.PostAndReply GetRoute

let private routeAgent = RouteAgent()

/// Starts guiding the player towards the given place with the given directions.
let follow cityId (destination: Place) directions =
    Some
        { CityId = cityId
          Destination = destination
          Directions = directions }
    |> routeAgent.Set

/// Stops guiding the player.
let stop () = routeAgent.Set None

/// Returns the destination being followed and the directions left to reach it.
let current () =
    routeAgent.Get()
    |> Option.map (fun route -> route.Destination, route.Directions)

/// Advances the route after the player moves and returns the destination if
/// the player arrived to it.
let advance (before: RoomCoordinates) (after: RoomCoordinates) =
    let _, previousPlaceId, _ = before
    let cityId, placeId, _ = after

    match routeAgent.Get() with
    | Some route when route.CityId <> cityId ->
        stop ()
        None
    | Some route when previousPlaceId <> placeId ->
        let directionsLeft =
            Pathfinding.advanceDirections
                cityId
                placeId
                route.Destination.Id
                route.Directions

        match directionsLeft with
        | Some [] ->
            stop ()
            Some route.Destination
        | Some directions ->
            routeAgent.Set(Some { route with Directions = directions })
            None
        | None ->
            (* The destination can no longer be reached. *)
            stop ()
            None
    | _ -> None
