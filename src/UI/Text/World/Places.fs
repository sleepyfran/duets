module UI.Text.World.Places

open Avalonia.Controls.Documents
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Entities

/// Describes the given place in text.
let text (place: Place) : IView =
    Span.create [
        Span.inlines [
            match place.Type with
            | RehearsalSpace _ ->
                Run.create [
                    Run.text
                        "You’re in the rehearsal room, the previous band left all their empty beers and a bunch of cigarettes on the floor. Your band’s morale has decreased a bit."
                ]
                :> IView
            | _ -> Run.create []
        ]
    ]
