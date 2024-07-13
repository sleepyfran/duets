namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities.SituationTypes
open Duets.Simulation

module Situational =
    let internal interaction state =
        let currentSituation = Queries.Situations.current state

        match currentSituation with
        | Focused(UsingComputer(item, computer)) ->
            Computer.interaction item computer
        | _ -> []
