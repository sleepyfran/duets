namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Simulation

module Cinema =
    /// Gather all available interactions inside a cinema.
    let internal interactions state roomType =
        let currentDate = Queries.Calendar.today state

        match roomType with
        | RoomType.ScreeningRoom ->
            match Queries.Cinema.currentMovie currentDate with
            | Some movie ->
                [ Interaction.Cinema(CinemaInteraction.WatchMovie movie) ]
            | None -> []
        | _ -> []
