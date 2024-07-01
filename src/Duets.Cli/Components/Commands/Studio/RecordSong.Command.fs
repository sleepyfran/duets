namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module RecordSongCommand =
    let rec private promptForSong studio unreleasedAlbums finishedSongs =
        showOptionalChoicePrompt
            $"""Select which {Styles.highlight "song"} will be added to the album"""
            Generic.cancel
            (fun (Finished(fs: Song, currentQuality)) ->
                Generic.songWithDetails fs.Name currentQuality fs.Length)
            finishedSongs
        |> Option.iter (promptForAlbum studio unreleasedAlbums finishedSongs)

    and private promptForAlbum studio unreleasedAlbums finishedSongs song =
        let (Finished(fs, _)) = song

        showOptionalChoicePrompt
            $"Which album do you want to add {fs.Name} to?"
            Generic.cancel
            (fun (unreleasedAlbum: UnreleasedAlbum) ->
                unreleasedAlbum.Album.Name)
            unreleasedAlbums
        |> Option.iter (
            promptForConfirmation studio unreleasedAlbums finishedSongs song
        )

    and private promptForConfirmation
        studio
        unreleasedAlbums
        finishedSongs
        song
        unreleasedAlbum
        =
        let album = Album.fromUnreleased unreleasedAlbum
        let (Finished(fs, _)) = song

        let confirmed =
            $"Are you sure you want to add {fs.Name} to the album {album.Name}"
            |> showConfirmationPrompt

        if confirmed then
            checkBankAndRecordSong studio unreleasedAlbum song
        else
            promptForSong studio unreleasedAlbums finishedSongs

    and private checkBankAndRecordSong studio album song =
        let state = State.get ()

        let band = Queries.Bands.currentBand state

        StudioRecordSongForAlbum
            {| Studio = studio
               Band = band
               Album = album
               Song = song |}
        |> Effect.applyAction

    /// Command to record a new song for a previously started album.
    let create studio unreleasedAlbums finishedSongs =
        { Name = "record song"
          Description =
            "Allows you to record a song and add it to a previously created album that hasn't been released yet"
          Handler =
            (fun _ ->
                if List.isEmpty finishedSongs then
                    Studio.createNoSongs |> showMessage
                else
                    promptForSong studio unreleasedAlbums finishedSongs

                Scene.World) }
