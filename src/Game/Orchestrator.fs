module Orchestrator

open Action

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith render state chain =
  match chain with
  | Some(chain) ->
    render chain.Current |> ignore
    Array.fold (fun acc effect -> effect acc) state chain.Effects
    |> fun state -> runWith render state (chain.Next state) 
  | None -> ()