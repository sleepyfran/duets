module Entities.Identity

open System

/// Creates a new identity GUID.
let create = Guid.NewGuid
