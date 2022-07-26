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
        | RoomType.Backstage -> World.backstageDescription coordinates.Place
        | RoomType.Bar _ -> World.barDescription coordinates.Place
        | RoomType.Bedroom -> World.bedroomDescription
        | RoomType.Kitchen -> World.kitchenDescription
        | RoomType.LivingRoom -> World.livingRoomDescription
        | RoomType.Lobby -> World.lobbyDescription coordinates.Place
        | RoomType.MasteringRoom -> World.masteringRoomDescription
        | RoomType.RecordingRoom -> World.recordingRoomDescription
        | RoomType.RehearsalRoom -> World.rehearsalRoomDescription
        | RoomType.Stage -> World.stageDescription coordinates.Place
    | ResolvedOutsideCoordinates coordinates ->
        (coordinates.Node.Name, coordinates.Node.Descriptors)
        ||> match coordinates.Node.Type with
            | OutsideNodeType.Boulevard -> World.streetDescription
            | OutsideNodeType.Street -> World.boulevardDescription
            | OutsideNodeType.Square -> World.squareDescription

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
                        | RoomType.Backstage -> World.backstageName
                        | RoomType.Bar _ -> World.barName
                        | RoomType.Bedroom -> World.bedroomName
                        | RoomType.Kitchen -> World.kitchenName
                        | RoomType.LivingRoom -> World.livingRoomName
                        | RoomType.Lobby -> World.lobbyName
                        | RoomType.MasteringRoom -> World.masteringRoomName
                        | RoomType.RecordingRoom -> World.recordingRoomName
                        | RoomType.RehearsalRoom -> World.rehearsalRoomName
                        | RoomType.Stage -> World.stageName
                    | ResolvedOutsideCoordinates _ ->
                        // Character is outside, show connected place name.
                        roomCoords.Place.Name
                | ResolvedOutsideCoordinates coords -> coords.Node.Name
                |> Tuple.two direction
                |> Some
            | _ -> None)

    if not (List.isEmpty directions) then
        directions |> Command.lookEntrances |> showMessage

    interactions
    |> Interaction.chooseFreeRoam (fun interaction ->
        match interaction with
        | FreeRoamInteraction.GoOut (_, coordinates) ->
            Some coordinates.Node.Name
        | _ -> None)
    |> List.tryHead
    |> Option.iter (Command.lookExit >> showMessage)

let private commandsFromInteractions interactions =
    interactions
    |> List.collect (fun interactionWithState ->
        match interactionWithState.Interaction with
        | Interaction.Concert concertInteraction ->
            match concertInteraction with
            | ConcertInteraction.AdjustDrums _ ->
                [ Command.message
                      "adjust drums"
                      Command.adjustDrumsDescription
                      Concert.adjustDrumsMessage ]
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
                      Command.faceBandDescription
                      Concert.faceBandMessage ]
            | ConcertInteraction.FaceCrowd _ ->
                [ Command.message
                      "face crowd"
                      Command.faceCrowdDescription
                      Concert.faceCrowdMessage ]
            | ConcertInteraction.MakeCrowdSing ongoingConcert ->
                [ MakeCrowdSingCommand.create ongoingConcert ]
            | ConcertInteraction.PlaySong ongoingConcert ->
                [ PlaySongCommands.createPlaySong ongoingConcert
                  PlaySongCommands.createDedicateSong ongoingConcert ]
            | ConcertInteraction.PutMicOnStand _ ->
                [ Command.message
                      "put mic on stand"
                      Command.putMicOnStandDescription
                      Concert.putMicOnStandMessage ]
            | ConcertInteraction.SpinDrumsticks ongoingConcert ->
                [ SpinDrumsticksCommand.create ongoingConcert ]
            | ConcertInteraction.TakeMic _ ->
                [ Command.message
                      "take mic"
                      Command.takeMicDescription
                      Concert.takeMicMessage ]
            | ConcertInteraction.TuneInstrument ongoingConcert ->
                [ TuneInstrumentCommand.create ongoingConcert ]
        | Interaction.Home homeInteraction ->
            match homeInteraction with
            | HomeInteraction.Eat ->
                [ Command.interaction
                      "eat"
                      Command.eatDescription
                      (Interactions.Home.eat (State.get ()))
                      Interaction.eatResult ]
            | HomeInteraction.Sleep ->
                [ Command.interaction
                      "sleep"
                      Command.sleepDescription
                      // TODO: Allow player to choose how many hours they want to sleep.
                      (Interactions.Home.sleep (State.get ()) 8)
                      Interaction.sleepResult ]
            | HomeInteraction.PlayXbox ->
                [ Command.interaction
                      "play xbox"
                      Command.playXboxDescription
                      (Interactions.Home.playXbox (State.get ()))
                      Interaction.playXboxResult ]
            | HomeInteraction.WatchTv ->
                [ Command.interaction
                      "watch tv"
                      Command.watchTvDescription
                      (Interactions.Home.watchTv (State.get ()))
                      Interaction.watchTvResult ]
        | Interaction.Item itemInteraction ->
            match itemInteraction with
            | ItemInteraction.Drink -> [ DrinkCommand.get ]
            | ItemInteraction.Eat -> []
        | Interaction.FreeRoam freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.GoOut (exit, _) -> [ OutCommand.create exit ]
            | FreeRoamInteraction.Inventory inventory ->
                [ InventoryCommand.create inventory ]
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
        | Interaction.Bar shopInteraction ->
            match shopInteraction with
            | BarInteraction.Order shop -> [ OrderCommand.create shop ]
            | BarInteraction.SeeMenu shop -> [ SeeMenuCommand.create shop ]
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

    let mood, health, energy, fame =
        Queries.Characters.playableCharacterAttribute4
            (State.get ())
            CharacterAttribute.Mood
            CharacterAttribute.Health
            CharacterAttribute.Energy
            CharacterAttribute.Fame

    let today =
        Queries.Calendar.today (State.get ())

    let currentDayMoment =
        Calendar.Query.dayMomentOf today

    let interactionsWithState =
        Queries.Interactions.availableCurrently (State.get ())

    let currentPosition =
        State.get ()
        |> Queries.World.Common.currentPosition

    let situation =
        Queries.Situations.current (State.get ())

    descriptionFromCoordinates currentPosition.Content
    |> showMessage

    showRoomConnections interactionsWithState

    let characterAttributes =
        Queries.Characters.allPlayableCharacterAttributes (State.get ())

    let promptText =
        match situation with
        | InConcert ongoingConcert ->
            Concert.actionPrompt
                today
                currentDayMoment
                characterAttributes
                ongoingConcert.Points
        | _ -> Command.commonPrompt today currentDayMoment characterAttributes

    showCommandPrompt
        promptText
        (commandsFromInteractions interactionsWithState)
