module Duets.Scenes.CharacterCreatorScene

open Nez.UI

open Entities
open Duets.Scenes.Base
open Duets.Text.Constants

type CharacterCreatorScene() =
    inherit UiScene()

    let mutable characterName = ""
    let mutable characterAge = 18
    let mutable characterGender = Gender.Male

    override this.SetupView() =
        this.UiRoot.AddText
            (TextConstant CharacterCreatorTitle)
            TextSize.Title
            centered

        this.UiRoot.AddInput
            (TextConstant CharacterCreatorCharacterNameLabel)
            (fun text -> characterName <- text)
            centered

        // TODO: Handle validations and non-numbers
        this.UiRoot.AddInput
            (TextConstant CharacterCreatorCharacterAgeLabel)
            (fun text -> characterAge <- if text = "" then 0 else int text)
            centered

        this.UiRoot.AddSelector
            (TextConstant CharacterCreatorCharacterGenderLabel)
            [ Gender.Male
              Gender.Female
              Gender.Other ]
            (fun gender -> characterGender <- gender)
            centered

        this.UiRoot.AddButton
            (Literal "Do")
            (fun () ->
                printfn $"{characterName} {characterAge} {characterGender}")
            centered

        ()
