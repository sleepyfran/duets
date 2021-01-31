module State

/// Defines the global state of the game.
type State = {
  Test: string
}

/// Returns an empty game state. Base case for when there's no savegame
/// available and the user wants to start from the beginning.
let empty () =
  {
    Test = "test"
  }