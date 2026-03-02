module Duets.UI.Scenes.InGame.RehearsalRoom.ComposeSong

open Duets.Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Duets.Common
open Duets.Entities
open Duets.Entities.Time
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation.Songs.Composition.ComposeSong
open Duets.UI
open Duets.UI.Components.Choice
open Duets.UI.Components.Layout
open Duets.UI.Components.MultiProgressBar
open Duets.UI.Components.Picker
open Duets.UI.Hooks.Effect
open Duets.UI.Hooks.Scene
open Duets.UI.Hooks.ViewStack
open Duets.UI.SceneIndex
open Duets.UI.Types

let private compositionProgress song =
    Component.create (
        "Rehearsal-ComposeSong-CompositionProgress",
        fun ctx ->
            let applyEffects = ctx.useEffectRunner ()
            let switchTo = ctx.useSceneSwitcher ()

            let onProgressFinish _ =
                composeSong (State.get ()) song |> applyEffects

                Scene.InGame |> switchTo

            vertical [
                view
                    {
                        Steps =
                            [
                                "Brainstorming..."
                                "Configuring reverb..."
                                "Playing foosball..."
                            ]
                        StepDuration = 1<second>
                        OnFinish = onProgressFinish
                    }
            ]
    )

let view =
    Component.create (
        "Rehearsal-ComposeSong",
        fun ctx ->
            let viewStack = ctx.useViewStack ()

            let name = ctx.useState ""

            let length = Length.from 3<minute> 45<second> |> ctx.useState

            let vocalStyle = ctx.useState VocalStyle.Normal

            let lengthChanged l = Length.parse l |> length.Set

            let createEnabled =
                Song.validateName name.Current
                |> Result.mapError (fun _ -> ())
                |> Result.andThen (
                    match length.Current with
                    | Ok l ->
                        Song.validateLength l |> Result.mapError (fun _ -> ())
                    | _ -> Error()
                )
                |> Result.isOk

            let onCreate _ =
                let songLength =
                    length.Current
                    |> Result.unwrap (* Validated in createEnabled. *)

                let song =
                    Song.from name.Current songLength vocalStyle.Current

                [ compositionProgress song :> IView ]
                |> Subcomponent
                |> viewStack.AddAction

            vertical [
                TextBlock.create [ TextBlock.text "Song's name:" ]
                TextBox.create [
                    TextBox.watermark "Song's name"
                    TextBox.onTextChanged name.Set
                ]

                TextBlock.create [
                    TextBlock.text "Song's length (format: mm:ss, as in 5:45)"
                ]
                TextBox.create [
                    TextBox.watermark "Song's length (mm:ss)"
                    TextBox.onTextChanged lengthChanged
                ]

                TextBlock.create [ TextBlock.text "Song's vocal style:" ]
                create
                    {
                        Selected = vocalStyle
                        ToText = Text.Music.vocalStyleName
                        Values = Duets.Data.VocalStyles.all
                    }

                createCancellable
                    {
                        ActionLabel = "Create"
                        ActionEnabled = createEnabled
                        OnAction = onCreate
                        OnCancel = (fun () -> ())
                    }
            ]
    )
