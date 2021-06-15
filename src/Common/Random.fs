module Common.Random

let private seededRandom = System.Random()

/// Returns a random non-negative number.
let random () = seededRandom.Next()

/// Returns a random number between the inclusive range of min and max.
let between min max = seededRandom.Next(min, max)

/// Returns a random non-negative float between min and max.
let floatBetween min max =
    seededRandom.NextDouble() * (max - min) + min
