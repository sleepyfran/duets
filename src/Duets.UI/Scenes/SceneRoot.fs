module Duets.UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Duets.Entities
open Duets.UI
open Duets.UI.Theme
open Duets.UI.Common

let private displayLabel (item: InteractionWithMetadata) : string =
    let e = Text.World.Interactions.emoji item
    let l = Duets.UI.Common.Text.World.Interactions.label item
    $"{e} {l}"

let view =
    Component(fun _ ->
        Border.create [
            Border.background Brush.containerBg
            Border.cornerRadius 10
            Border.margin (80, 30)
            Border.child (
                ScrollViewer.create [
                    ScrollViewer.content (
                        Renderer.run
                            "Game"
                            (Duets.UI.Common.Scenes.Dispatcher.run displayLabel Navigate.MainMenu)
                    )
                ]
            )
        ]
    )
