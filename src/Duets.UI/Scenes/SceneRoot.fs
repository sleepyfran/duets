module Duets.UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Duets.UI
open Duets.UI.Theme
open Duets.UI.SceneIndex

let private content scene =
    Component.create (
        "SceneRootContent",
        fun ctx ->
            let currentScene = ctx.usePassedRead scene

            let rec onScroll (x: ScrollChangedEventArgs) =
                if x.ExtentDelta.Y > 0 then
                    match x.Source with
                    | :? ScrollViewer as scrollViewer ->
                        scrollViewer.ScrollToEnd()
                    | _ -> ()

                ()

            Border.create [
                Border.background Brush.containerBg
                Border.cornerRadius 10
                Border.margin (80, 30)
                Border.child (
                    ScrollViewer.create [
                        ScrollViewer.onScrollChanged onScroll
                        ScrollViewer.content (
                            StackPanel.create [
                                StackPanel.horizontalAlignment
                                    HorizontalAlignment.Stretch
                                StackPanel.margin 50
                                StackPanel.children [
                                    match currentScene.Current with
                                    | Scene.NewGame ->
                                        Duets.UI.Scenes.NewGame.view
                                    | Scene.MainMenu ->
                                        Duets.UI.Scenes.MainMenu.view
                                    | Scene.InGame ->
                                        Duets.UI.Scenes.InGame.Root.view
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
                match currentScene.Current with
                | Scene.InGame ->
                    Duets.UI.Scenes.InGame.Header.view ctx
                | _ -> ()

                content currentScene
            ]
        ])
