module UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open UI
open UI.Components
open UI.SceneIndex

let private content scene =
    Component.create (
        "SceneRootContent",
        fun ctx ->
            let currentScene = ctx.usePassedRead scene

            Border.create [
                Border.background Theme.Brush.containerBg
                Border.cornerRadius 10
                Border.margin (80, 30)
                Border.child (
                    ScrollViewer.create [
                        ScrollViewer.content (
                            StackPanel.create [
                                StackPanel.horizontalAlignment
                                    HorizontalAlignment.Stretch
                                StackPanel.margin 50
                                StackPanel.children [
                                    match currentScene.Current with
                                    | Scene.NewGame -> NewGame.view
                                    | Scene.MainMenu -> MainMenu.view
                                    | Scene.InGame ->
                                        TextBlock.create [
                                            TextBlock.text "Coming soon"
                                        ]
                                ]
                            ]
                        )
                    ]
                )
            ]
    )

let view =
    Component(fun ctx ->
        let currentScene = ctx.usePassedRead Store.shared.CurrentScene

        DockPanel.create [
            DockPanel.children [
                if currentScene.Current = Scene.InGame then
                    Header.view

                content currentScene
            ]
        ])
