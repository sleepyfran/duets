module Orchestrator

open System
open Cli.View.Actions
open Cli.View.Scenes.BandCreator
open Cli.View.Scenes.CharacterCreator
open Cli.View.Scenes.MainMenu
open Cli.View.Scenes.RehearsalRoom

/// Returns the sequence of actions associated with a screen given its name.
let actionsFrom scene =
  match scene with
  | MainMenu -> mainMenu ()
  | CharacterCreator -> characterCreator ()
  | BandCreator character -> bandCreator character
  | RehearsalRoom -> rehearsalRoom ()

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
