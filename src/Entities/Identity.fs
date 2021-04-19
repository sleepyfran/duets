module Entities.Identity

open System

/// Defines the type that all entities with an ID should use.
type Identity = Guid

/// Creates a new identity GUID.
let create = Guid.NewGuid
