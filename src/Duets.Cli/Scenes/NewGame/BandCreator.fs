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
    let instrument =
        showInstrumentPrompt Creator.bandInstrumentPrompt genre Data.Roles.all

    promptForExtraInstrument character originCity name genre [ instrument ]

and private promptForExtraInstrument character originCity name genre roles =
    let compatibleInstruments =
        Data.Roles.all
        |> List.filter (fun instrument ->
            Instrument.compatible (roles @ [ instrument ]))

    let extraInstrument =
        match compatibleInstruments with
        | [] -> None
        | _ ->
            showOptionalInstrumentPrompt
                Creator.bandExtraInstrumentPrompt
                Creator.bandNoExtraInstrument
                genre
                compatibleInstruments

    match extraInstrument with
    | Some instrument ->
        promptForExtraInstrument
            character
            originCity
            name
            genre
            (roles @ [ instrument ])
    | None -> promptForConfirmation character originCity name genre roles

and private promptForConfirmation
    character
    (originCity: City)
    name
    genre
    roles
    =
    let confirmed =
        showConfirmationPrompt (
            Creator.bandConfirmationPrompt character.Name name genre roles
        )

    if confirmed then
        let characterMember =
            Band.Member.from character.Id roles Calendar.gameBeginning

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
