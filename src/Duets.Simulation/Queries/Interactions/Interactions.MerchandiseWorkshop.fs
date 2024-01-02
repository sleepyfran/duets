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
                    { Brand = band.Name
                      Name = album.Name
                      Properties = [ Listenable(physicalMediaType, album.Id) ] },
                    Config.MusicSimulation.Merch.minimumPhysicalMediaOrders,
                    Config.MusicSimulation.Merch.maximumPhysicalMediaOrders,
                    price))

        let wearableMerchandise =
            [ Hoodie, Config.MusicSimulation.Merch.hoodiePrice
              TShirt, Config.MusicSimulation.Merch.tShirtPrice
              ToteBag, Config.MusicSimulation.Merch.toteBagPrice ]
            |> List.map (fun (wearableItem, price) ->
                let itemName = Union.caseName wearableItem

                { Brand = band.Name
                  Name = itemName
                  Properties = [ Wearable wearableItem ] },
                Config.MusicSimulation.Merch.minimumPhysicalMediaOrders,
                Config.MusicSimulation.Merch.maximumPhysicalMediaOrders,
                price)

        albumMerchandise @ wearableMerchandise

    /// Gather all available interactions inside a merchandise workshop, which
    /// allows the player to create designs for their merchandise and make them
    /// to sell them later at concerts.
    let internal interactions state roomType =
        match roomType with
        | RoomType.Workshop ->
            [ createAvailableItems state
              |> MerchandiseWorkshopInteraction.OrderMerchandise
              |> Interaction.MerchandiseWorkshop ]
        | _ -> []
