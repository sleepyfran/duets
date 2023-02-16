namespace Duets.Entities

module SituationTypes =
    /// Situations that happen while on an airport/airplane.
    type AirportSituation = Flying of Flight

    /// Situations that happen while on a concert.
    type ConcertSituation =
        | InConcert of OngoingConcert
        | InBackstage of OngoingConcert option

    /// Defines all situations in which the character can be in.
    type Situation =
        /// Player is exploring the world in no specific situation.
        | FreeRoam
        /// Player is inside of an airport about to flight or flying.
        | Airport of AirportSituation
        /// Player is performing a concert.
        | Concert of ConcertSituation
