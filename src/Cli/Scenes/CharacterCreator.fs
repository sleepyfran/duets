module Cli.Scenes.CharacterCreator

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities

let private genderText gender =
    match gender with
    | Male -> CreatorText CharacterCreatorGenderMale
    | Female -> CreatorText CharacterCreatorGenderFemale
    | Other -> CreatorText CharacterCreatorGenderOther
    |> I18n.translate

let private showNameError error =
    match error with
    | Character.NameTooShort -> CreatorText CreatorErrorCharacterNameTooShort
    | Character.NameTooLong -> CreatorText CreatorErrorCharacterNameTooLong
    |> I18n.translate
    |> showMessage

let private showAgeError error =
    match error with
    | Character.AgeTooYoung -> CreatorText CreatorErrorCharacterAgeTooYoung
    | Character.AgeTooOld -> CreatorText CreatorErrorCharacterAgeTooOld
    |> I18n.translate
    |> showMessage

/// Shows a wizard to create a character.
let rec characterCreator () = promptForName ()

and private promptForName () =
    showTextPrompt (
        CreatorText CharacterCreatorInitialPrompt
        |> I18n.translate
    )
    |> Character.validateName
    |> Result.switch promptForGender (showNameError >> promptForName)

and private promptForGender name =
    showChoicePrompt
        (CreatorText CharacterCreatorGenderPrompt
         |> I18n.translate)
        genderText
        [ Male; Female; Other ]
    |> promptForAge name

and private promptForAge name gender =
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
