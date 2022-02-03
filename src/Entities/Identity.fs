module Entities.Identity

open System

/// Creates a new identity GUID.
let create = Guid.NewGuid

/// Attempts to parse the given Identity GUID.
let from (str: string) = Guid.Parse str
