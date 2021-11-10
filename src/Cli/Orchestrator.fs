module Orchestrator

open Aether
open Entities
open Cli.View.Actions
open Cli.DefaultCommands
open Cli.View.Scenes
open Cli.View.TextConstants
open Cli.View.Renderer
open Common
open Simulation.Queries

/// Returns the sequence of actions associated with a screen given its name.
let actionsFromScene state scene =
    match scene with
#if DEBUG
    | DeveloperRoom -> DeveloperRoom.developerRoom state
#endif
    | MainMenu savegameState -> MainMenu.mainMenu savegameState
    | CharacterCreator -> CharacterCreator.characterCreator ()
    | BandCreator character -> BandCreator.bandCreator character
    | RehearsalRoom -> RehearsalRoom.Root.rehearsalRoomScene ()
    | Management -> Management.Root.managementScene ()
    | Map -> Map.mapScene ()
    | Bank -> Bank.Root.bankScene state
    | Studio studio -> Studio.Root.studioScene state studio
    | Statistics -> Statistics.Root.statisticsScene ()
    | World -> World.worldScene ()

let actionsFromSubScene state subScene =
    match subScene with
    | SubScene.RehearsalRoomCompose -> RehearsalRoom.Compose.compose state
    | RehearsalRoomComposeSong ->
        RehearsalRoom.ComposeSong.composeSongScene state
    | RehearsalRoomImproveSong ->
        RehearsalRoom.ImproveSong.improveSongScene state
    | RehearsalRoomFinishSong -> RehearsalRoom.FinishSong.finishSongScene state
    | RehearsalRoomDiscardSong ->
        RehearsalRoom.DiscardSong.discardSongScene state
    | SubScene.ManagementHireMember -> Management.Hire.hireScene state
    | SubScene.ManagementFireMember -> Management.Fire.fireScene state
    | ManagementListMembers -> Management.MemberList.memberListScene state
    | BankTransfer (sender, receiver) ->
        Bank.Transfer.transferSubScene state sender receiver
    | StudioCreateRecord studio ->
        Studio.CreateRecord.createRecordSubscene state studio
    | SubScene.StudioContinueRecord studio ->
        Studio.ContinueRecord.continueRecordSubscene state studio
    | SubScene.StudioPromptToRelease (onCancel, studio, band, album) ->
        Studio.PromptToRelease.promptToReleaseAlbum
            onCancel
            state
            studio
            band
            album
    | StatisticsOfBand -> Statistics.Band.bandStatisticsSubScene state
    | StatisticsOfAlbums -> Statistics.Albums.albumsStatisticsSubScene state

let actionsFromEffect effect =
    match effect with
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        ImproveSongCanBeFurtherImproved(previousQuality, currentQuality)
        |> TextConstant
        |> Message
    | SkillImproved (character, Diff (before, after)) ->
        let (skill, previousLevel) = before
        let (_, currentLevel) = after

        CommonSkillImproved(
            character.Name,
            character.Gender,
            skill,
            previousLevel,
            currentLevel
        )
        |> TextConstant
        |> Message
    | MoneyTransferred (holder, transaction) ->
        BankTransferSuccess(holder, transaction)
        |> TextConstant
        |> Message
    | AlbumRecorded (_, UnreleasedAlbum album) ->
        StudioCreateAlbumRecorded album.Name
        |> TextConstant
        |> Message
    | AlbumRenamed (_, UnreleasedAlbum album) ->
        StudioContinueRecordAlbumRenamed album.Name
        |> TextConstant
        |> Message
    | AlbumReleased (_, releasedAlbum) ->
        StudioCommonAlbumReleased releasedAlbum.Album.Name
        |> TextConstant
        |> Message
    | _ -> NoOp

/// Determines whether the given scene is out of gameplay (main menu, creators,
/// etc.) or not.
let private outOfGameplayScene scene =
    match scene with
    | MainMenu _ -> true
    | CharacterCreator _ -> true
    | BandCreator _ -> true
    | _ -> false

