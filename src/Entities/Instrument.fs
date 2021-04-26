module Entities.Instrument

/// Creates an instrument given its type.
let createInstrument instrumentType =
  { Id = InstrumentId <| Identity.create ()
    Type = instrumentType }
