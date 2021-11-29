namespace Entities

[<AutoOpen>]
module WorldTypes =
    /// Defines all the different objects that can appear in the game world.
    type ObjectType = Instrument of InstrumentType
