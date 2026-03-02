module Duets.UI.Components.Run

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Media

/// Creates a text view with the success color applied.
let success (message: string) : IView =
    TextBlock.create [
        TextBlock.text message
        TextBlock.foreground (SolidColorBrush(Color.Parse("#2FAA65")))
    ]
    :> IView

/// Creates a plain text view.
let createText (message: string) : IView =
    TextBlock.create [ TextBlock.text message ] :> IView
