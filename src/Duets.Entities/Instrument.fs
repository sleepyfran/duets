module Duets.Entities.Instrument

/// Creates an instrument given its type.
let createInstrument instrumentType =
    { Id = InstrumentId <| Identity.create ()
      Type = instrumentType }

/// Returns the limbs needed to play the given instrument type.
let limbs instrumentType =
    match instrumentType with
    | Guitar -> [ Hands ]
    | Drums -> [ Hands; Feet ]
    | Bass -> [ Hands ]
    | Vocals -> [ Mouth ]

/// Returns whether one person can play all the given instrument types at once.
let compatible instrumentTypes =
    let allLimbs = instrumentTypes |> List.collect limbs
    allLimbs |> List.distinct |> List.length = allLimbs.Length

module Type =
    /// Creates an instrument type given its type as a string. Defaults to vocals if
    /// an invalid string is given.
    let from str =
        match str with
        | "Guitar" -> Guitar
        | "Drums" -> Drums
        | "Bass" -> Bass
        | _ -> Vocals
