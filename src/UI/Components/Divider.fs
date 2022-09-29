module UI.Components.Divider

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open UI

let horizontal =
    Border.create [
        Border.borderBrush Theme.Brush.bg
        Border.borderThickness (0, 0, Theme.Layout.borderThickness, 0)
        Border.height 25
    ]

let vertical =
    Border.create [
        Border.borderBrush Theme.Brush.bg
        Border.borderThickness (0, 0, 0, Theme.Layout.borderThickness)
        Border.height 3
    ]
