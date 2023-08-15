namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Simulation

module Gym =
    /// Gather all available interactions inside a gym.
    let internal interactions cityId place roomType =
        match roomType with
        | RoomType.Lobby ->
            let entranceFee =
                (cityId, place) ||> Queries.Gym.calculateEntranceCost

            [ Interaction.Gym(GymInteraction.PayEntrance entranceFee) ]
        | _ -> []
