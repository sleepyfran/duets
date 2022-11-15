module UI.Components.Choice

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open UI

type ChoiceItem = {
    Text: string
    Classes: string list
    Enabled: bool
}

type ChoiceInput<'a> = {
    /// ID that the component will have associated. This is needed for FuncUI
    /// to detect that we're creating new choices:
    /// https://funcui.avaloniaui.net/components/component-lifetime#component-identity-key
    Id: string
    /// Function to be called when an option is selected.
    OnSelected: 'a -> unit
    /// Function to call when we want to display an option.
    ChoiceContent: 'a -> ChoiceItem
    /// List of values that will be shown.
    Values: 'a list
}

let private createChoices
    input
    (selectedValue: IWritable<'a option>)
    isValueSelected
    =
    let selectValue item =
        if selectedValue.Current.IsNone then
            item |> Some |> selectedValue.Set
            input.OnSelected item
        else
            () (* We don't want to notify multiple times the same value. *)

    input.Values
    |> List.map (fun item ->
        let content = item |> input.ChoiceContent

        Button.create [
            Button.margin (0, 0, Theme.Padding.small, Theme.Padding.small)
            Button.content content.Text

            if isValueSelected item then
                [ "selected" ] @ content.Classes
            else
                content.Classes
            |> Button.classes

            Button.isEnabled (
                content.Enabled
                && (selectedValue.Current.IsNone || isValueSelected item)
            )

            Button.onClick (fun _ -> selectValue item)
        ]
        :> IView)

/// Creates a choice given its options.
let create input =
    Component.create (
        $"Choice-{input.Id}",
        fun ctx ->
            let selectedValue = ctx.useState (None: 'a Option)

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

type CancellableChoice =
    | Action
    | Cancel

type CancellableInput = {
    ActionLabel: string
    ActionEnabled: bool
    OnAction: unit -> unit
    OnCancel: unit -> unit
}

/// Creates a wrapper around a choice that contains only an action and a cancel
/// option.
let createCancellable input =
    create
        {
            Id = $"Cancellable-{input.ActionLabel}-{input.ActionEnabled}"
            OnSelected =
                (function
                | Action -> input.OnAction()
                | Cancel -> input.OnCancel())
            ChoiceContent =
                (function
                | Action -> {
                    Text = input.ActionLabel
                    Classes = []
                    Enabled = input.ActionEnabled
                  }
                | Cancel -> {
                    Text = "Cancel"
                    Classes = [ "destructive" ]
                    Enabled = true
                  })
            Values = [ Action; Cancel ]
        }
