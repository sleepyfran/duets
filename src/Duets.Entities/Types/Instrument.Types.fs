namespace Duets.Entities

[<AutoOpen>]
module InstrumentTypes =
    /// Unique identifier of an instrument.
    type InstrumentId = InstrumentId of Identity

    /// Defines what kind of instrument we're defining to be able to query different
    /// information about it.
    type InstrumentType =
        | Guitar
        | Drums
        | Bass
        | Vocals

    /// Defines the parts of the body that an instrument needs while playing it.
    type Limb =
        | Hands
        | Feet
        | Mouth

    /// Represents the archetype instrument that a character can use.
    type Instrument =
        { Id: InstrumentId
          Type: InstrumentType }
