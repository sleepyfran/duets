namespace Duets.Entities

[<AutoOpen>]
module rec MerchTypes =
    /// Defines the prices for each kind of merch that the band can sell.
    type BandMerchPrices = Map<BandId, Map<ItemProperty, Amount>>

    /// Defines the fans waiting at the merch stand of a venue after a concert.
    type MerchStandQueue =
        { Venue: CityId * PlaceId
          People: int
          /// Last moment in which the queue is still there.
          Until: Date }
