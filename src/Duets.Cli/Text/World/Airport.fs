module Duets.Cli.Text.World.Airport

open Duets.Agents
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

let rec description _ (roomType: RoomType) =
    match roomType with
    | RoomType.Lobby -> lobbyDescription ()
    | RoomType.SecurityControl -> securityControlDescription ()
    | _ -> failwith "Room type not supported in airport"

and private lobbyDescription () =
    let city = State.get () |> Queries.World.currentCity

    $"The entrance of {city.Id |> Generic.cityName}'s airport is humming with life. There's a lot of people checking in, waiting in bars and restaurants, or just sitting on the floor, waiting for their flight."

and private securityControlDescription () =
    [ "The security control area is crowded with people waiting to be checked. The line is moving slowly."
      "There are several security agents, some of them are checking people, others are just standing around, chatting." ]
    |> List.sample
