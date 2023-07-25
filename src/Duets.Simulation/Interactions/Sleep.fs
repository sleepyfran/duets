module Duets.Simulation.Interactions.Sleep

open Duets.Entities
open Duets.Simulation

/// Sleeps until the given date and day moment, advancing the time and updating
/// the energy and health of the playable character. If the player sleeps more
/// than 3 day moments, the health will be negatively affected and sleep will
/// not be as effective.
///
/// If the item passed is not a bed, an error will be returned.
let sleep state untilDate untilDayMoment (item: Item) =
    match item.Type with
    | Interactive(InteractiveItemType.Furniture(FurnitureItemType.Bed)) ->
        let currentDate = Queries.Calendar.today state

        let endDate =
            untilDate |> Calendar.Transform.changeDayMoment untilDayMoment

        let sleepDuration = Calendar.Query.dayMomentsBetween currentDate endDate

        let rawDuration = sleepDuration / 1<dayMoments>

        let energy, health =
            match sleepDuration with
            | duration when duration > 3<dayMoments> ->
                rawDuration * 20, rawDuration * -1
            | _ -> rawDuration * 40, rawDuration * 10

        [ yield! Time.AdvanceTime.advanceDayMoment' state sleepDuration
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
        |> Ok
    | _ -> Error Items.ActionNotPossible
