module Duets.UI.Scenes.NewGame

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Setup
open Duets.UI
open Duets.UI.Theme
open Duets.UI.Components.Divider
open Duets.UI.Components.Layout
open Duets.UI.Components.Picker
open Duets.UI.Hooks.Scene
open Duets.UI.Hooks.Effect
open Duets.UI.SceneIndex

let characterState = {|
    Name = new State<string>("")
    Gender = new State<Gender>(Gender.Other)
    BirthYear = new State<string>("")
|}

let bandState = {|
    Name = new State<string>("")
    Genre = new State<Genre>(Duets.Data.Genres.all |> List.head)
    CharacterInstrument = new State<InstrumentType>(Duets.Data.Roles.all |> List.head)
|}

let cityState = new State<City>(Queries.World.allCities |> List.head)

let private characterCreator =
    Component.create (
        "CharacterCreator",
        fun ctx ->
            let name = ctx.usePassed characterState.Name
            let gender = ctx.usePassed characterState.Gender
            let birthYear = ctx.usePassed characterState.BirthYear

            vertical [
                TextBlock.create [ TextBlock.text "Your character's name:" ]
                TextBox.create [
                    TextBox.watermark "Character's name"
                    TextBox.onTextChanged name.Set
                ]

                TextBlock.create [ TextBlock.text "Gender:" ]
                create
                    {
                        Selected = gender
                        ToText = Text.Character.gender
                        Values = [ Male; Female; Other ]
                    }

                TextBlock.create [
                    TextBlock.text
                        "Birth year (e.g. 1990, must be at least 18 years before game start):"
                ]
                TextBox.create [
                    TextBox.watermark "Birth year"
                    TextBox.onTextChanged birthYear.Set
                ]
            ]
    )

let private bandCreator =
    Component.create (
        "BandCreator",
        fun ctx ->
            let name = ctx.usePassed bandState.Name
            let genre = ctx.usePassed bandState.Genre
            let instrument = ctx.usePassed bandState.CharacterInstrument

            vertical [
                TextBlock.create [ TextBlock.text "Your band's name:" ]
                TextBox.create [
                    TextBox.watermark "Band's name"
                    TextBox.onTextChanged name.Set
                ]

                TextBlock.create [
                    TextBlock.text "Genre: (you can always change this later)"
                ]
                create
                    {
                        Selected = genre
                        ToText = id
                        Values = Duets.Data.Genres.all
                    }

                TextBlock.create [
                    TextBlock.text "Which instrument will you play?"
                ]
                create
                    {
                        Selected = instrument
                        ToText = Text.Music.roleName
                        Values = Duets.Data.Roles.all
                    }
            ]
    )

let private worldSelector =
    Component.create (
        "WorldSelector",
        fun ctx ->
            let city = ctx.usePassed cityState

            vertical [
                TextBlock.create [
                    TextBlock.text "Which city do you want to start in?"
                ]
                create
                    {
                        Selected = city
                        ToText = (fun (c: City) -> Text.World.Cities.name c.Id)
                        Values = Queries.World.allCities
                    }
            ]
    )

let view =
    Component.create (
        "NewGame",
        fun ctx ->
            let switchTo = ctx.useSceneSwitcher ()
            let applyEffects = ctx.useEffectRunner ()

            let characterName = ctx.usePassedRead characterState.Name
            let characterGender = ctx.usePassedRead characterState.Gender
            let birthYearStr = ctx.usePassedRead characterState.BirthYear
            let bandName = ctx.usePassedRead bandState.Name
            let bandGenre = ctx.usePassedRead bandState.Genre

            let characterInstrument =
                ctx.usePassedRead bandState.CharacterInstrument

            let selectedCity = ctx.usePassedRead cityState

            let tryParseBirthday () =
                match System.Int32.TryParse(birthYearStr.Current) with
                | true, year ->
                    Character.validateBirthday (year * 1<years>)
                | _ -> Error Character.AgeTooYoung

            let newGameEnabled =
                (Character.validateName characterName.Current |> Result.isOk)
                && (Band.validateName bandName.Current |> Result.isOk)
                && (tryParseBirthday () |> Result.isOk)

            let onNewGame _ =
                let birthday = tryParseBirthday () |> Result.unwrap

                let character =
                    Character.from
                        characterName.Current
                        characterGender.Current
                        birthday

                let city = selectedCity.Current

                let bandMember =
                    Band.Member.from
                        character.Id
                        characterInstrument.Current
                        Calendar.gameBeginning

                let band =
                    Band.from
                        bandName.Current
                        bandGenre.Current
                        bandMember
                        Calendar.gameBeginning
                        city.Id

                [ startGame character band [] city ] |> applyEffects

                Scene.InGame |> switchTo

            let onBack _ = switchTo Scene.MainMenu

            StackPanel.create [
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.spacing Padding.big
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text
                            "Welcome to Duets, let's start by creating your character and your very own band."
                    ]

                    characterCreator

                    Duets.UI.Components.Divider.vertical

                    bandCreator

                    Duets.UI.Components.Divider.vertical

                    worldSelector

                    Duets.UI.Components.Divider.vertical

                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.spacing Padding.small
                        StackPanel.children [
                            Button.create [
                                Button.content "➡️ Start game"
                                Button.isEnabled newGameEnabled
                                Button.onClick onNewGame
                            ]

                            Button.create [
                                Button.content "Go back"
                                Button.classes [ "destructive" ]
                                Button.onClick onBack
                            ]
                        ]
                    ]
                ]
            ]
    )
