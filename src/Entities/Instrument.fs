module Entities.Instrument

open Entities.Identity

type InstrumentId = InstrumentId of Identity

/// Defines what kind of instrument we're defining to be able to query different
/// information about it.
type Type =
  | Guitar
  | Drums
  | Bass
  | Vocals

/// Represents the archetype instrument that a character can use.
type Instrument = { Id: InstrumentId; Type: Type }

let createInstrument instrumentType =
  { Id = InstrumentId(create ())
    Type = instrumentType }
