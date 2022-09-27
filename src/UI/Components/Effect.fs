module UI.Components.Effect

open Agents
open Common
open Entities
open Simulation

/// <summary>
/// Applies an effect to the simulation and displays any message or action that
/// is associated with that effect. For example, transferring money displays a
/// message with the transaction.
/// </summary>
/// <param name="effect">Effect to apply</param>
let rec apply effect =
    let effects, state =
        Simulation.tick (State.get ()) effect

    State.set state

    effects |> Seq.tap Log.append |> ignore
