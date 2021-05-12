namespace Simulation.Queries

module Characters =
    open Aether
    open Entities

    /// Returns the character that the player is playing with.
    let playableCharacter state =
        state |> Optic.get Lenses.State.character_
