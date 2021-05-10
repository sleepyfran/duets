module Entities.Effect

/// Creates a sequence of just one given effect.
let single (effect: Effect) = seq { yield effect }

/// Creates a sequence of a couple of effects.
let two (first: Effect) (second: Effect) =
  seq {
    yield first
    yield second
  }

/// Creates an empty sequence of effects.
let empty = Seq.empty
