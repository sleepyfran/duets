module Cli.Scenes.NewGame.BandCreator

open Cli.Components
open Cli.Text
open Cli.SceneIndex
open Common
open Entities

let private showNameError error =
    match error with
    | Band.NameTooShort -> Creator.errorBandNameTooShort
    | Band.NameTooLong -> Creator.errorBandNameTooLong
    |> showMessage

let private instrumentNameText instrumentType =
    Generic.instrument instrumentType

/// Shows a wizard to create a band for the given character.
let rec bandCreator (character: Character) =
    showSeparator None
    promptForName character

and private promptForName character =
    showTextPrompt Creator.bandInitialPrompt
    |> Band.validateName
    |> Result.switch
        (promptForGenre character)
        (showNameError >> (fun _ -> promptForName character))

and private promptForGenre character name =
    showChoicePrompt Creator.bandGenrePrompt id Data.Genres.all
    |> promptForInstrument character name

and private promptForInstrument character name genre =
    showChoicePrompt Creator.bandInstrumentPrompt instrumentNameText Data.Roles.all
    |> promptForConfirmation character name genre

and private promptForConfirmation character name genre instrument =
    let confirmed =
        showConfirmationPrompt (
            Creator.bandConfirmationPrompt character.Name name genre instrument
        )

    if confirmed then
        let characterMember =
            Band.Member.from character.Id instrument (Calendar.gameBeginning)

        let band = Band.from name genre characterMember Calendar.gameBeginning

        Scene.SkillEditor(character, characterMember, band)
    else
        Scene.CharacterCreator
