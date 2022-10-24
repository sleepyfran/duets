module rec UI.Components.Choice

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open UI

type ChoiceInput<'a> = {
    /// ID that the component will have associated. This is needed for FuncUI
    /// to detect that we're creating new choices:
    /// https://funcui.avaloniaui.net/components/component-lifetime#component-identity-key
    Id: string
    /// Function to be called when an option is selected.
    OnSelected: 'a -> unit
    /// Function to call when we want to display an option.
    ToText: 'a -> string
    /// List of values that will be shown.
    Values: 'a list
}

let create input =
    Component.create (
        $"Choice-{input.Id}",
        fun ctx ->
            let selectedValue =
                ctx.useState (None: 'a Option)

            let isValueSelected item =
                match selectedValue.Current with
                | Some value when value = item -> true
                | _ -> false

            WrapPanel.create [
                WrapPanel.orientation Orientation.Horizontal
                createChoices input selectedValue isValueSelected
                |> WrapPanel.children
            ]
    )

let private createChoices input selectedValue isValueSelected =
    let selectValue item =
        if selectedValue.Current.IsNone then
            item |> Some |> selectedValue.Set
            input.OnSelected item
        else
            () (* We don't want to notify multiple times the same value. *)

    input.Values
    |> List.map (fun item ->
        Button.create [
            Button.margin (0, 0, Theme.Padding.small, Theme.Padding.small)
            item |> input.ToText |> Button.content

            if isValueSelected item then
                [ "selected" ]
            else
                []
            |> Button.classes

            Button.isEnabled (
                selectedValue.Current.IsNone
                || isValueSelected item
            )

            Button.onClick (fun _ -> selectValue item)
        ]
        :> IView)
