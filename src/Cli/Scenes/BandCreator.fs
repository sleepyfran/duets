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
    let genres = Database.genres ()

    showChoicePrompt Creator.bandGenrePrompt id genres
    |> promptForInstrument character name

and private promptForInstrument character name genre =
    let instruments = Database.roles

    showChoicePrompt Creator.bandGenrePrompt instrumentNameText instruments
    |> promptForConfirmation character name genre

and private promptForConfirmation character name genre instrument =
    let confirmed =
        showConfirmationPrompt (
            Creator.bandConfirmationPrompt character.Name name genre instrument
        )

    if confirmed then
        let characterMember =
            Band.Member.from character.Id instrument (Calendar.gameBeginning)

        let band =
            Band.from name genre characterMember Calendar.gameBeginning

        startGame character band |> Effect.apply
        clearScreen ()
        Scene.World
    else
        Scene.CharacterCreator
