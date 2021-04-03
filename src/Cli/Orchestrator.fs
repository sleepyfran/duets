module Orchestrator

open System
open View.Actions
open View.Scenes.BandCreator
open View.Scenes.CharacterCreator
open View.Scenes.MainMenu

/// Returns the sequence of actions associated with a screen given its name.
let actionsFrom scene =
  match scene with
  | MainMenu -> mainMenu ()
  | CharacterCreator -> characterCreator ()
  | BandCreator character -> bandCreator character

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith renderPrompt renderMessage chain =
  chain
  |> Seq.iter (fun action ->
       match action with
       | Prompt prompt ->
           renderPrompt prompt
           |> fun input ->
                match prompt.Content with
                | ChoicePrompt (choices, handler) ->
                    handler (choiceById input choices)
                | ConfirmationPrompt handler ->
                    handler (input |> Convert.ToBoolean)
                | NumberPrompt handler -> handler (input |> int)
                | TextPrompt handler -> handler input
                |> runWith renderPrompt renderMessage
       | Message message ->
           renderMessage message
           ()
       | Scene scene -> runWith renderPrompt renderMessage (actionsFrom scene)
       | NoOp -> ())
