namespace Entities

[<AutoOpen>]
module SituationsTypes =
    /// Situations that happen while on a concert.
    type ConcertSituation =
        | InConcert of OngoingConcert
        | InBackstage of OngoingConcert option

    /// Defines all situations in which the character can be in.
    type Situation =
        /// Player is exploring the world in no specific situation.
        | FreeRoam
        /// Player is performing a concert.
        | Concert of ConcertSituation
