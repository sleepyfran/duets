module UI.Components.Choice

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open UI

type ChoiceInput<'a> = {
    OnSelected: 'a -> unit
    ToText: 'a -> string
    Values: 'a list
}

let create input =
    Component.create (
        "Choice",
        fun ctx ->
            let selectedValue = ctx.useState (None: 'a Option)

            let selectValue item =
                if selectedValue.Current.IsNone then
                    item |> Some |> selectedValue.Set
                    input.OnSelected item
                else
                    () (* We don't want to notify multiple times the same value. *)

            let isValueSelected item =
                match selectedValue.Current with
                | Some value when value = item -> true
                | _ -> false

            WrapPanel.create [
                WrapPanel.orientation Orientation.Horizontal
                input.Values
                |> List.map (fun item ->
                    Button.create [
                        Button.margin (
                            0,
                            0,
                            Theme.Padding.small,
                            Theme.Padding.small
                        )
                        item |> input.ToText |> Button.content

                        if isValueSelected item then [ "selected" ] else []
                        |> Button.classes

                        Button.isEnabled (
                            selectedValue.Current.IsNone || isValueSelected item
                        )

                        Button.onClick (fun _ -> selectValue item)
                    ]
                    :> IView)
                |> StackPanel.children
            ]
    )
