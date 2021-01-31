module Startup

open Action
open State

/// Given a savegame file, returns the current state of the game as well as the
/// the corresponding screen.
let fromSavegame savegame  =
    match savegame with
    | Some(_) -> (empty (), {
        Current = Message "Just a test"
        Effects = Array.empty
        Next = fun _ -> None
    })
    | None -> (empty (), {
        Current = Prompt {
            Title = "What?"
            Content = TextPrompt
        }
        Effects = Array.empty
        Next = fun _ -> None
    })
