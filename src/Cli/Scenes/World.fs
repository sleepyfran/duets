module Cli.Scenes.World

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Components.Commands.Map
open Cli.Text
open Common
open Entities
open Simulation

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
                [ PlaySongCommands.createPlaySong ongoingConcert ]
            | ConcertInteraction.DedicateSong ongoingConcert ->
                [ PlaySongCommands.createDedicateSong ongoingConcert ]
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
        | Interaction.Item itemInteraction ->
            match itemInteraction with
            | ItemInteraction.Consumable ConsumableItemInteraction.Drink ->
                [ ConsumeCommands.drink ]
            | ItemInteraction.Consumable ConsumableItemInteraction.Eat ->
                [ ConsumeCommands.eat ]
            | ItemInteraction.Interactive InteractiveItemInteraction.Sleep ->
                [ InteractiveCommand.sleep ]
            | ItemInteraction.Interactive InteractiveItemInteraction.Play ->
                [ InteractiveCommand.play ]
            | ItemInteraction.Interactive InteractiveItemInteraction.Watch ->
                [ InteractiveCommand.watch ]
        | Interaction.FreeRoam freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.Inventory inventory ->
                [ InventoryCommand.create inventory ]
            | FreeRoamInteraction.Map -> [ MapCommand.get ]
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

type WorldMode =
    | IgnoreDescription
    | ShowDescription

/// Creates the world scene, which displays information about the current place
/// where the character is located as well as allowing the actions available
/// as given by the simulation layer.
let worldScene mode =
    lineBreak ()

    let today = Queries.Calendar.today (State.get ())

    let currentDayMoment = Calendar.Query.dayMomentOf today

    let interactionsWithState =
        Queries.Interactions.availableCurrently (State.get ())

    let currentPlace = State.get () |> Queries.World.currentPlace

    let situation = Queries.Situations.current (State.get ())

    match mode with
    | ShowDescription -> World.placeDescription currentPlace |> showMessage
    | IgnoreDescription -> ()

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
