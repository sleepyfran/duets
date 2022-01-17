module Cli.View.Scenes.MainMenu

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text

/// Current version of the game as loaded from the fsproj.
let version =
    System
        .Reflection
        .Assembly
        .GetEntryAssembly()
        .GetName()
        .Version.ToString()

let menuOptions hasSavegameAvailable =
    seq {
        yield
            { Id = "new_game"
              Text = I18n.translate (MainMenuText MainMenuNewGame) }

        if hasSavegameAvailable then
            yield
                { Id = "load_game"
                  Text = I18n.translate (MainMenuText MainMenuLoadGame) }
    }
    |> List.ofSeq

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu savegameState =
    seq {
        yield I18n.translate (CommonText GameName) |> Figlet
        yield GameInfo version

        if savegameState = Savegame.Incompatible then
            yield
                I18n.translate (MainMenuText MainMenuIncompatibleSavegame)
                |> Message

        yield! showMenu savegameState
    }

and showMenu savegameState =
    seq {
        yield
            Prompt
                { Title = I18n.translate (MainMenuText MainMenuPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices =
                                menuOptions (savegameState = Savegame.Available)
                            Handler =
                                basicOptionalChoiceHandler NoOp processSelection
                            BackText =
                                I18n.translate (MainMenuText MainMenuExit) } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "new_game" -> yield (Scene CharacterCreator)
        | "load_game" -> yield (Scene World)
        | _ -> yield NoOp
    }
