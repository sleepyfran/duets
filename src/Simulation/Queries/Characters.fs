namespace Simulation.Queries

module Characters =
    open Aether
    open Aether.Operators
    open Common
    open Entities

    /// Returns the character that the player is playing with.
    let playableCharacter state =
        let playableCharacterId =
            state |> Optic.get Lenses.State.playableCharacter_

        state
        |> Optic.get Lenses.State.characters_
        |> Map.find playableCharacterId

    /// Returns a character given its ID. Throws an exception if the key is not
    /// found.
    let find state id =
        let lens =
            Lenses.State.characters_ >-> Map.key_ id

        Optic.get lens state |> Option.get
