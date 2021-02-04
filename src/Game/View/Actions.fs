module View.Actions

open Entities.State

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
  | Message of string
  | Prompt of Prompt
  | Effect of (State -> State)
  | NoOp

/// Sequence of actions to be executed.
and ActionChain = Action seq

/// Indicates the need to prompt the user for information.
and Prompt =
  { Title: string
    Content: PromptContent }

/// Specified the different types of prompts available.
and PromptContent =
  | TextPrompt of PromptHandler<string>
  | ChoicePrompt of ChoicePrompt * PromptHandler<Choice>

/// Defines a handler that takes whatever result the prompt is giving out and
/// returns another chain of actions.
and PromptHandler<'a> = 'a -> ActionChain

/// Defines a list of choices that the user can select.
and ChoicePrompt = Choice seq

and Choice = { Id: string; Text: string }
