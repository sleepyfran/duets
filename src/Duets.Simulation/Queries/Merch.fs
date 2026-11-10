namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Simulation

module Merch =
    /// Retrieves the price of a merch item for the given band.
    let itemPrice state bandId itemProperty =
        let lens =
            Lenses.State.merchPrices_ >-> Map.keyWithDefault_ bandId Map.empty
            >?> Map.key_ itemProperty

        Optic.get lens state

    /// Retrieves the price of a merch item for the given band.
    let itemPrice' state bandId item =
        let mainProperty = Item.Property.tryMain item

        match mainProperty with
        | Some property -> itemPrice state bandId property
        | None -> None

    /// Returns the cost of producing the given item.
    let itemProductionCost itemProperty =
        match itemProperty with
        | Listenable(CD, _) -> Config.MusicSimulation.Merch.cdPrice
        | Listenable(Vinyl, _) -> Config.MusicSimulation.Merch.vinylPrice
        | Wearable(Hoodie) -> Config.MusicSimulation.Merch.hoodiePrice
        | Wearable(TShirt) -> Config.MusicSimulation.Merch.tShirtPrice
        | Wearable(ToteBag) -> Config.MusicSimulation.Merch.toteBagPrice
        | _ -> 0m<dd>

    /// Lists all the merch items that the current band can order.
    let availableItems state : MerchandiseItem list =
        let band = Bands.currentBand state

        let albumMerchandise =
            Albums.releasedByBand state band.Id
            |> List.collect (fun releasedAlbum ->
                let album = Album.fromReleased releasedAlbum

                [ CD; Vinyl ]
                |> List.map (fun physicalMediaType ->
                    let itemPrice =
                        itemProductionCost (
                            Listenable(physicalMediaType, album.Id)
                        )

                    { Item =
                        { Brand = band.Name
                          Name = album.Name
                          Properties =
                            [ Listenable(physicalMediaType, album.Id) ] }
                      MinPieces =
                        Config.MusicSimulation.Merch.minimumPhysicalMediaOrders
                      MaxPieces =
                        Config.MusicSimulation.Merch.maximumPhysicalMediaOrders
                      PricePerPiece = itemPrice }))

        let wearableMerchandise =
            [ Hoodie; TShirt; ToteBag ]
            |> List.map (fun wearableItem ->
                let itemPrice =
                    itemProductionCost (Wearable wearableItem)

                let itemName = Union.caseName wearableItem

                { Item =
                    { Brand = band.Name
                      Name = itemName
                      Properties = [ Wearable wearableItem ] }
                  MinPieces =
                    Config.MusicSimulation.Merch.minimumPhysicalMediaOrders
                  MaxPieces =
                    Config.MusicSimulation.Merch.maximumPhysicalMediaOrders
                  PricePerPiece = itemPrice })

        albumMerchandise @ wearableMerchandise

    /// Returns the recommended price of a merch item so that it will sell for
    /// the given band.
    let recommendedItemPrice state bandId itemProperty =
        let bandFame = Bands.estimatedFameLevel state bandId

        let bandFameModifier =
            match bandFame with
            | fame when fame < 25 -> 1.5m
            | fame when fame < 50 -> 2m
            | fame when fame < 75 -> 3m
            | _ -> 4m

        itemProductionCost itemProperty * bandFameModifier

    /// Returns the recommended price of a merch item so that it will sell for
    /// the given band.
    let recommendedItemPrice' state bandId item =
        let mainProperty = Item.Property.tryMain item

        match mainProperty with
        | Some property -> recommendedItemPrice state bandId property
        | None -> 0m<dd>
