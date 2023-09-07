namespace Duets.Cli.Components.Commands

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets
open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Songs.Composition.ComposeSong

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
        showLengthPrompt (Rehearsal.composeSongLengthPrompt)
        |> Song.validateLength
        |> Result.switch
            (promptForVocalStyle name)
            (showLengthError >> fun _ -> promptForLength name)

    and private promptForVocalStyle name length =
        let vocalStyle =
            showChoicePrompt
                Rehearsal.composeSongVocalStylePrompt
                snd
                Data.VocalStyles.allNames
            |> fst

        Song.from name length vocalStyle |> composeWithProgressbar

    and private composeWithProgressbar song =
        let state = State.get ()

        showProgressBarAsync
            [ Rehearsal.composeSongProgressBrainstorming
              Rehearsal.composeSongProgressConfiguringReverb
              Rehearsal.composeSongProgressTryingChords ]
            2<second>

        Rehearsal.composeSongConfirmation song.Name |> showMessage

        composeSong state song |> Duets.Cli.Effect.applyMultiple

    /// Command to compose a new song.
    let get =
        { Name = "compose song"
          Description = Command.composeSongDescription
          Handler =
            (fun _ ->
                promptForName ()

                Scene.World) }
