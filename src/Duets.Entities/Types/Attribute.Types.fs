namespace Duets.Entities

[<AutoOpen>]
module AttributeTypes =
    /// Identifier of an attribute of a character.
    [<RequireQualifiedAccess>]
    type CharacterAttribute =
        | Drunkenness
        | Energy
        | Fame
        | Health
        | Hunger
        | Mood

    /// Wraps an int to define the amount of an attribute. These are always
    /// between 0 and 100.
    type CharacterAttributeAmount = int

    /// Gathers all the different needs and statuses of the character that
    /// increase and decrease with the normal flow of the game. All these attributes
    /// are represented from 0 to 100 and the absence of an attribute in the
    /// map indicates that the value is 0.
    type CharacterAttributes = Map<CharacterAttribute, CharacterAttributeAmount>
