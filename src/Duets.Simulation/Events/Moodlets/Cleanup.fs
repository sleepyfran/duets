module Duets.Simulation.Events.Moodlets.Cleanup

open Duets.Entities
open Duets.Simulation

/// Removes any moodlets from the playable character that might have expired.
let cleanup state =
    let currentDate = Queries.Calendar.today state
    let character = Queries.Characters.playableCharacter state
    let currentMoodlets = Queries.Characters.moodlets character

    let updatedMoodlets =
        currentMoodlets
        |> Set.filter (fun moodlet ->
            match moodlet.Expiration with
            | MoodletExpirationTime.Never -> true
            | MoodletExpirationTime.AfterDays daysToExpire ->
                let daysSinceStart = Moodlet.daysSinceStart moodlet currentDate

                daysSinceStart <= daysToExpire
            | MoodletExpirationTime.AfterDayMoments dayMomentsToExpire ->
                let dayMomentsSinceStart =
                    Moodlet.dayMomentsSinceStart moodlet currentDate

                dayMomentsSinceStart <= dayMomentsToExpire)

    (character.Id, Diff(currentMoodlets, updatedMoodlets))
    |> CharacterMoodletsChanged
    |> List.singleton
