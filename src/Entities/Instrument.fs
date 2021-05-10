module Entities.Instrument

/// Creates an instrument given its type.
let createInstrument instrumentType =
    { Id = InstrumentId <| Identity.create ()
      Type = instrumentType }

module Type =
    /// Creates an instrument type given its type as a string. Defaults to vocals if
    /// an invalid string is given.
    let from str =
        match str with
        | "Guitar" -> Guitar
        | "Drums" -> Drums
        | "Bass" -> Bass
        | _ -> Vocals
