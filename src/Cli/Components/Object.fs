[<AutoOpen>]
module Cli.Components.Object

open Cli.Components.Commands
open Entities

/// Defines an object that can be placed in an interactive room so that the user
/// can interact with it.
type Object =
    { Type: ObjectType
      Commands: Command list }
