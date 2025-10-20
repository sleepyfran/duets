namespace Duets.Entities

[<AutoOpen>]
module CarTypes =
    [<Measure>]
    type horsepower

    /// Defines a car that can be purchased and driven.
    type Car = { Power: int<horsepower> }
