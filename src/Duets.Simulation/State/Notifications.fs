module Duets.Simulation.State.Notifications

open Aether
open Duets.Entities

let schedule date dayMoment notification =
    let lens = Lenses.FromState.Notifications.forDateDayMoment_ date dayMoment
    Optic.map lens (fun ns -> notification :: ns)
