module Duets.UI.Components.Divider

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Duets.UI.Theme

let horizontal =
    Border.create [
        Border.borderBrush Brush.bg
        Border.borderThickness (Thickness(0, 0, Layout.borderThickness, 0))
        Border.height 25
    ]

let vertical =
    Border.create [
        Border.borderBrush Brush.bg
        Border.borderThickness (Thickness(0, 0, 0, Layout.borderThickness))
        Border.height 3
    ]
