[<RequireQualifiedAccess>]
module Duets.Cli.Text.MainMenu

let incompatibleSavegame =
    Styles.error "Your savegame is incompatible or malformed and was ignored"

let prompt = "Select an option to begin"
let newGame = "New game"
let loadGame = "Load game"
let exit = Styles.faded "Exit"

let savegameNotAvailable =
    Styles.error "No savegame available. Create a new game"

let newGameReplacePrompt =
    Styles.danger
        "Creating a new game will replace your current savegame and all the progress will be lost, are you sure?"
