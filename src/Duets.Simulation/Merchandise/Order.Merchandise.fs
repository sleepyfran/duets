module Duets.Simulation.Merchandise.Order

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

type OrderError =
    | MinNotReached of min: int<quantity>
    | MaxSurpassed of max: int<quantity>
    | NotEnoughFunds

let private generatePayment state merchItem quantity =
    let band = Queries.Bands.currentBand state
    let bandAccount = Band band.Id
    let quantity = quantity / 1<quantity> |> decimal

    let totalPayment = merchItem.PricePerPiece * quantity

    expense state bandAccount totalPayment

let private createOrderEffects state merchItem quantity =
    (* We assume that this was called from a merch workshop. *)
    let coordinates = Queries.World.currentCoordinates state
    let cityId, _, _ = coordinates
    let place = Queries.World.currentPlace state
    let currentDate = Queries.Calendar.today state

    let deliveryDate = currentDate |> Calendar.Ops.addDays 7
    let currentDayMoment = Calendar.Query.dayMomentOf currentDate

    let deliverable =
        { Brand = "DuetsDelivery"
          Name = "Merch Delivery"
          Properties =
            [ (deliveryDate,
               DeliverableItem.Description(merchItem.Item, quantity))
              |> Deliverable ] }

    let deliveryNotification =
        Notification.DeliveryArrived(cityId, place.Id, DeliveryType.Merchandise)

    [ ItemAddedToWorld(coordinates, deliverable)
      Notifications.create deliveryDate currentDayMoment deliveryNotification ]

let private orderMerch' state merchItem quantity =
    let billingResult = generatePayment state merchItem quantity

    match billingResult with
    | Ok billingEffects ->
        let orderEffects = createOrderEffects state merchItem quantity
        Ok(billingEffects @ orderEffects)
    | Error _ -> Error NotEnoughFunds

/// Attempts to order the given merch item a given number of times, checking
/// if the band has enough funds to pay for the items and checking that the
/// quantity is inside the supported range of the item.
let orderMerch state merchItem quantity =
    if quantity < merchItem.MinPieces then
        MinNotReached merchItem.MinPieces |> Error
    else if quantity > merchItem.MaxPieces then
        MaxSurpassed merchItem.MaxPieces |> Error
    else
        orderMerch' state merchItem quantity
