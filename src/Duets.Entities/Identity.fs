module Duets.Entities.Identity

open System

/// Creates a new identity GUID.
let create = Guid.NewGuid

/// Attempts to parse the given Identity GUID.
let from (str: string) = Guid.Parse str

module Reproducible =
    /// Creates a Base64 encoded based on the given input. This is useful for
    /// IDs that need to be reproducible, like the ones in Places or Zones that
    /// are based on the name of the place or zone.
    let create (input: string) =
        input |> Text.Encoding.ASCII.GetBytes |> Convert.ToBase64String
