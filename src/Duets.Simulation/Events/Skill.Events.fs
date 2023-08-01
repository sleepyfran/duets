module Duets.Simulation.Events.Skill

open Duets.Entities
open Duets.Simulation.Skills.Improve

/// Runs all the events associated with certain effects that have happened that
/// require the engine to improve the skills of the band or the character. For
/// example, starting a song applies a 50% chance of improving the skills of the
/// character and the rest of the band.
let internal run effect =
    match effect with
    | SongStarted(band, _) ->
        [ Composition.improveBandSkillsChance band ] |> ContinueChain |> Some
    | SongImproved(band, _) ->
        [ Composition.improveBandSkillsChance band ] |> ContinueChain |> Some
    | SongPracticed(band, _) ->
        [ Composition.improveBandSkillsChance band ] |> ContinueChain |> Some
    | _ -> None
