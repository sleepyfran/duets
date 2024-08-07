module Duets.Simulation.Situations

open Duets.Entities
open Duets.Entities.SituationTypes

/// Sets the current situation to free roam.
let freeRoam = SituationChanged FreeRoam

/// Sets the current situation to preparing for a concert.
let preparingConcert =
    Preparing
        { SoundcheckDone = false
          MerchStandSetup = false }
    |> Concert
    |> SituationChanged

/// Sets the current situation to preparing for a concert, but with the given
/// checklist.
let preparingConcert' checklist =
    checklist |> Preparing |> Concert |> SituationChanged

/// Sets the current situation that the character is in to in concert with the
/// given ongoing concert. This should also be used to update the ongoing
/// concert situation.
let inConcert ongoingConcert =
    InConcert ongoingConcert |> Concert |> SituationChanged
    
/// Sets the current situation to focused, which makes the character focus
/// on a specific task and prevents them from doing other things.
let focused focusedState =
    focusedState |> Focused |> SituationChanged

/// Sets the current situation to inside of the plane, flying somewhere.
let onboardedInPlane flight =
    flight |> Flying |> Airport |> SituationChanged

/// Sets the current situation to playing a mini game.
let playingMiniGame miniGameState =
    miniGameState |> PlayingMiniGame |> SituationChanged

/// Sets the current situation to socializing.
let socializing socializingState =
    socializingState |> Socializing |> SituationChanged
