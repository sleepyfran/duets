module Cli.View.Actions

open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Storage.Savegame

/// Defines the index of all scenes available in the game that can be instantiated.
type Scene =
  | MainMenu of SavegameState
  | CharacterCreator
  // Band creator needs a character the character that was created in the
  // previous step.
  | BandCreator of Character
  | RehearsalRoom
  | Management

/// Encapsulates text that can either be defined by a text constant, which is
/// resolved by the UI layer, or a string constant that is just passed from this
/// layer into the UI.
type Text =
  | TextConstant of TextConstant
  | Literal of string

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
  | Message of Text
  | Figlet of Text
  | Prompt of Prompt
  | ProgressBar of ProgressBarContent
  | Scene of Scene
  | NoOp

/// Sequence of actions to be executed.
and ActionChain = Action seq

/// Indicates the need to prompt the user for information.
and Prompt = { Title: Text; Content: PromptContent }

/// Specified the different types of prompts available.
and PromptContent =
  | ChoicePrompt of ChoicePrompt * PromptHandler<Choice>
  | ConfirmationPrompt of PromptHandler<bool>
  | NumberPrompt of PromptHandler<int>
  | TextPrompt of PromptHandler<string>

/// Defines a handler that takes whatever result the prompt is giving out and
/// returns another chain of actions.
and PromptHandler<'a> = 'a -> ActionChain

/// Defines a list of choices that the user can select.
and ChoicePrompt = Choice list

and Choice = { Id: string; Text: Text }

/// Defines the content of a progress bar by giving the number of steps and
/// the duration of each step. If Async is set to true the steps will be shown
/// randomly at the same time advancing at the same pace with a different
/// beat.
and ProgressBarContent =
  { StepNames: Text list
    StepDuration: int<second>
    Async: bool }

/// Returns a possible choice from a set of choices given its ID.
let choiceById id = List.find (fun c -> c.Id = id)
