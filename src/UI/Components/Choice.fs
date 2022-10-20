module rec UI.Components.Choice

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open UI

type ChoiceValues<'a> =
    | Simple of 'a list
    | Sectioned of 'a list list

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
    Values: ChoiceValues<'a>
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

            match input.Values with
            | Simple items ->
                createSimpleChoice input items selectedValue isValueSelected
            | Sectioned sections ->
                createSectionedChoice
                    input
                    sections
                    selectedValue
                    isValueSelected
    )

let createSimpleChoice input items selectedValue isValueSelected =
    WrapPanel.create [
        WrapPanel.orientation Orientation.Horizontal
        createChoices input items selectedValue isValueSelected
        |> WrapPanel.children
    ]

let createSectionedChoice input sections selectedValue isValueSelected =
    StackPanel.create [
        StackPanel.orientation Orientation.Vertical
        sections
        |> List.map (fun items ->
            createSimpleChoice input items selectedValue isValueSelected)
        |> StackPanel.children
    ]

let private createChoices input items selectedValue isValueSelected =
    let selectValue item =
        if selectedValue.Current.IsNone then
            item |> Some |> selectedValue.Set
            input.OnSelected item
        else
            () (* We don't want to notify multiple times the same value. *)

    items
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
