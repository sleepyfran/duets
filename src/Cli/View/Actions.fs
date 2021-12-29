module Cli.View.Actions

open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
    | Separator
    | Message of Text
    | Figlet of Text
    | Prompt of Prompt
    | ProgressBar of ProgressBarContent
    | Scene of Scene
    | GameInfo of version: string
    | Effect of Effect
    | NewLine
    | ClearScreen
    | Exit
    | NoOp

/// Sequence of actions to be executed.
and ActionChain = Action seq

/// Defines the index of all scenes available in the game that can be instantiated.
and Scene =
    | MainMenu of Savegame.SavegameState
    | CharacterCreator
    // Band creator needs a character the character that was created in the
    // previous step.
    | BandCreator of Character
    | World
    | Management
    | Bank
    | Statistics
    | Phone

/// Defines an object that can be placed in an interactive room so that the user
/// can interact with it.
and Object =
    { Type: ObjectType
      Commands: Command list }

/// Defines a space in which there are multiple rooms connected via a graph. This
/// action takes care of the navigation inside of the space and relies on the
/// properties passed to gather the description, objects and extra commands of
/// the current room.
and InteractiveSpace<'a> =
    { Rooms: Graph<'a>
      GetDescription: 'a -> Text
      GetObjects: 'a -> Object list
      GetExtraCommands: 'a -> Command list }

/// Defines the content of a progress bar by giving the number of steps and
/// the duration of each step. If Async is set to true the steps will be shown
/// randomly at the same time advancing at the same pace with a different
/// beat.
and ProgressBarContent =
    { StepNames: Text list
      StepDuration: int<second>
      Async: bool }

/// Indicates the need to prompt the user for information.
and Prompt = { Title: Text; Content: PromptContent }

/// Specified the different types of prompts available.
and PromptContent =
    | ChoicePrompt of ChoicePrompt
    | MultiChoicePrompt of MultiChoiceHandler
    | ConfirmationPrompt of PromptHandler<bool>
    | NumberPrompt of PromptHandler<int>
    | TextPrompt of PromptHandler<string>
    | LengthPrompt of PromptHandler<Length>
    | CommandPrompt of CommandPrompt

/// Defines a handler that takes whatever result the prompt is giving out and
/// returns another chain of actions.
and PromptHandler<'a> = 'a -> ActionChain

and Choice = { Id: string; Text: Text }

and OptionalChoice =
    | Choice of Choice
    | Back

/// Represents choices that cannot be skipped.
and MandatoryChoiceHandler =
    { Choices: Choice list
      Handler: PromptHandler<Choice> }

/// Represents choices that can be skipped with a back or cancel button.
and OptionalChoiceHandler =
    { Choices: Choice list
      Handler: PromptHandler<OptionalChoice>
      BackText: Text }

/// Represents choices that cannot be skipped and allows multiple selections.
and MultiChoiceHandler =
    { Choices: Choice list
      Handler: PromptHandler<Choice list> }

and ChoicePrompt =
    | MandatoryChoiceHandler of MandatoryChoiceHandler
    | OptionalChoiceHandler of OptionalChoiceHandler

/// Separates the two types of command handlers that we can have:
/// - `HandlerWithNavigation` represents a handler that, when executed, eventually
///   moves the user into another scene, sub-scene or exist the game and
///   effectively continues an action chain and therefore does not need the
///   command prompt to show after the chain is finished.
/// - `HandlerWithoutNavigation` represents a handler that does need the command
///   prompt to show again after because it just shows information or executes
///   an effect but does not have any chain to continue afterwards and, without
///   showing the prompt, the game would exit.
and CommandPromptHandler =
    | HandlerWithNavigation of PromptHandler<string list>
    | HandlerWithoutNavigation of PromptHandler<string list>

and Command =
    { Name: string
      Description: Text
      Handler: CommandPromptHandler }

and CommandPrompt = Command list

/// Returns a possible choice from a set of choices given its ID.
let choiceById id = List.find (fun c -> c.Id = id)

/// Returns all the choices the user made from a list of choices.
let choicesById ids =
    List.filter (fun c -> List.contains c.Id ids)
