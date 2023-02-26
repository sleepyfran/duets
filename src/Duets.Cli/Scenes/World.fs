module Duets.Cli.Scenes.World

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let private commandsFromInteractions interactions =
    interactions
    |> List.collect (fun interactionWithMetadata ->
        match interactionWithMetadata.Interaction with
        | Interaction.Airport airportInteraction ->
            match airportInteraction with
            | AirportInteraction.BoardAirplane flight ->
                [ BoardPlaneCommand.create flight ]
            | AirportInteraction.WaitUntilLanding flight ->
                [ WaitForLandingCommand.create flight ]
        | Interaction.Career careerInteraction ->
            match careerInteraction with
            | CareerInteraction.Work job -> [ WorkCommand.create job ]
        | Interaction.Concert concertInteraction ->
            match concertInteraction with
            | ConcertInteraction.StartConcert placeId ->
                [ StartConcertCommand.create placeId ]
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
            | FreeRoamInteraction.Look items -> [ LookCommand.create items ]
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
            | RehearsalInteraction.ListSongs (unfinishedSongs, finishedSongs) ->
                [ ListSongsCommand.create unfinishedSongs finishedSongs ]
            | RehearsalInteraction.PracticeSong finishedSongs ->
                [ PracticeSongCommand.create finishedSongs ]
        | Interaction.Shop shopInteraction ->
            match shopInteraction with
            | ShopInteraction.Order shop -> [ OrderCommand.create shop ]
            | ShopInteraction.SeeMenu shop -> [ SeeMenuCommand.create shop ]
        | Interaction.Studio studioInteraction ->
            match studioInteraction with
            | StudioInteraction.CreateAlbum (studio, finishedSongs) ->
                [ CreateAlbumCommand.create studio finishedSongs ]
            | StudioInteraction.AddSongToAlbum (studio, albums, finishedSongs) ->
                [ RecordSongCommand.create studio albums finishedSongs ]
            | StudioInteraction.EditAlbumName unreleasedAlbums ->
                [ EditAlbumNameCommand.create unreleasedAlbums ]
            | StudioInteraction.ListUnreleasedAlbums unreleasedAlbums ->
                [ ListUnreleasedAlbumsCommand.create unreleasedAlbums ]
            | StudioInteraction.ReleaseAlbum unreleasedAlbums ->
                [ ReleaseAlbumCommand.create unreleasedAlbums ]
        |> List.map (Tuple.two interactionWithMetadata))
    |> List.map (fun (interactionWithMetadata, command) ->
        match interactionWithMetadata.State with
        | InteractionState.Enabled -> command
        | InteractionState.Disabled disabledReason ->
            Command.disable disabledReason command
        |> Tuple.two interactionWithMetadata.TimeAdvance)

let private filterAttributesForInfoBar =
    List.choose (fun (attr, amount) ->
        match attr with
        | CharacterAttribute.Energy
        | CharacterAttribute.Health
        | CharacterAttribute.Mood
        | CharacterAttribute.Hunger when amount <= 50 -> Some(attr, amount)
        | CharacterAttribute.Drunkenness when amount > 0 -> Some(attr, amount)
        | _ -> None)

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
        |> filterAttributesForInfoBar

    let promptText =
        match situation with
        | Airport (Flying flight) ->
            Airport.planeActionPrompt
                today
                currentDayMoment
                characterAttributes
                flight
        | Concert (InConcert ongoingConcert)
        | Concert (InBackstage (Some ongoingConcert)) ->
            Concert.actionPrompt
                today
                currentDayMoment
                characterAttributes
                ongoingConcert.Points
        | _ -> Command.commonPrompt today currentDayMoment characterAttributes

    let commandsWithMetadata =
        commandsFromInteractions interactionsWithState
        @ [ (0<dayMoments>, ExitCommand.get); (0<dayMoments>, MeCommand.get) ]

    commandsWithMetadata
    |> List.map snd
    |> (@) [ HelpCommand.create commandsWithMetadata ]
    |> showCommandPrompt promptText
