namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module SeeMenuCommand =
    /// Command to display the menu available on a restaurant or bar.
    let create (availableItems: PurchasableItem list) =
        { Name = "see menu"
          Description = Command.seeMenuDescription
          Handler =
            (fun _ ->
                let tableColumns =
                    [ Shop.itemNameHeader
                      Shop.itemTypeHeader
                      Shop.itemPriceHeader ]

                let tableRows =
                    availableItems
                    |> List.sortBy (fun (item, _) -> item.Brand)
                    |> List.map (fun (item, price) ->
                        [ item.Brand
                          Shop.itemType item
                          Shop.itemPrice price ])

                showTable tableColumns tableRows

                Scene.World) }
