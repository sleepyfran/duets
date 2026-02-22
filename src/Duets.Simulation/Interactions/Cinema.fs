module Duets.Simulation.Interactions.Cinema

open Duets.Entities
open Duets.Simulation

/// Watches the currently showing movie. Assumes the player already has a ticket.
let watchMovie state (movie: Movie) =
    let moodIncrease =
        match movie.Quality with
        | q when q <= 3 -> Config.LifeSimulation.Mood.watchingPoorMovieIncrease
        | q when q <= 6 -> Config.LifeSimulation.Mood.watchingDecentMovieIncrease
        | q when q <= 8 -> Config.LifeSimulation.Mood.watchingGoodMovieIncrease
        | _ -> Config.LifeSimulation.Mood.watchingExcellentMovieIncrease

    let character = Queries.Characters.playableCharacter state

    [ yield MovieWatched(movie.Title, movie.Quality)
      yield! Character.Attribute.add character CharacterAttribute.Mood moodIncrease
      yield Wait Config.Cinema.movieWatchingTime ]
