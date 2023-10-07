module Duets.Simulation.Character.Moodlets

open Duets.Entities
open Duets.Simulation

/// Creates a moodlet of the given type and expiration that starts from the
/// current date.
let createFromNow state moodletType expiration =
    let currentDate = Queries.Calendar.today state
    Moodlet.create moodletType currentDate expiration

/// Applies the given moodlet to the playable character. If the character
/// has already a moodlet of the same type it will be replaced to ensure that
/// the `StartedOn` and `Expiration` fields are updated.
let apply state moodlet =
    let character = Queries.Characters.playableCharacter state
    let currentMoodlets = character |> Queries.Characters.moodlets

    let updatedMoodlets =
        currentMoodlets
        |> Set.filter (fun m -> m.MoodletType <> moodlet.MoodletType)
        |> Set.add moodlet

    (character.Id, Diff(currentMoodlets, updatedMoodlets))
    |> CharacterMoodletsChanged
