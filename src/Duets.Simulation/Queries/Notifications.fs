namespace Duets.Simulation.Queries

open Aether
open Duets.Entities

module Notifications =
    /// Returns all the currently scheduled notifications for the given date and
    /// day moment.
    let forDate state date =
        let normalizedDate = date |> Calendar.Transform.resetDayMoment
        let dayMoment = Calendar.Query.dayMomentOf date

        let lens =
            Lenses.FromState.Notifications.forDateDayMoment_
                normalizedDate
                dayMoment

        Optic.get lens state |> Option.defaultValue []
