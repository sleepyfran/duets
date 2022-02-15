module Cli.Scenes.InteractiveSpaces.RehearsalRoom.DiscardSong

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.DiscardSong

let rec discardSongSubScene () = promptForSong ()

and private promptForSong () =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let songs =
        Songs.unfinishedByBand state currentBand.Id
        |> List.ofMapValues

    let selectedSong =
        showOptionalChoicePrompt
            (RehearsalSpaceText DiscardSongSelection
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (UnfinishedSong us, _, currentQuality) ->
                CommonSongWithDetails(us.Name, currentQuality, us.Length)
                |> CommonText
                |> I18n.translate)
            songs

    match selectedSong with
    | Some song -> discardSong currentBand song |> Cli.Effect.apply
    | None -> ()

    Scene.World
