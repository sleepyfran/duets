module Cli.Scenes.InteractiveSpaces.RehearsalRoom.ChooseAction

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Simulation.Queries
open Simulation.Songs.Composition

type private RehearseActionMenuOption =
    | ComposeSong
    | ImproveSong
    | FinishSong
    | DiscardSong
    | PracticeSong

let private textFromOption opt =
    match opt with
    | ComposeSong -> RehearsalSpaceText RehearsalSpaceText.ComposeSong
    | ImproveSong -> RehearsalSpaceText RehearsalSpaceText.ImproveSong
    | FinishSong -> RehearsalSpaceText RehearsalSpaceText.FinishSong
    | DiscardSong -> RehearsalSpaceText RehearsalSpaceText.DiscardSong
    | PracticeSong -> RehearsalSpaceText RehearsalSpaceText.PracticeSong
    |> I18n.translate

let rec createMenu () =
    let state = State.get ()
    let band = Bands.currentBand state

    let hasUnfinishedSongs =
        Songs.unfinishedByBand state band.Id
        |> Map.isEmpty
        |> not

    let hasFinishedSongs =
        Repertoire.allFinishedSongsByBand state band.Id
        |> List.isEmpty
        |> not

    let selectedChoice =
        showOptionalChoicePrompt
            (RehearsalSpaceText ComposePrompt |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            textFromOption
            [ ComposeSong
              if hasUnfinishedSongs then
                  ImproveSong
                  FinishSong
                  DiscardSong

              if hasFinishedSongs then PracticeSong ]

    match selectedChoice with
    | Some ComposeSong -> ComposeSong.composeSongSubScene ()
    | Some ImproveSong -> ImproveSong.improveSongSubScene ()
    | Some FinishSong -> FinishSong.finishSongSubScene ()
    | Some PracticeSong -> PracticeSong.practiceSongSubScene ()
    | Some DiscardSong -> DiscardSong.discardSongSubScene ()
    | None -> Scene.World
