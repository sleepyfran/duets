module Orchestrator

open View.Actions

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let runWith render state chain =
  Seq.fold
    (fun state action ->
      match action with
      | Effect effect -> effect state
      | _ ->
          render action |> ignore
          state)
    state
    chain
