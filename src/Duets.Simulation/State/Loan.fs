module Duets.Simulation.State.Loan

open Aether
open Duets.Entities

let setLoan loan state =
    Optic.set Lenses.FromState.Loan.activeLoan_ (Some loan) state

let removeLoan state =
    Optic.set Lenses.FromState.Loan.activeLoan_ None state

let setReputation reputation state =
    Optic.set Lenses.FromState.Loan.reputation_ reputation state
