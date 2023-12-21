module Duets.Cli.Scenes.NewGame.BandCreator

open Duets
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.SceneIndex
open Duets.Common
open Duets.Entities

let private showNameError error =
    match error with
    | Band.NameTooShort -> Creator.errorBandNameTooShort
    | Band.NameTooLong -> Creator.errorBandNameTooLong
    |> showMessage

/// Shows a wizard to create a band for the given character.
let rec bandCreator (character: Character) originCity =
    showSeparator None
    promptForName character originCity

and private promptForName character originCity =
    showTextPrompt Creator.bandInitialPrompt
    |> Band.validateName
    |> Result.switch
        (promptForGenre character originCity)
        (showNameError >> (fun _ -> promptForName character originCity))

and private promptForGenre character originCity name =
    showChoicePrompt Creator.bandGenrePrompt id Data.Genres.all
    |> promptForInstrument character originCity name

and private promptForInstrument character originCity name genre =
    showChoicePrompt Creator.bandInstrumentPrompt Generic.role Data.Roles.all
    |> promptForConfirmation character originCity name genre

and private promptForConfirmation
    character
    (originCity: City)
    name
    genre
    instrument
    =
    let confirmed =
        showConfirmationPrompt (
            Creator.bandConfirmationPrompt character.Name name genre instrument
        )

    if confirmed then
        let characterMember =
            Band.Member.from character.Id instrument Calendar.gameBeginning

        let band =
            Band.from
                name
                genre
                characterMember
                Calendar.gameBeginning
                originCity.Id

        Scene.SkillEditor(character, characterMember, band, originCity)
    else
        Scene.CharacterCreator
