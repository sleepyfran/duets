module Duets.UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Duets.UI
open Duets.UI.Theme
open Duets.UI.SceneIndex
open Duets.UI.Hooks.Scene

let view =
    Component(fun ctx ->
        let currentScene = ctx.usePassedRead Store.shared.CurrentScene
        let switchTo = ctx.useSceneSwitcher ()

        Border.create [
            Border.background Brush.containerBg
            Border.cornerRadius 10
            Border.margin (80, 30)
            Border.child (
                ScrollViewer.create [
                    ScrollViewer.content (
                        StackPanel.create [
                            StackPanel.horizontalAlignment HorizontalAlignment.Stretch
                            StackPanel.margin 50
                            StackPanel.children [
                                match currentScene.Current with
                                | Scene.MainMenu ->
                                    Duets.UI.Scenes.MainMenu.view switchTo
                                | Scene.NewGame ->
                                    Duets.UI.Scenes.NewGame.view switchTo
                                | Scene.InGame ->
                                    // Not yet migrated to the Scene DSL
                                    TextBlock.create [
                                        TextBlock.text "InGame scene coming soon."
                                    ]
                            ]
                        ]
                    )
                ]
            )
        ]
    )
