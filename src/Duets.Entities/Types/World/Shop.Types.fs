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
        | Vietnamese
