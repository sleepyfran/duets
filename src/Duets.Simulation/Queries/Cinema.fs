namespace Duets.Simulation.Queries

open Duets.Data
open Duets.Entities
open Duets.Simulation

module Cinema =
    /// Returns the movie currently showing based on the given date.
    /// Uses deterministic selection: same season/year always returns same movie.
    let currentMovie (date: Date) =
        let allMovies = Movies.all

        if List.isEmpty allMovies then
            None
        else
            let seasonNumber =
                match date.Season with
                | Spring -> 1
                | Summer -> 2
                | Autumn -> 3
                | Winter -> 4

            let seed = int date.Year * 4 + seasonNumber
            let index = seed % (List.length allMovies)
            List.item index allMovies |> Some

    /// Calculates the ticket price for a cinema based on city cost of living.
    let ticketPrice (city: City) =
        let multiplier = decimal city.CostOfLiving
        Config.Cinema.baseTicketPrice * multiplier
