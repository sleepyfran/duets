module Duets.Simulation.Events.Moodlets.JetLagged

open Duets.Entities
open Duets.Simulation

/// Checks if the character has travelled to a new city with a time difference of
/// more than 4 hours from the previous city, and in that case applies the
/// JetLagged moodlet.
let applyIfNeeded prevCityId currCityId state =
    let prevCity = Queries.World.cityById prevCityId
    let currCity = Queries.World.cityById currCityId

    (*
    Apply jet lag when the cities' timezones differ by more than 4 hours.
    *)
    let (Utc prevCityOffset) = prevCity.Timezone
    let (Utc currCityOffset) = currCity.Timezone

    let shouldApplyMoodlet =
        abs (prevCityOffset - currCityOffset) > 4<utcOffset>

    if shouldApplyMoodlet then
        let moodlet =
            Character.Moodlets.createFromNow
                state
                MoodletType.JetLagged
                (MoodletExpirationTime.AfterDays 3<days>)

        [ Character.Moodlets.apply state moodlet ]
    else
        []
