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
    | Keyboard -> [ Hands ]
    | Synthesizer -> [ Hands ]
    | UprightBass -> [ Hands ]
    | Saxophone -> [ Hands; Mouth ]
    | Trumpet -> [ Hands; Mouth ]
    | Trombone -> [ Hands; Mouth ]
    | Violin -> [ Hands ]
    | Cello -> [ Hands ]
    | Banjo -> [ Hands ]
    | Harmonica -> [ Mouth ]
    | Turntables -> [ Hands ]
    | HandPercussion -> [ Hands ]

/// Returns the family of the given instrument type.
let family instrumentType =
    match instrumentType with
    | Guitar -> Strings
    | Drums -> Percussion
    | Bass -> Strings
    | Vocals -> Voice
    | Keyboard -> Keys
    | Synthesizer -> Electronic
    | UprightBass -> Strings
    | Saxophone -> Winds
    | Trumpet -> Winds
    | Trombone -> Winds
    | Violin -> Strings
    | Cello -> Strings
    | Banjo -> Strings
    | Harmonica -> Winds
    | Turntables -> Electronic
    | HandPercussion -> Percussion

/// Returns whether one person can play all the given instrument types at once.
let compatible instrumentTypes =
    let allLimbs = instrumentTypes |> List.collect limbs
    allLimbs |> List.distinct |> List.length = allLimbs.Length
