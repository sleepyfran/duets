namespace Simulation.Queries

module Characters =
    open Aether
    open Aether.Operators
    open Common
    open Entities

    /// Returns a character given its ID.
    let byId state id =
        state |> Optic.get Lenses.State.characters_ |> Map.find id

    /// Returns the character that the player is playing with.
    let playableCharacter state =
        let playableCharacterId =
            state |> Optic.get Lenses.State.playableCharacter_

        byId state playableCharacterId

    /// Returns the value of an attribute from the playable character.
    let playableCharacterAttribute state attribute =
        playableCharacter state
        |> Optic.get (Lenses.Character.attribute_ attribute)
        |> Option.defaultValue 0

    /// Returns a list with all the character's attributes.
    let allPlayableCharacterAttributes state =
        [
            CharacterAttribute.Health
            CharacterAttribute.Energy
            CharacterAttribute.Mood
            CharacterAttribute.Drunkenness
            CharacterAttribute.Fame
        ]
        |> List.map (fun attr -> (attr, playableCharacterAttribute state attr))

    /// Returns a tuple of three values with the value of the given attributes
    /// from the playable character.
    let playableCharacterAttribute3 state attr1 attr2 attr3 =
        (playableCharacterAttribute state attr1,
         playableCharacterAttribute state attr2,
         playableCharacterAttribute state attr3)

    /// Returns a tuple of four values with the value of the given attributes
    /// from the playable character.
    let playableCharacterAttribute4 state attr1 attr2 attr3 attr4 =
        (playableCharacterAttribute state attr1,
         playableCharacterAttribute state attr2,
         playableCharacterAttribute state attr3,
         playableCharacterAttribute state attr4)

    /// Returns a character given its ID. Throws an exception if the key is not
    /// found.
    let find state id =
        let lens = Lenses.State.characters_ >-> Map.key_ id

        Optic.get lens state |> Option.get

    /// Returns the age of the character.
    let ageOf state character =
        let today = Calendar.today state

        let birthday = Optic.get Lenses.Character.birthday_ character

        Calendar.Query.yearsBetween birthday today
