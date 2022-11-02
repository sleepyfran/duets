module UI.Components.Layout

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout

/// Creates the default layout for an in game scene.
let vertical children =
    StackPanel.create [
        StackPanel.spacing 10
        StackPanel.children children
    ]

/// Creates the default horizontal layout for an in game scene.
let horizontal children =
    StackPanel.create [
        StackPanel.spacing 10
        StackPanel.orientation Orientation.Horizontal
        StackPanel.children children
    ]
