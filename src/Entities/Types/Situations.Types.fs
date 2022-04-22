namespace Entities

[<AutoOpen>]
module SituationsTypes =
    /// Defines all situations in which the character can be in.
    type Situation =
        | FreeRoam // Player is exploring the world in no specific situation.
        | InConcert of OngoingConcert // Player is performing a concert.
