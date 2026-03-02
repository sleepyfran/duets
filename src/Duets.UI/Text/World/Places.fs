module Duets.UI.Text.World.Places

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Duets.Entities

/// Describes the given place in text.
let text (place: Place) : IView =
    let description =
        match place.PlaceType with
        | RehearsalSpace _ ->
            "You're in the rehearsal room, the previous band left all their empty beers and a bunch of cigarettes on the floor. Your band's morale has decreased a bit."
        | _ -> ""

    TextBlock.create [ TextBlock.text description ] :> IView
