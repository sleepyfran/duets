module UI.Components.Header

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Entities
open Simulation
open UI
open UI.Hooks.GameState

let private headerText text customAttrs =
    [
        TextBlock.verticalAlignment VerticalAlignment.Center
        TextBlock.text text
    ]
    @ customAttrs
    |> TextBlock.create
    :> IView

let private headerEmojiText emoji text =
    StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.verticalAlignment VerticalAlignment.Center
        StackPanel.spacing 5
        StackPanel.children [
            TextBlock.create [ TextBlock.text emoji ]

            TextBlock.create [ TextBlock.text text ]
        ]
    ]
    :> IView

let private characterAttributes state =
    let attrs =
        Queries.Characters.allPlayableCharacterAttributes state

    let allowedAttributes = [
        CharacterAttribute.Health
        CharacterAttribute.Energy
        CharacterAttribute.Fame
    ]

    attrs
    |> List.filter (fun (attr, _) -> allowedAttributes |> List.contains attr)
    |> List.map (fun (attr, amount) ->
        headerEmojiText (Text.Emoji.attribute attr amount) $"{amount}")

let private concertAttributes state =
    let situation =
        Queries.Situations.current state

    match situation with
    | InConcert ongoingConcert -> [
        headerEmojiText Text.Emoji.concert $"{ongoingConcert.Points}"
      ]
    | _ -> []

let view (ctx: IComponentContext) =
    let state = ctx.useGameState ()

    let today =
        Queries.Calendar.today state.Current

    let currentDayMoment =
        Calendar.Query.dayMomentOf today

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
                        StackPanel.verticalAlignment VerticalAlignment.Stretch
                        StackPanel.spacing 5
                        StackPanel.margin (10, 0)
                        [
                            headerText (Text.Date.format today) []

                            headerText (Text.Emoji.dayMoment currentDayMoment) []

                            headerText
                                (Text.Date.dayMomentName currentDayMoment)
                                [
                                    (TextBlock.foreground Theme.Brush.fg)
                                ]

                            Divider.horizontal

                            yield! characterAttributes state.Current
                            yield! concertAttributes state.Current
                        ]
                        |> StackPanel.children
                    ]
                )
            ]

            Button.create [
                Button.isEnabled false
                Button.content "🗺️ Map"
            ]

            Button.create [
                Button.isEnabled false
                Button.content "📱 Phone"
            ]
        ]
    ]
