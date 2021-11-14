module Cli.View.Scenes.Interactive.Objects

open Cli.View.Actions
open Entities

/// Creates a bass with the normal associated commands of the instrument.
/// Executes the given actions once the guitar is called.
let bass description actions =
    { Type = ObjectType.Instrument Bass
      Commands =
          [ { Name = "play"
              Description = description
              Handler = HandlerWithoutNavigation(fun _ -> actions) } ] }

/// Creates drums with the normal associated commands of the instrument.
/// Executes the given actions once the guitar is called.
let drums description actions =
    { Type = ObjectType.Instrument Drums
      Commands =
          [ { Name = "play"
              Description = description
              Handler = HandlerWithoutNavigation(fun _ -> actions) } ] }

/// Creates a guitar with the normal associated commands of the instrument.
/// Executes the given actions once the guitar is called.
let guitar description actions =
    { Type = ObjectType.Instrument Guitar
      Commands =
          [ { Name = "play"
              Description = description
              Handler = HandlerWithoutNavigation(fun _ -> actions) } ] }

/// Creates a microphone with the normal associated commands of the instrument.
/// Executes the given actions once the guitar is called.
let microphone description actions =
    { Type = ObjectType.Instrument Vocals
      Commands =
          [ { Name = "sing"
              Description = description
              Handler = HandlerWithoutNavigation(fun _ -> actions) } ] }
