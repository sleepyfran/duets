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
    | Band.NameTooShort -> CreatorText CreatorErrorBandNameTooShort
    | Band.NameTooLong -> CreatorText CreatorErrorBandNameTooLong
    |> I18n.translate
    |> showMessage

/// Shows a wizard to create a band for the given character.
let rec bandCreator (character: Character) = promptForName character

and private promptForName character =
    showTextPrompt (
        CreatorText BandCreatorInitialPrompt
        |> I18n.translate
    )
    |> Band.validateName
    |> Result.switch
        (promptForGenre character)
        (showNameError
         >> (fun _ -> promptForName character))

and private promptForGenre character name =
    let genres = Database.genres ()

    showChoicePrompt
        (CreatorText BandCreatorGenrePrompt
         |> I18n.translate)
        I18n.constant
        genres
    |> promptForInstrument character name

and private promptForInstrument character name genre =
    let instruments = Database.roles ()

    showChoicePrompt
        (CreatorText BandCreatorGenrePrompt
         |> I18n.translate)
        I18n.constant
        instruments
    |> promptForConfirmation character name genre

and private promptForConfirmation character name genre instrument =
    let confirmed =
        showConfirmationPrompt (
            BandCreatorConfirmationPrompt(
                character.Name,
                name,
                genre,
                instrument
            )
            |> CreatorText
            |> I18n.translate
        )

    let characterMember =
        Band.Member.from
            character
            (Instrument.Type.from instrument)
            (Calendar.gameBeginning)

    let band =
        Band.from name genre characterMember Calendar.gameBeginning

    startGame character band |> Effect.apply
    clearScreen ()
    Scene.World
