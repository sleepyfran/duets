module Cli.View.Actions

open Mediator.Mutations.Setup
open Cli.View.TextConstants

/// Defines the index of all scenes available in the game that can be instantiated.
type Scene =
  | MainMenu
  | CharacterCreator
  // Band creator needs a character the character that was created in the
  // previous step.
  | BandCreator of CharacterInput
  | RehearsalRoom

/// Encapsulates text that can either be defined by a text constant, which is
/// resolved by the UI layer, or a string constant that is just passed from this
/// layer into the UI.
type Text =
  | TextConstant of TextConstant
  | String of string

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
  | Message of Text
  | Prompt of Prompt
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

/// Returns a possible choice from a set of choices given its ID.
let choiceById id = List.find (fun c -> c.Id = id)
