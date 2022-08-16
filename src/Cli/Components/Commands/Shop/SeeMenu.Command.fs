namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

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
                    |> List.map (fun (item, price) ->
                        [ item.Brand
                          Shop.itemType item.Type
                          Shop.itemPrice price ])

                showTable tableColumns tableRows

                Scene.World) }
