namespace Duets.Entities

[<AutoOpen>]
module CommonTypes =
    /// Defines the type that all entities with an ID should use.
    type Identity = System.Guid

    /// Defines the before and after of an action.
    type Diff<'a> = Diff of before: 'a * after: 'a

    /// Measure for the in-game currency. DD as in DuetsDollars. Imagination
    /// at its best.
    [<Measure>]
    type dd

    /// Measure for counting number of times.
    [<Measure>]
    type times

    /// Defines an amount in the in-game currency.
    type Amount = decimal<dd>

    /// Measure for the quality of something, as a percentage.
    [<Measure>]
    type quality

    /// Measure for how many things of something.
    [<Measure>]
    type quantity

    type Quality = int<quality>
    type MaxQuality = int<quality>

    /// Measure for counting milliliters of something.
    [<Measure>]
    type milliliter

    /// Measure for counting grams of something.
    [<Measure>]
    type gram

    /// Measure for counting kilometers.
    [<Measure>]
    type km

    /// Measure for percentages.
    [<Measure>]
    type percent
