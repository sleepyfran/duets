module UI.Components.Picker

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open UI

let view (selectedValue: IWritable<'a>) (optionTextFn: 'a -> string) values =
    Component.create (
        "Picker",
        fun ctx ->
            let value = ctx.usePassed selectedValue

            WrapPanel.create [
                WrapPanel.orientation Orientation.Horizontal
                values
                |> List.map (fun item ->
                    Button.create [
                        Button.margin (
                            0,
                            0,
                            Theme.Padding.small,
                            Theme.Padding.small
                        )
                        item |> optionTextFn |> Button.content
                        if value.Current = item then
                            [ "selected" ]
                        else
                            []
                        |> Button.classes
                        Button.onClick (fun _ -> value.Set item)
                    ]
                    :> IView)
                |> StackPanel.children
            ]
    )
