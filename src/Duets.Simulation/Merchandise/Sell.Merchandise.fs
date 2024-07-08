[<RequireQualifiedAccess>]
module rec Duets.Simulation.Merchandise.Sell

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

/// Simulates the sale of merchandise for a specific concert, based on the
/// merch that the artist has available, the number of people that attended
/// the concert and the price of the merch. Creates effects describing the
/// sale of the merch, or an empty list if no merch was sold or available.
let afterConcert (band: Band) performedConcert state =
    let concert = Concert.fromPast performedConcert
    let merch = Queries.Inventory.band band.Id state
    let totalPossibleMerchSales = attendantsBuyingMerch performedConcert

    if Map.isEmpty merch then
        []
    else
        let _, soldItems =
            merch
            |> Map.toList
            |> List.shuffle
            |> List.fold
                (fun (merchSalesLeft, soldItems) (merch, stock) ->
                    let itemPrice =
                        Queries.Merch.itemPrice' state band.Id merch
                        |> Option.defaultValue 0m<dd>

                    let recommendedPrice =
                        Queries.Merch.recommendedItemPrice' state band.Id merch

                    let salePercentage =
                        itemSalePercentageBasedOnPrice
                            itemPrice
                            recommendedPrice

                    let quantityToSell = min stock merchSalesLeft

                    let quantityToSell =
                        float quantityToSell * salePercentage
                        |> Math.ceilToNearest
                        |> (*) 1<quantity>

                    if quantityToSell > 0<quantity> then
                        let merchSalesLeft = merchSalesLeft - quantityToSell
                        (merchSalesLeft, (merch, quantityToSell) :: soldItems)
                    else
                        merchSalesLeft, soldItems)
                (totalPossibleMerchSales, [])

        let merchEarnings = incomeFromSoldItems state band soldItems
        let bandAccount = Band band.Id

        [ MerchSold(band, soldItems, merchEarnings)
          income state bandAccount merchEarnings ]

let private incomeFromSoldItems state (band: Band) soldItems =
    soldItems
    |> List.sumBy (fun (item, quantity) ->
        let itemPrice =
            Queries.Merch.itemPrice' state band.Id item
            |> _.Value (* We've pre-validated this in the previous step. *)

        itemPrice * decimal quantity)

let private itemSalePercentageBasedOnPrice itemPrice recommendedPrice =
    let priceDifference = itemPrice - recommendedPrice
    let percentage = priceDifference / recommendedPrice

    match percentage with
    | p when p <= 0m -> 1.0
    | p when p <= 0.1m -> 0.5
    | p when p <= 0.5m -> 0.1
    | _ -> 0.0

let private attendantsBuyingMerch concert =
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
