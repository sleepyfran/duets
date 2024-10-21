module Duets.Simulation.Interactions.Sleep

open Duets.Entities
open Duets.Simulation

/// Sleeps until the given date and day moment, advancing the time and updating
/// the energy and health of the playable character. If the player sleeps more
/// than 3 day moments, the health will be negatively affected and sleep will
/// not be as effective.
let sleep state untilDate untilDayMoment =
    let character = Queries.Characters.playableCharacter state
    let currentDate = Queries.Calendar.today state

    let endDate = untilDate |> Calendar.Transform.changeDayMoment untilDayMoment

    let sleepDuration = Calendar.Query.dayMomentsBetween currentDate endDate

    let rawDuration = sleepDuration / 1<dayMoments>

    let energy, health =
        match sleepDuration with
        | duration when duration > 3<dayMoments> ->
            rawDuration * 20, rawDuration * -1
        | _ -> rawDuration * 40, rawDuration * 10

    [ CharacterSlept(character.Id, sleepDuration)
      yield!
          Character.Attribute.addToPlayable
              CharacterAttribute.Energy
              energy
              state
      yield!
          Character.Attribute.addToPlayable
              CharacterAttribute.Health
              health
              state ]
