namespace Duets.Entities

[<AutoOpen>]
module ShopTypes =
    /// Measure for the price multiplier of shops.
    [<Measure>]
    type multiplier

    /// Defines a shop in-game with all the items that they sell.
    type Shop =
        { AvailableItems: PurchasableItem list
          /// Defines a multiplier to be applied on top of the base price
          /// of each item. For example, if given 2 then all the prices of the
          /// items above will be BasePrice * 2.
          PriceModifier: int<multiplier> }
