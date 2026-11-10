namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities
open Duets.Simulation

module MerchandiseWorkshop =
    let private ordersInPlace state coords =
        Queries.Items.allWithHiddenIn state coords
        |> List.collect (fun item ->
            item.Properties
            |> List.choose (function
                | Deliverable(deliveryDate, items) -> Some(deliveryDate, items)
                | _ -> None))

    let private createListOrders state coords =
        let items =
            Queries.Items.allWithHiddenIn state coords
            |> List.collect (fun item ->
                item.Properties
                |> List.choose (function
                    | Deliverable(deliveryDate, items) ->
                        Some(deliveryDate, items)
                    | _ -> None))

        match items with
        | [] -> []
        | items ->
            [ items
              |> MerchandiseWorkshopInteraction.ListOrderedMerchandise
              |> Interaction.MerchandiseWorkshop ]

    let private createPickUpItems state coords =
        let currentDate = Queries.Calendar.today state

        let items =
            Queries.Items.allWithHiddenIn state coords
            |> List.collect (fun item ->
                item.Properties
                |> List.choose (function
                    | Deliverable(deliveryDate, _) when
                        deliveryDate <= currentDate
                        ->
                        Some item
                    | _ -> None))

        match items with
        | [] -> []
        | items ->
            [ items
              |> MerchandiseWorkshopInteraction.PickUpMerchandise
              |> Interaction.MerchandiseWorkshop ]

    /// Gather all available interactions inside a merchandise workshop, which
    /// allows the player to create designs for their merchandise and make them
    /// to sell them later at concerts.
    let internal interactions state coords roomType =
        match roomType with
        | RoomType.Workshop ->
            [ Queries.Merch.availableItems state
              |> MerchandiseWorkshopInteraction.OrderMerchandise
              |> Interaction.MerchandiseWorkshop

              yield! createListOrders state coords
              yield! createPickUpItems state coords ]
        | _ -> []
