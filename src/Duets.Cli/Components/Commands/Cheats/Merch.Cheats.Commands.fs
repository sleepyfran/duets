namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module MerchCommands =
    /// Allows the player to order merch that is added to the band's inventory
    /// immediately and for free.
    let sweatshop =
        { Name = "sweatshop"
          Description =
            "Allows you to order merch that gets delivered to your band's inventory immediately and for free"
          Handler =
            fun _ ->
                let state = State.get ()
                let band = Queries.Bands.currentBand state
                let merch = 
                    Queries.Merch.availableItems state
                    |> OrderMerchandiseCommand.promptForItem

                match merch with
                | Some merchItem ->
                    let quantity =
                        OrderMerchandiseCommand.promptForQuantity merchItem

                    ItemAddedToBandInventory(band, merchItem.Item, quantity)
                    |> Effect.apply
                | None -> ()

                Scene.Cheats }