/// Saves the game to the savegame file only if the screen is not the main menu,
/// character creator or band creator, which still have unreliable data or
/// might not have data at all.
let saveIfNeeded scene =
    if not (outOfGameplayScene scene) then
        Savegame.save ()
    else
        ()

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith chain =
    chain
    |> Seq.iter
        (fun action ->
            match action with
            | Separator -> separator ()
            | Prompt prompt -> renderPrompt prompt |> runWith
            | Message message -> renderMessage message
            | Figlet text -> renderFiglet text
            | ProgressBar content -> renderProgressBar content
            | Scene scene -> runScene (State.Root.get ()) scene
            | SceneAfterKey scene ->
                waitForInput
                <| TextConstant CommonPressKeyToContinue

                runScene (State.Root.get ()) scene
            | SubScene subScene ->
                subScene
                |> actionsFromSubScene (State.Root.get ())
                |> runWith
            | Effect effect ->
                Simulation.Galactus.runOne (State.Root.get ()) effect
                |> Seq.tap State.Root.apply
                |> Seq.tap Log.append
                |> Seq.map actionsFromEffect
                |> runWith
            | GameInfo version -> renderGameInfo version
            | NoOp -> ())

and renderPrompt prompt =
    match prompt.Content with
    | ChoicePrompt content ->
        match content with
        | MandatoryChoiceHandler content ->
            let choiceId =
                renderMandatoryPrompt prompt.Title content

            content.Choices
            |> choiceById choiceId
            |> content.Handler
        | OptionalChoiceHandler content ->
            renderOptionalPrompt prompt.Title content
            |> fun choiceId ->
                match choiceId with
                | "back" -> content.Handler Back
                | _ ->
                    content.Choices
                    |> choiceById choiceId
                    |> Choice
                    |> content.Handler
    | MultiChoicePrompt content ->
        let choiceId =
            renderMultiChoicePrompt prompt.Title content

        content.Choices
        |> choicesById choiceId
        |> content.Handler
    | ConfirmationPrompt handler ->
        renderConfirmationPrompt prompt.Title |> handler
    | NumberPrompt handler -> renderNumberPrompt prompt.Title |> handler
    | TextPrompt handler -> renderTextPrompt prompt.Title |> handler
    | LengthPrompt handler ->
        renderLengthPrompt prompt.Title
        |> fun length ->
            match Time.Length.parse length with
            | Ok length -> handler length
            | _ ->
                raise (
                    invalidOp
                        "The given input was not a correct length. This should've caught by the validator but apparently it didn't :)"
                )
    | CommandPrompt commands ->
        renderLineBreak ()
        renderMessage prompt.Title

        let commandsWithDefaults = commands @ [ exitCommand ]

        let commandsWithHelp =
            commandsWithDefaults
            @ [ createHelpCommand commandsWithDefaults ]

        /// Prompts for a command until a valid one is given, reporting an error
        /// when an invalid command is inputted.
        let rec promptForCommand () =
            renderTextPrompt (Literal "")
            |> String.split ' '
            |> List.ofArray
            |> fun commandWithArgs ->
                match commandWithArgs with
                | commandName :: args ->
                    runCommand
                        (seq { Prompt prompt })
                        commandsWithHelp
                        commandName
                        args
                | _ -> None
            |> Option.defaultWith promptForCommand

        promptForCommand ()

/// Saves the game, clears the screen and runs the next scene with a separator
/// on top.
and runScene state scene =
    saveIfNeeded scene
    clear ()
    separator ()
    showStatusBar state scene
    runWith (actionsFromScene state scene)

and showStatusBar state scene =
    if not (outOfGameplayScene scene) then
        statusBarContent state
        |> TextConstant
        |> renderMessage

        separator ()

and statusBarContent state =
    let date = Calendar.today state
    let dayMoment = Calendar.dayMomentOf date

    let characterBalance =
        Characters.playableCharacter state
        |> Optic.get Lenses.Character.id_
        |> Character
        |> Bank.balanceOf state

    let bandBalance =
        Bands.currentBand state
        |> Optic.get Lenses.Band.id_
        |> Band
        |> Bank.balanceOf state

    CommonStatusBar(date, dayMoment, characterBalance, bandBalance)

and runCommand currentChain availableCommands commandName args =
    availableCommands
    |> List.tryFind (fun command -> command.Name = commandName)
    |> fun command ->
        match command with
        | Some command ->
            // If the handler has navigation then just return the chain itself,
            // otherwise combine it with the current chain to prevent the orchestrator
            // from running out of actions prematurely.
            match command.Handler with
            | HandlerWithNavigation handler -> handler args |> Some
            | HandlerWithoutNavigation handler ->
                Seq.append (handler args) currentChain |> Some
        | None ->
            renderMessage (TextConstant CommonInvalidCommand)
            None
