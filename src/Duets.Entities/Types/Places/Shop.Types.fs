namespace Duets.Entities

[<AutoOpen>]
module ShopTypes =
    /// Measure for the price multiplier of shops.
    [<Measure>]
    type multiplier

    /// Defines the type of cuisine a restaurant serves.
    type RestaurantCuisine =
        | American
        | Czech
        | Italian
        | French
        | Japanese
        | Mexican
        | Turkish
        | Vietnamese
        | Spanish

    /// Defines the type of cars that a dealer sells.
    type CarPriceRange =
        | Budget
        | MidRange
        | Premium

    /// Represents a specific type of shop where the character can buy cars, which
    /// has a dealer that manages the purchase.
    type CarDealer =
        { Dealer: Character
          PriceRange: CarPriceRange }
