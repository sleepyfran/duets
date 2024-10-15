module Duets.Simulation.Events.NonInteractiveGame

open Duets.Entities
open Duets.Simulation

/// Runs all the events associated with certain effects that have happened that
/// require the engine to improve the skills of the band or the character. For
/// example, starting a song applies a 50% chance of improving the skills of the
/// character and the rest of the band.
let internal run effect =
    match effect with
    | GamePlayed(PlayResult.Darts result)
    | GamePlayed(PlayResult.Pool result) ->
        match result with
        | SimpleResult.Win ->
            [ Character.Attribute.addToPlayable
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.winningNonInteractiveGameIncrease ]
        | SimpleResult.Lose ->
            [ Character.Attribute.addToPlayable
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.losingNonInteractiveGameIncrease ]
        |> ContinueChain
        |> Some
    | _ -> None
