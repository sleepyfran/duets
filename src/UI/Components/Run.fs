module UI.Components.Run

open Avalonia.Controls.Documents
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

type Run with

    /// Creates a run with the specified message and the success styles pre-applied.
    static member success(message: string) : IView =
        Run.create [ Run.text message; Run.classes [ "success" ] ]
