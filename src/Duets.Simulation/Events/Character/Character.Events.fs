module Duets.Simulation.Events.Character.Character

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Events

/// Runs all the events associated with a character. For example, when the health
/// of the character goes below a certain threshold we should hospitalize the
/// character.
let internal run effect =
    match effect with
    | BandFansChanged(band, _) ->
        ContinueChain [ Fame.followBandsFame band.Id ] |> Some
    | CharacterAttributeChanged(character, attribute, Diff(_, amount)) when
        attribute = CharacterAttribute.Health && amount < 10
        ->
        BreakChain [ Hospitalization.hospitalize character ] |> Some
    | ConcertCancelled _ ->
        (*
        TODO: If we ever emit this events for other bands remember to check if the concert failed for the character's band.
        *)
        ContinueChain
            [ Character.Attribute.addToPlayable
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.concertFailIncrease ]
        |> Some
    | ConcertFinished(_, pastConcert, _) ->
        let quality =
            match pastConcert with
            | PerformedConcert(_, quality) -> quality
            | _ -> 0<quality>

        match quality with
        | q when q < 35<quality> ->
            Config.LifeSimulation.Mood.concertPoorResultIncrease
        | q when q < 75<quality> ->
            Config.LifeSimulation.Mood.concertNormalResultIncrease
        | _ -> Config.LifeSimulation.Mood.concertGoodResultIncrease
        |> Character.Attribute.addToPlayable CharacterAttribute.Mood
        |> List.ofItem
        |> ContinueChain
        |> Some
    | SongImproved _
    | SongStarted _ ->
        ContinueChain
            [ Character.Attribute.addToPlayable CharacterAttribute.Energy -20
              Character.Attribute.addToPlayable
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.playingMusicIncrease ]
        |> Some
    | SongPracticed _ ->
        ContinueChain
            [ Character.Attribute.addToPlayable CharacterAttribute.Energy -10
              Character.Attribute.addToPlayable
                  CharacterAttribute.Mood
                  Config.LifeSimulation.Mood.playingMusicIncrease ]
        |> Some
    | TimeAdvanced _ ->
        ContinueChain
            [ Drunkenness.soberUpAfterTime
              Drunkenness.reduceHealth
              Hunger.reduceHealth ]
        |> Some
    | _ -> None
