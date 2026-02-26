module Duets.Simulation.Cinema.Ticket

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

/// Pays for a cinema ticket for the given movie, which deducts the amount from
/// the player's account and adds a movie ticket to their inventory allowing
/// them to enter the screening room.
let pay state (movie: Movie) amount =
    let cityId, placeId, _ = Queries.World.currentCoordinates state

    let characterAccount = Queries.Bank.playableCharacterAccount state

    expense state characterAccount amount
    |> Result.map (fun effects ->
        let ticket = Item.Key.createCinemaTicketFor cityId placeId

        effects
        @ [ MovieTicketPurchased(cityId, placeId, movie.Title, amount)
            ItemAddedToCharacterInventory ticket ])
