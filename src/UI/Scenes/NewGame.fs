module UI.Scenes.NewGame

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Common
open Entities
open Simulation.Setup
open System
open UI
open UI.Components
open UI.Hooks.Scene
open UI.SceneIndex

let characterState =
    {|
        Name = new State<string>("")
        Gender = new State<Gender>(Gender.Other)
        Birthday =
            new State<Date>(
                Calendar.gameBeginning
                |> Calendar.Ops.addYears -18
            )
    |}

let bandState =
    {|
        Name = new State<string>("")
        Genre = new State<Genre>(Data.Genres.all |> List.head)
        CharacterInstrument =
            new State<InstrumentType>(Data.Roles.all |> List.head)
    |}

let private characterCreator =
    Component.create (
        "CharacterCreator",
        fun ctx ->
            let name = ctx.usePassed characterState.Name

            let gender =
                ctx.usePassed characterState.Gender

            let birthday =
                ctx.usePassed characterState.Birthday

            let birthdaySelected (dateOffset: System.Nullable<DateTimeOffset>) =
                birthday.Set dateOffset.Value.DateTime

            StackPanel.create [
                StackPanel.spacing 10
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text "Your character's name:"
                    ]
                    TextBox.create [
                        TextBox.watermark "Character's name"
                        TextBox.onTextChanged name.Set
                    ]

                    TextBlock.create [
                        TextBlock.text "Gender:"
                    ]
                    Picker.view
                        gender
                        Text.Character.gender
                        [ Male; Female; Other ]

                    TextBlock.create [
                        TextBlock.text "Birthday:"
                    ]
                    DatePicker.create [
                        DatePicker.selectedDate birthday.Current
                        DatePicker.onSelectedDateChanged birthdaySelected
                        Calendar.gameBeginning.AddYears -18
                        |> DatePicker.maxYear
                    ]
                ]
            ]
    )

let private bandCreator =
    Component.create (
        "BandCreator",
        fun ctx ->
            let name = ctx.usePassed bandState.Name
            let genre = ctx.usePassed bandState.Genre

            let instrument =
                ctx.usePassed bandState.CharacterInstrument

            StackPanel.create [
                StackPanel.spacing 10
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text "Your band's name:"
                    ]
                    TextBox.create [
                        TextBox.watermark "Band's name"
                        TextBox.onTextChanged name.Set
                    ]

                    TextBlock.create [
                        TextBlock.text
                            "Genre: (you can always change this later)"
                    ]
                    Picker.view genre id Data.Genres.all

                    TextBlock.create [
                        TextBlock.text "Which instrument will you play?"
                    ]
                    Picker.view instrument Text.Music.roleName Data.Roles.all
                ]
            ]
    )

let view =
    Component.create (
        "NewGame",
        fun ctx ->
            let switchTo = ctx.useSceneSwitcher ()

            let characterName =
                ctx.usePassedRead characterState.Name

            let characterGender =
                ctx.usePassedRead characterState.Gender

            let characterBirthday =
                ctx.usePassedRead characterState.Birthday

            let bandName =
                ctx.usePassedRead bandState.Name

            let bandGenre =
                ctx.usePassedRead bandState.Genre

            let characterInstrument =
                ctx.usePassedRead bandState.CharacterInstrument

            let newGameEnabled =
                (Character.validateName characterName.Current
                 |> Result.isOk)
                && (Band.validateName bandName.Current |> Result.isOk)

            let onNewGame _ =
                let character =
                    Character.from
                        characterName.Current
                        characterGender.Current
                        characterBirthday.Current

                let band =
                    Band.Member.from
                        character.Id
                        characterInstrument.Current
                        Calendar.gameBeginning
                    |> fun mem ->
                        Band.from
                            bandName.Current
                            bandGenre.Current
                            mem
                            Calendar.gameBeginning

                startGame character band |> Effect.apply
                switchTo Scene.InGame

            StackPanel.create [
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.spacing 20
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text
                            "Welcome to Duets, let’s start by creating your character and your very own band."
                    ]

                    characterCreator

                    Divider.view

                    bandCreator

                    Divider.view

                    Button.create [
                        Button.content "➡️ Start game"
                        Button.isEnabled newGameEnabled
                        Button.onClick onNewGame
                    ]
                ]
            ]
    )
