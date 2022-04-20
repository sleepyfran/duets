module Cli.Scenes.InteractiveSpaces.RehearsalRoom.FinishSong

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.FinishSong

let rec finishSongSubScene () = promptForSong ()

and private promptForSong () =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let songs =
        Songs.unfinishedByBand state currentBand.Id
        |> List.ofMapValues

    let selectedSong =
        showOptionalChoicePrompt
            (RehearsalSpaceText FinishSongSelection
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (UnfinishedSong us, _, currentQuality) ->
                CommonSongWithDetails(us.Name, currentQuality, us.Length)
                |> CommonText
                |> I18n.translate)
            songs

    match selectedSong with
    | Some song -> finishSong currentBand song |> Cli.Effect.apply
    | None -> ()

    Scene.World
