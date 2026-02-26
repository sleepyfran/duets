module Duets.Simulation.Interactions.Cinema

open Duets.Data.Items
open Duets.Entities
open Duets.Simulation

/// Maximum number of cinema snacks/drinks consumed while watching a movie.
let [<Literal>] private maxItemsConsumed = 3

/// Returns effects for consuming cinema-bought snacks/drinks from inventory.
let private consumeCinemaItems state =
    Queries.Inventory.character state
    |> List.filter (fun item -> item.Brand = Cinema.brand)
    |> List.truncate maxItemsConsumed
    |> List.collect (fun item ->
        item.Properties
        |> List.tryPick (function
            | Edible food -> Some(Food.eat state item food)
            | Drinkable drink -> Some(Drink.drink state item drink)
            | _ -> None)
        |> Option.defaultValue [])

/// Watches the currently showing movie. Assumes the player already has a ticket.
/// Automatically consumes any cinema snacks or drinks from the inventory.
let watchMovie state (movie: Movie) =
    let moodIncrease =
        match movie.Quality with
        | q when q <= 3 -> Config.LifeSimulation.Mood.watchingPoorMovieIncrease
        | q when q <= 6 -> Config.LifeSimulation.Mood.watchingDecentMovieIncrease
        | q when q <= 8 -> Config.LifeSimulation.Mood.watchingGoodMovieIncrease
        | _ -> Config.LifeSimulation.Mood.watchingExcellentMovieIncrease

    let character = Queries.Characters.playableCharacter state

    [ yield! consumeCinemaItems state
      yield MovieWatched(movie.Title, movie.Quality)
      yield! Character.Attribute.add character CharacterAttribute.Mood moodIncrease
      yield Wait Config.Cinema.movieWatchingTime ]
