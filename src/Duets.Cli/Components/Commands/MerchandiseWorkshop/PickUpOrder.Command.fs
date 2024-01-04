namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation.Merchandise.PickUp

[<RequireQualifiedAccess>]
module PickUpMerchandiseOrdersCommand =
    /// Command to pick up all the merchandise orders that are available in the shop.
    let create (items: Item list) =
        { Name = "pick order"
          Description =
            "Allows you to pick up any order that is already available"
          Handler =
            fun _ ->
                pickUpOrder (State.get ()) items |> Effect.applyMultiple

                Scene.World }
