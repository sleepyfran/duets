module Duets.Cli.Scenes.NewGame.CharacterCreator

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities

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
    |> showRangedNumberPrompt 1930 (Calendar.gameBeginning.Year / 1<years>)
    |> (*) 1<years>
    |> Character.validateBirthday
    |> Result.switch
        (Character.from name gender >> Scene.WorldSelector)
        (fun error ->
            showAgeError error
            promptForBirthday name gender)
