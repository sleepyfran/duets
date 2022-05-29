module Cli.Scenes.World

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Common
open Entities
open Simulation

let private descriptionFromCoordinates coords =
    match coords with
    | ResolvedPlaceCoordinates coordinates ->
        match coordinates.Room with
        | Room.Backstage -> WorldBackstageDescription coordinates.Place
        | Room.Bar -> WorldBarDescription coordinates.Place
        | Room.Lobby -> WorldLobbyDescription coordinates.Place
        | Room.MasteringRoom -> WorldMasteringRoomDescription coordinates.Place
        | Room.RecordingRoom -> WorldRecordingRoomDescription coordinates.Place
        | Room.RehearsalRoom -> WorldRehearsalRoomDescription coordinates.Place
        | Room.Stage -> WorldStageDescription coordinates.Place
        |> WorldText
    | ResolvedOutsideCoordinates coordinates ->
        (coordinates.Node.Name, coordinates.Node.Descriptors)
        |> match coordinates.Node.Type with
           | OutsideNodeType.Boulevard -> WorldStreetDescription
           | OutsideNodeType.Street -> WorldBoulevardDescription
           | OutsideNodeType.Square -> WorldSquareDescription
        |> WorldText
    |> I18n.translate

let private showRoomConnections interactionsWithState =
    let interactions =
        interactionsWithState
        |> List.map (fun interactionWithState ->
            interactionWithState.Interaction)

    let directions =
        interactions
        |> Interaction.chooseFreeRoam (fun interaction ->
            match interaction with
            | FreeRoamInteraction.Move (direction, nodeCoordinates) ->
                let coords =
                    Queries.World.Common.coordinates
                        (State.get ())
                        nodeCoordinates

                match coords.Content with
                | ResolvedPlaceCoordinates roomCoords ->
                    let currentPosition =
                        Queries.World.Common.currentPosition (State.get ())

                    match currentPosition.Content with
                    | ResolvedPlaceCoordinates _ ->
                        // Character is inside the place, show connected room name.
                        match roomCoords.Room with
                        | Room.Backstage -> WorldBackstageName
                        | Room.Bar -> WorldBarName
                        | Room.Lobby -> WorldLobbyName
                        | Room.MasteringRoom -> WorldMasteringRoomName
                        | Room.RecordingRoom -> WorldRecordingRoomName
                        | Room.RehearsalRoom -> WorldRehearsalRoomName
                        | Room.Stage -> WorldStageName
                        |> WorldText
                        |> I18n.translate
                    | ResolvedOutsideCoordinates _ ->
                        // Character is outside, show connected place name.
                        roomCoords.Place.Name |> I18n.constant
                | ResolvedOutsideCoordinates coords ->
                    coords.Node.Name |> I18n.constant
                |> Tuple.two direction
                |> Some
            | _ -> None)

    if not (List.isEmpty directions) then
        directions
        |> CommandLookEntrances
        |> CommandText
        |> I18n.translate
        |> showMessage

    interactions
    |> Interaction.chooseFreeRoam (fun interaction ->
        match interaction with
        | FreeRoamInteraction.GoOut (_, coordinates) ->
            Some coordinates.Node.Name
        | _ -> None)
    |> List.tryHead
    |> Option.iter (
        I18n.constant
        >> CommandLookExit
        >> CommandText
        >> I18n.translate
        >> showMessage
    )

