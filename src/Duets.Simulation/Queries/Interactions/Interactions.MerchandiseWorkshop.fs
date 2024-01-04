namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities
open Duets.Simulation

module MerchandiseWorkshop =
    let private createAvailableItems state : MerchandiseItem list =
        let band = Queries.Bands.currentBand state

        let albumMerchandise =
            Queries.Albums.releasedByBand state band.Id
            |> List.collect (fun releasedAlbum ->
                let album = Album.fromReleased releasedAlbum

                [ CD, Config.MusicSimulation.Merch.cdPrice
                  Vinyl, Config.MusicSimulation.Merch.vinylPrice ]
                |> List.map (fun (physicalMediaType, price) ->
                    { Item =
                        { Brand = band.Name
                          Name = album.Name
                          Properties =
                            [ Listenable(physicalMediaType, album.Id) ] }
                      MinPieces =
                        Config.MusicSimulation.Merch.minimumPhysicalMediaOrders
                      MaxPieces =
                        Config.MusicSimulation.Merch.maximumPhysicalMediaOrders
                      PricePerPiece = price }))

        let wearableMerchandise =
            [ Hoodie, Config.MusicSimulation.Merch.hoodiePrice
              TShirt, Config.MusicSimulation.Merch.tShirtPrice
              ToteBag, Config.MusicSimulation.Merch.toteBagPrice ]
            |> List.map (fun (wearableItem, price) ->
                let itemName = Union.caseName wearableItem

                { Item =
                    { Brand = band.Name
                      Name = itemName
                      Properties = [ Wearable wearableItem ] }
                  MinPieces =
                    Config.MusicSimulation.Merch.minimumPhysicalMediaOrders
                  MaxPieces =
                    Config.MusicSimulation.Merch.maximumPhysicalMediaOrders
                  PricePerPiece = price })

        albumMerchandise @ wearableMerchandise

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
                        deliveryDate = currentDate
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
            [ createAvailableItems state
              |> MerchandiseWorkshopInteraction.OrderMerchandise
              |> Interaction.MerchandiseWorkshop

              yield! createListOrders state coords
              yield! createPickUpItems state coords ]
        | _ -> []
