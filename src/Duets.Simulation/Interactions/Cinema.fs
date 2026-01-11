module Duets.Simulation.Interactions.Cinema

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

/// Attempts to purchase a ticket and watch the currently showing movie.
let watchMovie state =
    let currentDate = Queries.Calendar.today state
    let (cityId, placeId, _) = Queries.World.currentCoordinates state
    let city = Queries.World.cityById cityId
    let characterAccount = Queries.Bank.playableCharacterAccount state

    match Queries.Cinema.currentMovie currentDate with
    | None -> Error "No movie currently showing"
    | Some movie ->
        let price = Queries.Cinema.ticketPrice city

        match expense state characterAccount price with
        | Error _ -> Error "Not enough funds"
        | Ok paymentEffects ->
            let moodIncrease =
                match movie.Quality with
                | q when q <= 3 -> Config.LifeSimulation.Mood.watchingPoorMovieIncrease
                | q when q <= 6 -> Config.LifeSimulation.Mood.watchingDecentMovieIncrease
                | q when q <= 8 -> Config.LifeSimulation.Mood.watchingGoodMovieIncrease
                | _ -> Config.LifeSimulation.Mood.watchingExcellentMovieIncrease

            let character = Queries.Characters.playableCharacter state

            [ yield! paymentEffects
              yield MovieTicketPurchased(cityId, placeId, movie.Title, price)
              yield MovieWatched(movie.Title, movie.Quality)
              yield! Character.Attribute.add
                         character
                         CharacterAttribute.Mood
                         moodIncrease
              yield Wait Config.Cinema.movieWatchingTime ]
            |> Ok
