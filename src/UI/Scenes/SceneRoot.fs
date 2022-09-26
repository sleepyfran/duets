module UI.Scenes.SceneRoot

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open UI.SceneIndex
open UI

let view =
    Component (fun ctx ->
        let currentScene = ctx.useState Scene.MainMenu

        Border.create [ Border.background Theme.Brush.containerBg
                        Border.cornerRadius 10
                        Border.margin (80, 30)
                        Border.child (
                            ScrollViewer.create [ ScrollViewer.content (
                                                      StackPanel.create [ StackPanel.horizontalAlignment
                                                                              HorizontalAlignment.Stretch
                                                                          StackPanel.margin
                                                                              50
                                                                          StackPanel.children [ match currentScene.Current
                                                                                                    with
                                                                                                | Scene.NewGame ->
                                                                                                    NewGame.view
                                                                                                | _ ->
                                                                                                    MainMenu.view
                                                                                                        currentScene ] ]
                                                  ) ]
                        ) ])
