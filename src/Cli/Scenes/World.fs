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

let private showRoomConnections interactions =
    interactions
    |> Interaction.chooseFreeRoam (fun interaction ->
        match interaction with
        | FreeRoamInteraction.Move (direction, nodeCoordinates) ->
            let coords =
                Queries.World.Common.coordinates (State.get ()) nodeCoordinates

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
    |> List.collect (fun interaction ->
        match interaction with
        | Interaction.FreeRoam freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.GoOut (exit, _) -> [ OutCommand.create exit ]
            | FreeRoamInteraction.Move (direction, nodeId) ->
                [ NavigationCommand.create direction nodeId ]
            | FreeRoamInteraction.Phone -> [ PhoneCommand.get ]
            | FreeRoamInteraction.Wait -> [ WaitCommand.get ]
        | Interaction.Concert concertInteraction ->
            match concertInteraction with
            | ConcertInteraction.DoEncore ongoingConcert ->
                [ DoEncoreCommand.create ongoingConcert ]
            | ConcertInteraction.FinishConcert ongoingConcert ->
                [ FinishConcertCommand.create ongoingConcert ]
            | ConcertInteraction.GiveSpeech ongoingConcert ->
                [ GiveSpeechCommand.create ongoingConcert ]
            | ConcertInteraction.GreetAudience ongoingConcert ->
                [ GreetAudienceCommand.create ongoingConcert ]
            | ConcertInteraction.PlaySong ongoingConcert ->
                [ PlaySongCommands.createPlaySong ongoingConcert
                  PlaySongCommands.createDedicateSong ongoingConcert ]
            | ConcertInteraction.GetOffStage ongoingConcert ->
                [ GetOffStageCommand.create ongoingConcert ]
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
        | _ -> [])

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

    let interactions =
        Interactions.Root.availableCurrently (State.get ())

    let currentPosition =
        State.get ()
        |> Queries.World.Common.currentPosition

    descriptionFromCoordinates currentPosition.Content
    |> showMessage

    // TODO: Take disabled also into consideration. Show disabled in red.
    showRoomConnections interactions.Enabled

    showCommandPrompt
        (CommandCommonPrompt(today, currentDayMoment, character.Status)
         |> CommandText
         |> I18n.translate)
        (commandsFromInteractions
            interactions.Enabled (* TODO: Take disabled also into consideration. *) )
