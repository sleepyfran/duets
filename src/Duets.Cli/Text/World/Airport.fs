module Duets.Cli.Text.World.Airport

open Duets.Agents
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

let rec description place (roomType: RoomType) =
    match roomType with
    | RoomType.BoardingGate -> boardingGateDescription ()
    | RoomType.Cafe -> Cafe.description place roomType
    | RoomType.Lobby -> lobbyDescription ()
    | RoomType.Restaurant _ -> Restaurant.description place roomType
    | RoomType.SecurityControl -> securityControlDescription ()
    | _ -> failwith "Room type not supported in airport"

and private boardingGateDescription () =
    [ "The boarding gate is buzzing with activity as passengers prepare to board their flights."
      "There are rows of seats filled with travelers, some engrossed in their books or devices, while others are engaged in lively conversations."
      "Announcements over the intercom call out flight information, and airline staff are assisting passengers with the boarding process." ]
    |> List.sample

and private lobbyDescription () =
    let city = State.get () |> Queries.World.currentCity

    $"The entrance of {city.Id |> Generic.cityName}'s airport is humming with life. There's a lot of people checking in, waiting in bars and restaurants, or just sitting on the floor, waiting for their flight."

and private securityControlDescription () =
    [ "The security control area is crowded with people waiting to be checked. The line is moving slowly."
      "There are several security agents, some of them are checking people, others are just standing around, chatting." ]
    |> List.sample
