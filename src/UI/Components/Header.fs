module UI.Components.Header

open Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Entities
open Simulation
open UI

let view =
    let today = Queries.Calendar.today (State.get ())

    let currentDayMoment = Calendar.Query.dayMomentOf today

    let characterAttributes =
        Queries.Characters.allPlayableCharacterAttributes (State.get ())

    StackPanel.create [
        StackPanel.dock Dock.Top
        StackPanel.orientation Orientation.Horizontal
        StackPanel.horizontalAlignment HorizontalAlignment.Stretch
        StackPanel.verticalAlignment VerticalAlignment.Top
        StackPanel.margin (80, 30, 80, 0)
        StackPanel.spacing 10
        StackPanel.children [
            Border.create [
                Border.background Theme.Brush.containerBg
                Border.cornerRadius 10
                Border.child (
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.verticalAlignment VerticalAlignment.Center
                        StackPanel.spacing 10
                        StackPanel.margin (10, 0)
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.text
                                    $"""{Text.Date.format today} {Text.Emoji.dayMoment currentDayMoment}"""
                            ]
                            TextBlock.create [
                                TextBlock.foreground Theme.Brush.fg
                                Text.Date.dayMomentName currentDayMoment
                                |> TextBlock.text
                            ]

                            Divider.horizontal
                        ]
                    ]
                )
            ]

            Button.create [ Button.content "🗺️ Map" ]

            Button.create [ Button.content "📱 Phone" ]
        ]
    ]
