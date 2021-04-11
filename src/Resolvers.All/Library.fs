module Resolvers.All

open Resolvers

/// Initializes all resolvers of the game in one place.
let init () =
  Core.Setup.init ()
  Storage.Setup.init ()
