namespace Simulation.Queries.Internal.Interactions


open Entities

module Home =
    let internal availableCurrently room =
        match room with
        | Room.Bedroom -> [ Interaction.Home HomeInteraction.Sleep ]
        | Room.Kitchen -> [ Interaction.Home HomeInteraction.Eat ]
        | Room.LivingRoom ->
            [ Interaction.Home HomeInteraction.PlayXbox
              Interaction.Home HomeInteraction.WatchTv ]
        | _ -> []
