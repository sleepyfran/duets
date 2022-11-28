module Cli.Scenes.CharacterCreator

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities

let private genderText gender =
    match gender with
    | Male -> Creator.characterGenderMale
    | Female -> Creator.characterGenderFemale
    | Other -> Creator.characterGenderOther

let private showNameError error =
    match error with
    | Character.NameTooShort -> Creator.errorCharacterNameTooShort
    | Character.NameTooLong -> Creator.errorCharacterNameTooLong
    |> showMessage

let private showAgeError error =
    match error with
    | Character.AgeTooYoung -> Creator.errorCharacterAgeTooYoung
    | Character.AgeTooOld -> Creator.errorCharacterAgeTooOld
    |> showMessage

/// Shows a wizard to create a character.
let rec characterCreator () = promptForName ()

and private promptForName () =
    showTextPrompt Creator.characterInitialPrompt
    |> Character.validateName
    |> Result.switch promptForGender (showNameError >> promptForName)

and private promptForGender name =
    showChoicePrompt
        Creator.characterGenderPrompt
        genderText
        [ Male; Female; Other ]
    |> promptForBirthday name

and private promptForBirthday name gender =
    Creator.characterBirthdayInfo |> showMessage

    Creator.characterBirthdayPrompt gender
    |> showTextDatePrompt
    |> Character.validateBirthday
    |> Result.switch
        (Character.from name gender >> Scene.BandCreator)
        (fun error ->
            showAgeError error
            promptForBirthday name gender)
