module Orchestrator

open Agents
open Aether
open Cli.View.Commands
open Entities
open Cli.View.Actions
open Cli.View.Scenes
open Cli.View.Text
open Cli.View.Renderer
open Common
open Simulation
open Simulation.Queries
open System

/// Returns the sequence of actions associated with a screen given its name.
let actionsFromScene scene =
    match scene with
    | MainMenu savegameState -> MainMenu.mainMenu savegameState
    | CharacterCreator -> CharacterCreator.characterCreator ()
    | BandCreator character -> BandCreator.bandCreator character
    | Management -> Management.Root.managementScene ()
    | Phone -> Phone.Root.phoneScene ()
    | World -> World.worldScene ()

let actionsFromEffect effect =
    match effect with
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        ImproveSongCanBeFurtherImproved(previousQuality, currentQuality)
        |> RehearsalSpaceText
        |> I18n.translate
        |> Message
    | SongPracticed (_, (FinishedSong song, _)) ->
        PracticeSongImproved(song.Name, song.Practice)
        |> RehearsalSpaceText
        |> I18n.translate
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
        |> CommonText
        |> I18n.translate
        |> Message
    | MoneyTransferred (holder, transaction) ->
        BankAppTransferSuccess(holder, transaction)
        |> PhoneText
        |> I18n.translate
        |> Message
    | AlbumRecorded (_, UnreleasedAlbum album) ->
        StudioCreateAlbumRecorded album.Name
        |> StudioText
        |> I18n.translate
        |> Message
    | AlbumRenamed (_, UnreleasedAlbum album) ->
        StudioContinueRecordAlbumRenamed album.Name
        |> StudioText
        |> I18n.translate
        |> Message
    | AlbumReleased (_, releasedAlbum) ->
        StudioCommonAlbumReleased releasedAlbum.Album.Name
        |> StudioText
        |> I18n.translate
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
            | Separator -> renderSeparator ()
            | Prompt prompt -> renderPrompt prompt |> runWith
            | Message message -> renderMessage message
            | Figlet text -> renderFiglet text
            | ProgressBar content -> renderProgressBar content
            | BarChart items -> renderBarChart items
            | Scene scene -> runScene scene
            | Effect effect ->
                let effects, state = Simulation.tick (State.get ()) effect

                State.set state

                effects
                |> Seq.tap Log.append
                |> Seq.map actionsFromEffect
                |> runWith
            | GameInfo version -> renderGameInfo version
            | NewLine -> renderLineBreak ()
            | ClearScreen -> clear ()
            | Exit -> Environment.Exit(0)
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
            |> Pipe.tap renderSelection
            |> content.Handler
        | OptionalChoiceHandler content ->
            renderOptionalPrompt prompt.Title content
            |> fun choiceId ->
                match choiceId with
                | "back" -> content.Handler Back
                | _ ->
                    content.Choices
                    |> choiceById choiceId
                    |> Pipe.tap renderSelection
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

        let commandsWithDefaults =
            commands @ [ PhoneCommand.get; ExitCommand.get ]

        let commandsWithHelp =
            commandsWithDefaults
            @ [ HelpCommand.create commandsWithDefaults ]

        /// Prompts for a command until a valid one is given, reporting an error
        /// when an invalid command is inputted.
        let rec promptForCommand () =
            renderTextPrompt (Literal ">")
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
and runScene scene =
    saveIfNeeded scene
    renderLineBreak ()
    runWith (actionsFromScene scene)

/// Attempts to run a command from the given list and either returns the result
/// of the command or attaches the result to the current chain.
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
            I18n.translate (CommonText CommonInvalidCommand)
            |> renderMessage

            None
