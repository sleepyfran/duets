namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong

[<RequireQualifiedAccess>]
module ComposeSongCommand =
    let private showNameError error =
        match error with
        | Song.NameTooShort -> RehearsalSpaceText ComposeSongErrorNameTooShort
        | Song.NameTooLong -> RehearsalSpaceText ComposeSongErrorNameTooLong
        |> I18n.translate

    let private showLengthError error =
        match error with
        | Song.LengthTooShort ->
            RehearsalSpaceText ComposeSongErrorLengthTooShort
        | Song.LengthTooLong -> RehearsalSpaceText ComposeSongErrorLengthTooLong
        |> I18n.translate

    let rec private promptForName () =
        showTextPrompt (
            RehearsalSpaceText ComposeSongTitlePrompt
            |> I18n.translate
        )
        |> Song.validateName
        |> Result.switch
            promptForLength
            (showNameError >> fun _ -> promptForName ())

    and private promptForLength name =
        showLengthPrompt (
            RehearsalSpaceText ComposeSongLengthPrompt
            |> I18n.translate
        )
        |> Song.validateLength
        |> Result.switch
            (promptForGenre name)
            (showLengthError >> fun _ -> promptForLength name)

    and private promptForGenre name length =
        let genres = Database.genres ()

        showChoicePrompt
            (RehearsalSpaceText ComposeSongGenrePrompt
             |> I18n.translate)
            I18n.constant
            genres
        |> promptForVocalStyle name length

    and private promptForVocalStyle name length genre =
        let vocalStyles = Database.vocalStyles

        let vocalStyle =
            showChoicePrompt
                (RehearsalSpaceText ComposeSongVocalStylePrompt
                 |> I18n.translate)
                (snd >> I18n.constant)
                vocalStyles
            |> fst

        Song.from name length vocalStyle genre
        |> composeWithProgressbar

    and private composeWithProgressbar song =
        let state = State.get ()

        showProgressBarAsync
            [ RehearsalSpaceText ComposeSongProgressBrainstorming
              |> I18n.translate
              RehearsalSpaceText ComposeSongProgressConfiguringReverb
              |> I18n.translate
              RehearsalSpaceText ComposeSongProgressTryingChords
              |> I18n.translate ]
            2<second>

        ComposeSongConfirmation song.Name
        |> RehearsalSpaceText
        |> I18n.translate
        |> showMessage

        composeSong state song |> Cli.Effect.apply

    /// Command to compose a new song.
    let get =
        { Name = "compose song"
          Description =
            I18n.translate (CommandText CommandComposeSongDescription)
          Handler =
            (fun _ ->
                promptForName ()

                Scene.World) }