module UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open UI
open UI.SceneIndex

let view =
    Component (fun ctx ->
        let currentScene =
            ctx.usePassedRead Store.shared.CurrentScene

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
        ])
