namespace Entities

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