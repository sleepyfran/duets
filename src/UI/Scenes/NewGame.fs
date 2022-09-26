module UI.Scenes.NewGame

open System
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Entities
open UI.Components

let private characterCreator
    characterName
    characterGender
    (characterBirthday: IWritable<Date>)
    =
    Component.create (
        "CharacterCreator",
        fun ctx ->
            let name = ctx.usePassed characterName
            let gender = ctx.usePassed characterGender

            let birthday =
                ctx.usePassed characterBirthday

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
                        (function
                        | Male -> "♂ Male"
                        | Female -> "♀ Female"
                        | Other -> "⚥ Other")
                        [ Male; Female; Other ]

                    TextBlock.create [
                        TextBlock.text "Birthday:"
                    ]
                    DatePicker.create [
                        DatePicker.selectedDate characterBirthday.Current
                        DatePicker.onSelectedDateChanged birthdaySelected
                        Calendar.gameBeginning.AddYears -18
                        |> DatePicker.maxYear
                    ]
                ]
            ]
    )

let private bandCreator bandName bandGenre =
    Component.create (
        "BandCreator",
        fun ctx ->
            let name = ctx.usePassed bandName
            let genre = ctx.usePassed bandGenre

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
                ]
            ]
    )

let view =
    Component.create (
        "NewGame",
        fun ctx ->
            let characterName = ctx.useState ""

            let characterGender =
                ctx.useState Gender.Other

            let characterBirthday =
                Date.Now.AddYears -20 |> ctx.useState

            let bandName = ctx.useState ""

            let bandGenre =
                Data.Genres.all |> List.head |> ctx.useState

            let newGameEnabled =
                (String.IsNullOrEmpty characterName.Current |> not)
                && (String.IsNullOrEmpty bandName.Current |> not)

            let onNewGame _ =
                Console.WriteLine
                    $"{characterName.Current} {characterGender.Current} {characterBirthday.Current} {bandName.Current} {bandGenre.Current}"

            StackPanel.create [
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.spacing 20
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text
                            "Welcome to Duets, let’s start by creating your character and your very own band."
                    ]

                    characterCreator
                        characterName
                        characterGender
                        characterBirthday

                    Divider.view

                    bandCreator bandName bandGenre

                    Divider.view

                    Button.create [
                        Button.content "➡️ Start game"
                        Button.isEnabled newGameEnabled
                        Button.onClick onNewGame
                    ]
                ]
            ]
    )
