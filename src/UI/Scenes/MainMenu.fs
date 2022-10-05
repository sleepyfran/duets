module UI.Scenes.MainMenu

open Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open UI
open UI.SceneIndex
open UI.Hooks.Scene

type private MainMenuOption =
    | NewGame
    | LoadGame
    | Exit

let private gameVersion =
    System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()

let view =
    Component.create (
        "MainMenu",
        fun ctx ->
            let switchTo = ctx.useSceneSwitcher ()

            let exit _ = System.Environment.Exit(0)

            let newGame _ = switchTo Scene.NewGame
            let loadGame _ = switchTo Scene.InGame

            let savegameStatus = Savegame.load ()

            let loadGameEnabled =
                match savegameStatus with
                | Savegame.Available -> true
                | _ -> false

            StackPanel.create [
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.spacing Theme.Padding.medium
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
                        TextBlock.horizontalAlignment HorizontalAlignment.Right
                    ]

                    Button.create [
                        Button.content "New game"
                        Button.onClick newGame
                        Button.classes [ "menu" ]
                    ]

                    Button.create [
                        Button.content "Load game"
                        Button.isEnabled loadGameEnabled
                        Button.onClick loadGame
                        Button.classes [ "menu" ]
                    ]

                    Button.create [
                        Button.content "Exit"
                        Button.classes [ "menu"; "destructive" ]
                        Button.onClick exit
                    ]
                ]
            ]
    )
