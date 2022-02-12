module Cli.Scenes.CharacterCreator

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Entities.Character

let private genderText gender =
    match gender with
    | Male -> CreatorText CharacterCreatorGenderMale
    | Female -> CreatorText CharacterCreatorGenderFemale
    | Other -> CreatorText CharacterCreatorGenderOther
    |> I18n.translate

let private showNameError error =
    match error with
    | NameTooShort -> CreatorText CreatorErrorCharacterNameTooShort
    | NameTooLong -> CreatorText CreatorErrorCharacterNameTooLong
    |> I18n.translate
    |> showMessage

let private showAgeError error =
    match error with
    | AgeTooYoung -> CreatorText CreatorErrorCharacterAgeTooYoung
    | AgeTooOld -> CreatorText CreatorErrorCharacterAgeTooOld
    |> I18n.translate
    |> showMessage

/// Shows a wizard to create a character.
let rec characterCreator () = promptForName ()

and promptForName () =
    showTextPrompt (
        CreatorText CharacterCreatorInitialPrompt
        |> I18n.translate
    )
    |> Character.validateName
    |> Result.switch promptForGender (showNameError >> promptForName)

and promptForGender name =
    showChoicePrompt
        (CreatorText CharacterCreatorGenderPrompt
         |> I18n.translate)
        genderText
        [ Male; Female; Other ]
    |> promptForAge name

and promptForAge name gender =
    showNumberPrompt (
        CreatorText CharacterCreatorAgePrompt
        |> I18n.translate
    )
    |> Character.validateAge
    |> Result.switch
        (Character.from name gender >> Scene.BandCreator)
        (fun error ->
            showAgeError error
            promptForAge name gender)
