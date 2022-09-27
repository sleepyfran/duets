module UI.Components.Divider

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open UI

let view =
    Border.create [
        Border.borderBrush Theme.Brush.bg
        Border.borderThickness (0, 0, 0, Theme.Layout.borderThickness)
        Border.height 3
    ]
