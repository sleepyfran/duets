module Cli.View.Scenes.MainMenu

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

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
              Text = TextConstant MainMenuNewGame }

        if hasSavegameAvailable then
            yield
                { Id = "load_game"
                  Text = TextConstant MainMenuLoadGame }
    }
    |> List.ofSeq

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu savegameState =
    seq {
        yield Figlet <| TextConstant GameName
        yield GameInfo version

        if savegameState = Savegame.Incompatible then
            yield
                Message
                <| TextConstant MainMenuIncompatibleSavegame

        yield! showMenu savegameState
    }

and showMenu savegameState =
    seq {
        yield
            Prompt
                { Title = TextConstant MainMenuPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices =
                                menuOptions (savegameState = Savegame.Available)
                            Handler =
                                basicOptionalChoiceHandler NoOp processSelection
                            BackText = TextConstant MainMenuExit } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "new_game" -> yield (Scene CharacterCreator)
        | "load_game" -> yield (Scene World)
        | _ -> yield NoOp
    }
