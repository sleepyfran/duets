module Duets.Simulation.Events.Moodlets.Cleanup

open Duets.Common
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
                let daysSinceStart =
                    Calendar.Query.daysBetween moodlet.StartedOn currentDate
                    * -1

                daysSinceStart <= daysToExpire
            | MoodletExpirationTime.AfterDayMoments dayMomentsToExpire ->
                let dayMomentsSinceStart =
                    Calendar.Query.dayMomentsBetween
                        moodlet.StartedOn
                        currentDate

                dayMomentsSinceStart <= dayMomentsToExpire)

    (character.Id, Diff(currentMoodlets, updatedMoodlets))
    |> CharacterMoodletsChanged
    |> List.singleton
