[<RequireQualifiedAccess>]
module Duets.Cli.Components.Route

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

type private RouteAgentMsg =
    | SetDestination of Place option
    | GetDestination of AsyncReplyChannel<Place option>

type private RouteAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop destination =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | SetDestination newDestination ->
                        return! loop newDestination
                    | GetDestination channel ->
                        channel.Reply destination
                        return! loop destination
                }

            loop None

    member public this.Set destination =
        SetDestination destination |> agent.Post

    member public this.Get() = agent.PostAndReply GetDestination

let private routeAgent = RouteAgent()

/// Starts guiding the player towards the given place.
let follow (destination: Place) = Some destination |> routeAgent.Set

/// Stops guiding the player.
let stop () = routeAgent.Set None

/// Returns the destination being followed and the directions left to reach it
/// from the current place, if any.
let current state =
    routeAgent.Get()
    |> Option.bind (fun destination ->
        let cityId, currentPlaceId, _ = Queries.World.currentCoordinates state

        Pathfinding.directionsToNode cityId currentPlaceId destination.Id
        |> Option.map (Tuple.two destination))
