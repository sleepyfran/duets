namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Songs.Composition.FinishSong

[<RequireQualifiedAccess>]
module FinishSongCommand =
    /// Command to finish an unfinished song.
    let create unfinishedSongs =
        { Name = "finish song"
          Description =
            I18n.translate (CommandText CommandFinishSongDescription)
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        (RehearsalSpaceText FinishSongSelection
                         |> I18n.translate)
                        (CommonText CommonCancel |> I18n.translate)
                        (fun (UnfinishedSong us, _, currentQuality) ->
                            CommonSongWithDetails(
                                us.Name,
                                currentQuality,
                                us.Length
                            )
                            |> CommonText
                            |> I18n.translate)
                        unfinishedSongs

                match selectedSong with
                | Some song -> finishSong currentBand song |> Cli.Effect.apply
                | None -> ()

                Scene.World) }
