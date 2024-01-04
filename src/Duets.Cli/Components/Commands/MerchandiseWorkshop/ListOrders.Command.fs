namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module ListMerchandiseOrdersCommand =
    /// Command to list all the previous orders of a merch workshop.
    let create (items: (Date * DeliverableItem) list) =
        { Name = "list orders"
          Description = "Shows all the orders you have pending shipping"
          Handler =
            fun _ ->
                "These are your upcoming orders:"
                |> Styles.information
                |> showMessage

                let tableColumns =
                    [ Styles.header "Item"
                      Styles.header "Quantity"
                      Styles.header "Delivery date" ]

                let tableRows =
                    items
                    |> List.map (fun (date, item) ->
                        match item with
                        | DeliverableItem.Description(item, quantity) ->
                            [ item.Name
                              $"{quantity}"
                              Generic.dateWithDay date ])

                showTable tableColumns tableRows

                Scene.World }
