module Orchestrator

open System
open Cli.View.Actions
open Cli.View.Scenes.BandCreator
open Cli.View.Scenes.CharacterCreator
open Cli.View.Scenes.MainMenu
open Cli.View.Scenes.RehearsalRoom.Root
open Cli.View.Scenes.Management.Root
open Cli.View.TextConstants
open Cli.View.Renderer
open Storage

/// Returns the sequence of actions associated with a screen given its name.
let actionsFrom scene =
  match scene with
  | MainMenu savegameState -> mainMenu savegameState
  | CharacterCreator -> characterCreator ()
  | BandCreator character -> bandCreator character
  | RehearsalRoom -> rehearsalRoomScene ()
  | Management -> managementScene ()

/// Saves the game to the savegame file only if the screen is not the main menu,
/// character creator or band creator, which still have unreliable data or
/// might not have data at all.
let saveIfNeeded scene =
  match scene with
  | MainMenu _ -> ()
  | CharacterCreator _ -> ()
  | BandCreator _ -> ()
  | _ -> Savegame.save ()

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith chain =
  chain
  |> Seq.iter
       (fun action ->
         match action with
         | Prompt prompt ->
             renderPrompt prompt
             |> fun input ->
                  match prompt.Content with
                  | ChoicePrompt content ->
                      match content with
                      | MandatoryChoiceHandler content ->
                          content.Choices
                          |> choiceById input
                          |> content.Handler
                      | OptionalChoiceHandler content ->
                          match input with
                          | "back" -> content.Handler Back
                          | _ ->
                              content.Choices
                              |> choiceById input
                              |> Choice
                              |> content.Handler
                  | ConfirmationPrompt handler ->
                      handler (input |> Convert.ToBoolean)
                  | NumberPrompt handler -> handler (input |> int)
                  | TextPrompt handler -> handler input
                  |> runWith
         | Message message -> renderMessage message
         | Figlet text -> renderFiglet text
         | ProgressBar content -> renderProgressBar content
         | Scene scene ->
             saveIfNeeded scene
             separator ()
             runWith (actionsFrom scene)
         | SceneAfterKey scene ->
             waitForInput
             <| TextConstant CommonPressKeyToContinue

             clear ()
             runWith (actionsFrom scene)
         | NoOp -> ())
