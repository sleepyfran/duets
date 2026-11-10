namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
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

    /// Returns how many people want to buy merch after the given concert.
    let buyersAfterConcert concert =
        match concert with
        | PerformedConcert(concert, quality) ->
            let qualityBasedPercentage =
                match quality with
                | q when q < 25<quality> -> 0.001
                | q when q < 50<quality> -> 0.05
                | q when q < 75<quality> -> 0.15
                | _ -> 0.3

            let percentage =
                match concert.ParticipationType with
                | Headliner -> qualityBasedPercentage
                | OpeningAct _ ->
                    (* Opening a concert doesn't usually give much merch sales. *)
                    qualityBasedPercentage * 0.015

            float concert.TicketsSold * percentage
            |> Math.ceilToNearest
            |> (*) 1<quantity>
        | _ ->
            (* This technically should not happen, but let's just say they don't sell anything. *)
            0<quantity>

    /// Returns how many people are waiting at the merch stand of the given
    /// venue, or None if the band hasn't set up a stand there.
    let standQueue state cityId venueId =
        match Situations.current state with
        | Concert(Preparing checklist) when checklist.MerchStandSetup -> Some 0
        | Concert(InConcert ongoingConcert) when
            ongoingConcert.Checklist.MerchStandSetup
            ->
            Some 0
        | _ ->
            let today = Queries.Calendar.today state

            Optic.get Lenses.State.merchStandQueue_ state
            |> Option.filter (fun queue ->
                queue.Venue = (cityId, venueId) && today <= queue.Until)
            |> Option.map _.People
