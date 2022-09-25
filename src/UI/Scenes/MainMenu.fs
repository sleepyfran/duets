module UI.Scenes.MainMenu

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open UI

type private MainMenuOption =
    | NewGame
    | LoadGame
    | Exit

let private gameVersion =
    System
        .Reflection
        .Assembly
        .GetEntryAssembly()
        .GetName()
        .Version.ToString()

let view =
    Component (fun ctx ->
        let exit _ = System.Environment.Exit(0)

        Border.create [
            Border.background Theme.Brush.containerBg
            Border.cornerRadius 10
            Border.margin (80, 30)
            Border.child (
                StackPanel.create [
                    StackPanel.horizontalAlignment HorizontalAlignment.Stretch
                    StackPanel.margin 50
                    StackPanel.children [
                        StackPanel.create [
                            StackPanel.horizontalAlignment
                                HorizontalAlignment.Center
                            StackPanel.spacing 10
                            StackPanel.children [
                                TextBlock.create [
                                    TextBlock.text "duets"
                                    TextBlock.fontSize 64
                                    TextBlock.fontWeight FontWeight.Bold
                                ]

                                TextBlock.create [
                                    TextBlock.text $"v{gameVersion}"
                                    TextBlock.fontSize 20
                                    TextBlock.foreground Theme.Brush.bg
                                    TextBlock.horizontalAlignment
                                        HorizontalAlignment.Right
                                ]

                                Button.create [
                                    Button.content "New game"
                                ]

                                Button.create [
                                    Button.content "Load game"
                                ]

                                Button.create [
                                    Button.content "Exit"
                                    Button.borderBrush Theme.Brush.destructive
                                    Button.onClick exit
                                ]
                            ]
                        ]
                    ]
                ]
            )
        ])
