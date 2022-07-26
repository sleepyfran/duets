namespace Simulation.Queries.Internal.Interactions


open Entities

module Home =
    let internal availableCurrently room =
        match room with
        | RoomType.Bedroom -> [ Interaction.Home HomeInteraction.Sleep ]
        | RoomType.Kitchen -> [ Interaction.Home HomeInteraction.Eat ]
        | RoomType.LivingRoom ->
            [ Interaction.Home HomeInteraction.PlayXbox
              Interaction.Home HomeInteraction.WatchTv ]
        | _ -> []
