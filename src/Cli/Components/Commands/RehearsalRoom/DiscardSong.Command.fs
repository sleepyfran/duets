namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Songs.Composition.DiscardSong

[<RequireQualifiedAccess>]
module DiscardSongCommand =
    /// Command to discard an unfinished song.
    let create unfinishedSongs =
        { Name = "discard song"
          Description = I18n.translate (CommandText CommandWaitDescription)
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        (RehearsalSpaceText DiscardSongSelection
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
                | Some song -> discardSong currentBand song |> Cli.Effect.apply
                | None -> ()

                Scene.World) }
