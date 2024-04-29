namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module CreateAlbumCommand =
    let rec private promptForName studio finishedSongs =
        showTextPrompt Studio.createRecordName
        |> Album.validateName
        |> Result.switch
            (promptForFirstTrack studio finishedSongs)
            (Studio.showAlbumNameError
             >> fun _ -> promptForName studio finishedSongs)

    and private promptForFirstTrack
        studio
        (finishedSongs: Finished<Song> seq)
        name
        =
        finishedSongs
        |> showOptionalChoicePrompt
            Studio.createTrackListPrompt
            Generic.cancel
            (fun (Finished(fs, currentQuality)) ->
                Generic.songWithDetails fs.Name currentQuality fs.Length)
        |> Option.iter (promptForProducer studio finishedSongs name)

    and private promptForProducer studio finishedSongs name firstTrack =
        [ SelectedProducer.StudioProducer; SelectedProducer.PlayableCharacter ]
        |> showChoicePrompt Studio.producerPrompt (fun producer ->
            let state = State.get ()

            match producer with
            | SelectedProducer.PlayableCharacter ->
                let character = Queries.Characters.playableCharacter state

                let _, skillLevel =
                    Queries.Skills.characterSkillWithLevel
                        state
                        character.Id
                        SkillId.MusicProduction

                Studio.producerPlayableCharacterSelection skillLevel
            | SelectedProducer.StudioProducer ->
                let studioPlace = Queries.World.currentPlace state

                Studio.producerStudioProducerSelection
                    studio.PricePerSong
                    studioPlace.Quality)
        |> promptForConfirmation studio finishedSongs name firstTrack

    and private promptForConfirmation
        studio
        finishedSongs
        name
        selectedSong
        producer
        =
        let (Finished(fs, _)) = selectedSong

        let confirmed =
            Studio.confirmRecordingPrompt fs.Name |> showConfirmationPrompt

        if confirmed then
            checkBankAndRecordAlbum studio producer name selectedSong
        else
            promptForFirstTrack studio finishedSongs name

    and private checkBankAndRecordAlbum
        studio
        selectedProducer
        albumName
        selectedSong
        =
        let state = State.get ()

        let band = Queries.Bands.currentBand state

        StudioStartAlbum
            {| Studio = studio
               SelectedProducer = selectedProducer
               Band = band
               AlbumName = albumName
               FirstSong = selectedSong |}
        |> Effect.applyAction

    /// Command to create a new album and potentially release.
    let create studio finishedSongs =
        { Name = "create album"
          Description =
            "Allows you to create a new album and record a song for it"
          Handler =
            (fun _ ->
                if List.isEmpty finishedSongs then
                    Studio.createNoSongs |> showMessage
                else
                    promptForName studio finishedSongs

                Scene.World) }
