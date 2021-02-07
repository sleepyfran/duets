module Orchestrator

open System
open View.Actions
open View.Scenes.Index
open View.Scenes.BandCreator
open View.Scenes.CharacterCreator
open View.Scenes.MainMenu

/// Defines the functions that a renderer should contain. This should be
/// implemented in the specific UI and passed into the orchestrator.
type IRenderer =
  abstract RenderPrompt: Prompt -> string
  abstract RenderMessage: Text -> unit
  abstract Clear: unit -> unit

/// Returns the sequence of actions associated with a screen given its name.
let actionsFrom scene state =
  match scene with
  | MainMenu -> mainMenu state
  | CharacterCreator -> characterCreator ()
  | BandCreator character -> bandCreator character

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith (renderer: IRenderer) state chain =
  Seq.fold
    (fun state action ->
      match action with
      | Effect effect -> effect state
      | Prompt prompt ->
          renderer.RenderPrompt prompt
          |> fun input ->
               match prompt.Content with
               | ChoicePrompt (choices, handler) ->
                   handler (choiceById input choices)
               | ConfirmationPrompt handler ->
                   handler (input |> Convert.ToBoolean)
               | NumberPrompt handler -> handler (input |> int)
               | TextPrompt handler -> handler input
               |> runWith renderer state
      | Message message ->
          renderer.RenderMessage message
          state
      | Scene scene -> runWith renderer state (actionsFrom scene state)
      | NoOp -> state)
    state
    chain
