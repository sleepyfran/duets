module Cli.Scenes.BandCreator

open Cli
open Cli.Components
open Cli.Text
open Cli.SceneIndex
open Common
open Entities
open Simulation.Setup

let private showNameError error =
    match error with
    | Band.NameTooShort -> Creator.errorBandNameTooShort
    | Band.NameTooLong -> Creator.errorBandNameTooLong
    |> showMessage

let private instrumentNameText instrumentType =
    Generic.instrument instrumentType

/// Shows a wizard to create a band for the given character.
let rec bandCreator (character: Character) = promptForName character

and private promptForName character =
    showTextPrompt Creator.bandInitialPrompt
    |> Band.validateName
    |> Result.switch
        (promptForGenre character)
        (showNameError
         >> (fun _ -> promptForName character))

and private promptForGenre character name =
    showChoicePrompt Creator.bandGenrePrompt id Data.Genres.all
    |> promptForInstrument character name

and private promptForInstrument character name genre =
    showChoicePrompt Creator.bandGenrePrompt instrumentNameText Data.Roles.all
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

        startGame character band |> Effect.apply
        clearScreen ()
        Scene.World
    else
        Scene.CharacterCreator
