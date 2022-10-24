namespace Entities

[<AutoOpen>]
module SituationsTypes =
    /// Defines all situations in which the character can be in.
    type Situation =
        /// Player is exploring the world in no specific situation.
        | FreeRoam
        /// Player is performing a concert.
        | InConcert of OngoingConcert
        /// Player is inside of the backstage and *might* be performing in a concert.
        | InBackstage of OngoingConcert option
