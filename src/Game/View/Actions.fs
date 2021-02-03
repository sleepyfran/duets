module View.Action

open Entities.State

/// A choice to be made by the user from a pre-selected list of choices.
type Choice = {
    Id: string
    Text: string
}

/// Specifies the different type of prompts available according to the data
/// they return.
type PromptContent =
    | TextPrompt
    | ChoicePrompt of Choice[]

/// Indicates the need to prompt the user for some information.
type Prompt = {
    Title: string
    Content: PromptContent
}

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
    | Message of string
    | Prompt of Prompt
    | NoOp

/// Represents a chain of actions to be processed. Each chain has the current
/// action being processed, as well as the effects that need to be applied
/// in that step (if any), and a way to compute the next chain of actions.
type ActionChain = {
    Current: Action
    Effects: (State -> State)[]
    Next: State -> ActionChain option
}
