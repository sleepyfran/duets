module Duets.Simulation.State.Calendar

open Aether
open Duets.Entities

let setTime time = Optic.set Lenses.State.today_ time

let setTurnMinutes time =
    Optic.set Lenses.State.turnMinutes_ time
