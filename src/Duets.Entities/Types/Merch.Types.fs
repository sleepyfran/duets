namespace Duets.Entities

[<AutoOpen>]
module rec MerchTypes =
    /// Defines the prices for each kind of merch that the band can sell.
    type BandMerchPrices = Map<BandId, Map<ItemProperty, Amount>>
