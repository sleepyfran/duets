module Duets.UI.Components.Picker

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Duets.UI.Theme

type PickerInput<'a> = {
    Selected: IWritable<'a>
    ToText: 'a -> string
    Values: 'a list
}

let create input =
    Component.create (
        "Picker",
        fun ctx ->
            let value = ctx.usePassed input.Selected

            WrapPanel.create [
                WrapPanel.orientation Orientation.Horizontal
                input.Values
                |> List.map (fun item ->
                    Button.create [
                        Button.margin (
                            0,
                            0,
                            Padding.small,
                            Padding.small
                        )
                        item |> input.ToText |> Button.content
                        if value.Current = item then [ "selected" ] else []
                        |> Button.classes
                        Button.onClick (fun _ -> value.Set item)
                    ]
                    :> IView)
                |> WrapPanel.children
            ]
    )