let private commandsFromInteractions interactions =
    interactions
    |> List.collect (fun interactionWithState ->
        match interactionWithState.Interaction with
        | Interaction.Concert concertInteraction ->
            match concertInteraction with
            | ConcertInteraction.AdjustDrums _ ->
                [ Command.message
                      "adjust drums"
                      CommandAdjustDrumsDescription
                      ConcertAdjustDrumsMessage ]
            | ConcertInteraction.BassSolo ongoingConcert ->
                [ BassSoloCommand.create ongoingConcert ]
            | ConcertInteraction.DoEncore ongoingConcert ->
                [ DoEncoreCommand.create ongoingConcert ]
            | ConcertInteraction.DrumSolo ongoingConcert ->
                [ DrumSoloCommand.create ongoingConcert ]
            | ConcertInteraction.FinishConcert ongoingConcert ->
                [ FinishConcertCommand.create ongoingConcert ]
            | ConcertInteraction.GetOffStage ongoingConcert ->
                [ GetOffStageCommand.create ongoingConcert ]
            | ConcertInteraction.GiveSpeech ongoingConcert ->
                [ GiveSpeechCommand.create ongoingConcert ]
            | ConcertInteraction.GreetAudience ongoingConcert ->
                [ GreetAudienceCommand.create ongoingConcert ]
            | ConcertInteraction.GuitarSolo ongoingConcert ->
                [ GuitarSoloCommand.create ongoingConcert ]
            | ConcertInteraction.FaceBand _ ->
                [ Command.message
                      "face band"
                      CommandFaceBandDescription
                      ConcertFaceBandMessage ]
            | ConcertInteraction.FaceCrowd _ ->
                [ Command.message
                      "face crowd"
                      CommandFaceCrowdDescription
                      ConcertFaceCrowdMessage ]
            | ConcertInteraction.MakeCrowdSing ongoingConcert ->
                [ MakeCrowdSingCommand.create ongoingConcert ]
            | ConcertInteraction.PlaySong ongoingConcert ->
                [ PlaySongCommands.createPlaySong ongoingConcert
                  PlaySongCommands.createDedicateSong ongoingConcert ]
            | ConcertInteraction.PutMicOnStand _ ->
                [ Command.message
                      "put mic on stand"
                      CommandPutMicOnStandDescription
                      ConcertPutMicOnStandMessage ]
            | ConcertInteraction.SpinDrumsticks ongoingConcert ->
                [ SpinDrumsticksCommand.create ongoingConcert ]
            | ConcertInteraction.TakeMic _ ->
                [ Command.message
                      "take mic"
                      CommandTakeMicDescription
                      ConcertTakeMicMessage ]
            | ConcertInteraction.TuneInstrument ongoingConcert ->
                [ TuneInstrumentCommand.create ongoingConcert ]
        | Interaction.FreeRoam freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.GoOut (exit, _) -> [ OutCommand.create exit ]
            | FreeRoamInteraction.Move (direction, nodeId) ->
                [ NavigationCommand.create direction nodeId ]
            | FreeRoamInteraction.Phone -> [ PhoneCommand.get ]
            | FreeRoamInteraction.Wait -> [ WaitCommand.get ]
        | Interaction.Rehearsal rehearsalInteraction ->
            match rehearsalInteraction with
            | RehearsalInteraction.ComposeNewSong -> [ ComposeSongCommand.get ]
            | RehearsalInteraction.DiscardSong unfinishedSongs ->
                [ DiscardSongCommand.create unfinishedSongs ]
            | RehearsalInteraction.FinishSong unfinishedSongs ->
                [ FinishSongCommand.create unfinishedSongs ]
            | RehearsalInteraction.FireMember bandMembers ->
                [ FireMemberCommand.create bandMembers ]
            | RehearsalInteraction.HireMember -> [ HireMemberCommand.get ]
            | RehearsalInteraction.ImproveSong unfinishedSongs ->
                [ ImproveSongCommand.create unfinishedSongs ]
            | RehearsalInteraction.ListMembers (bandMembers, pastMembers) ->
                [ ListMembersCommand.create bandMembers pastMembers ]
            | RehearsalInteraction.PracticeSong finishedSongs ->
                [ PracticeSongCommand.create finishedSongs ]
        | Interaction.Studio studioInteraction ->
            match studioInteraction with
            | StudioInteraction.CreateAlbum (studio, finishedSongs) ->
                [ CreateAlbumCommand.create studio finishedSongs ]
            | StudioInteraction.EditAlbumName unreleasedAlbums ->
                [ EditAlbumNameCommand.create unreleasedAlbums ]
            | StudioInteraction.ReleaseAlbum unreleasedAlbums ->
                [ ReleaseAlbumCommand.create unreleasedAlbums ]
        |> List.map (Tuple.two interactionWithState.State))
    |> List.map (fun (interactionState, command) ->
        match interactionState with
        | InteractionState.Enabled -> command
        | InteractionState.Disabled disabledReason ->
            Command.disable disabledReason command)

/// Creates the world scene, which displays information about the current place
/// where the character is located as well as allowing the actions available
/// as given by the simulation layer.
let worldScene () =
    lineBreak ()

    let character =
        Queries.Characters.playableCharacter (State.get ())

    let today =
        Queries.Calendar.today (State.get ())

    let currentDayMoment =
        Calendar.Query.dayMomentOf today

    let interactionsWithState =
        Interactions.Root.availableCurrently (State.get ())

    let currentPosition =
        State.get ()
        |> Queries.World.Common.currentPosition

    let situation =
        Queries.Situations.current (State.get ())

    descriptionFromCoordinates currentPosition.Content
    |> showMessage

    showRoomConnections interactionsWithState

    let promptText =
        match situation with
        | InConcert ongoingConcert ->
            ConcertActionPrompt(
                today,
                currentDayMoment,
                character.Status,
                ongoingConcert.Points
            )
            |> ConcertText
        | _ ->
            CommandCommonPrompt(today, currentDayMoment, character.Status)
            |> CommandText

    showCommandPrompt
        (promptText |> I18n.translate)
        (commandsFromInteractions interactionsWithState)
