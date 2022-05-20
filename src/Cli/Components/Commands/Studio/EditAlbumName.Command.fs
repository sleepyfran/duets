namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Studio.RenameAlbum

[<RequireQualifiedAccess>]
module EditAlbumNameCommand =
    let rec private promptForAlbum unreleasedAlbums =
        let state = State.get ()

        let currentBand =
            Queries.Bands.currentBand state

        showChoicePrompt
            (StudioText StudioContinueRecordPrompt
             |> I18n.translate)
            (fun (UnreleasedAlbum album) -> I18n.constant album.Name)
            unreleasedAlbums
        |> promptForAlbumName currentBand

    and private promptForAlbumName band album =
        let name =
            showTextPrompt (
                StudioText StudioCreateRecordName
                |> I18n.translate
            )

        Album.validateName name
        |> Result.switch
            (fun name -> renameAlbum band album name |> Cli.Effect.apply)
            (Studio.showAlbumNameError
             >> fun _ -> promptForAlbumName band album)

    /// Command to edit the name of an unreleased album.
    let create unreleasedAlbums =
        { Name = "edit album name"
          Description =
            I18n.translate (CommandText CommandEditAlbumNameDescription)
          Handler =
            (fun _ ->
                promptForAlbum unreleasedAlbums
                Scene.World) }
