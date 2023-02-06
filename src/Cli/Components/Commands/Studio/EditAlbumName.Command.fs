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

        let currentBand = Queries.Bands.currentBand state

        showOptionalChoicePrompt
            "Which album do you want to edit?"
            Generic.cancel
            (fun (UnreleasedAlbum album) -> album.Name)
            unreleasedAlbums
        |> Option.iter (promptForAlbumName currentBand)

    and private promptForAlbumName band album =
        let name = showTextPrompt Studio.createRecordName

        Album.validateName name
        |> Result.switch
            (fun name ->
                renameAlbum band album name |> Cli.Effect.apply
                $"Album renamed to {name}" |> Styles.success |> showMessage)
            (Studio.showAlbumNameError >> fun _ -> promptForAlbumName band album)

    /// Command to edit the name of an unreleased album.
    let create unreleasedAlbums =
        { Name = "edit album name"
          Description = Command.editAlbumNameDescription
          Handler =
            (fun _ ->
                promptForAlbum unreleasedAlbums
                Scene.World) }
