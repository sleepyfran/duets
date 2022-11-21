module UI.Scenes.InGame.RehearsalRoom.ComposeSong

open Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Common
open Entities
open Entities.Time
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong
open UI
open UI.Components
open UI.Hooks.Effect
open UI.Hooks.Scene
open UI.Hooks.ViewStack
open UI.SceneIndex
open UI.Types

let private compositionProgress song =
    Component.create (
        "Rehearsal-ComposeSong-CompositionProgress",
        fun ctx ->
            let applyEffects = ctx.useEffectRunner ()
            let switchTo = ctx.useSceneSwitcher ()

            let onProgressFinish _ =
                composeSong (State.get ()) song |> List.ofItem |> applyEffects

                Scene.InGame |> switchTo

            Layout.vertical [
                MultiProgressBar.view
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

            let genre = ctx.useState Data.Genres.all.Head

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
                    Song.from
                        name.Current
                        songLength
                        vocalStyle.Current
                        genre.Current

                [ compositionProgress song :> IView ]
                |> Subcomponent
                |> viewStack.AddAction

            Layout.vertical [
                TextBlock.create [ TextBlock.text "Song's name:" ]
                TextBox.create [
                    TextBox.watermark "Song's name"
                    TextBox.onTextChanged name.Set
                ]

                TextBlock.create [
                    TextBlock.text "Song's length (format: mm:ss, as in 5:45)"
                ]
                MaskedTextBox.create [
                    MaskedTextBox.watermark "Song's length"
                    MaskedTextBox.mask "00:00"
                    MaskedTextBox.onTextChanged lengthChanged
                ]

                TextBlock.create [ TextBlock.text "Song's genre:" ]
                Picker.create
                    {
                        Selected = genre
                        ToText = id
                        Values = Data.Genres.all
                    }

                TextBlock.create [ TextBlock.text "Song's vocal style:" ]
                Picker.create
                    {
                        Selected = vocalStyle
                        ToText = Text.Music.vocalStyleName
                        Values = Data.VocalStyles.all
                    }

                Choice.createCancellable
                    {
                        ActionLabel = "Create"
                        ActionEnabled = createEnabled
                        OnAction = onCreate
                        OnCancel = (fun () -> ())
                    }
            ]
    )
