namespace Duets.Cli.Components.Commands

open Duets.Cli
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets
open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module ComposeSongCommand =
    let private showNameError error =
        match error with
        | Song.NameTooShort -> Rehearsal.composeSongErrorNameTooShort
        | Song.NameTooLong -> Rehearsal.composeSongErrorNameTooLong

    let private showLengthError error =
        match error with
        | Song.LengthTooShort -> Rehearsal.composeSongErrorLengthTooShort
        | Song.LengthTooLong -> Rehearsal.composeSongErrorLengthTooLong

    let rec private promptForName () =
        showTextPrompt Rehearsal.composeSongTitlePrompt
        |> Song.validateName
        |> Result.switch
            promptForLength
            (showNameError >> fun _ -> promptForName ())

    and private promptForLength name =
        showLengthPrompt Rehearsal.composeSongLengthPrompt
        |> Song.validateLength
        |> Result.switch
            (promptForVocalStyle name)
            (showLengthError >> fun _ -> promptForLength name)

    and private promptForVocalStyle name length =
        let hasVocalist =
            Queries.Bands.currentBandHasAnyMemberWithRole (State.get ()) Vocals

        if not hasVocalist then
            "To compose songs with vocals, you need to have a vocalist in your band. You can hire a new member from the rehearsal room."
            |> Styles.hint
            |> showMessage

        let vocalStyles =
            if hasVocalist then
                Data.VocalStyles.allNames
            else
                [ (Instrumental, Instrumental.ToString()) ]

        let vocalStyle =
            showChoicePrompt
                Rehearsal.composeSongVocalStylePrompt
                snd
                vocalStyles
            |> fst

        Song.from name length vocalStyle |> composeWithProgressbar

    and private composeWithProgressbar song =
        let state = State.get ()
        let currentBand = Queries.Bands.currentBand state

        showProgressBarAsync
            [ Rehearsal.composeSongProgressBrainstorming
              Rehearsal.composeSongProgressConfiguringReverb
              Rehearsal.composeSongProgressTryingChords ]
            2<second>

        Rehearsal.composeSongConfirmation song.Name |> showMessage

        RehearsalRoomComposeSong {| Band = currentBand; Song = song |}
        |> Effect.applyAction

    /// Command to compose a new song.
    let get =
        { Name = "compose song"
          Description = Command.composeSongDescription
          Handler =
            (fun _ ->
                promptForName ()

                Scene.World) }
