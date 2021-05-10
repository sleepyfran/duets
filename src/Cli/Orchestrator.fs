module Orchestrator

open System
open Cli.View.Actions
open Cli.View.Scenes
open Cli.View.TextConstants
open Cli.View.Renderer

/// Returns the sequence of actions associated with a screen given its name.
let actionsFromScene scene =
    match scene with
    | MainMenu savegameState -> MainMenu.mainMenu savegameState
    | CharacterCreator -> CharacterCreator.characterCreator ()
    | BandCreator character -> BandCreator.bandCreator character
    | RehearsalRoom -> RehearsalRoom.Root.rehearsalRoomScene ()
    | Management -> Management.Root.managementScene ()

let actionsFromSubScene state subScene =
    match subScene with
    | SubScene.RehearsalRoomCompose -> RehearsalRoom.Compose.compose state
    | SubScene.RehearsalRoomComposeSong ->
        RehearsalRoom.ComposeSong.composeSongScene state
    | SubScene.RehearsalRoomImproveSong ->
        RehearsalRoom.ImproveSong.improveSongScene state
    | SubScene.RehearsalRoomFinishSong ->
        RehearsalRoom.FinishSong.finishSongScene state
    | SubScene.RehearsalRoomDiscardSong ->
        RehearsalRoom.DiscardSong.discardSongScene state
    | SubScene.ManagementHireMember -> Management.Hire.hireScene state
    | SubScene.ManagementFireMember -> Management.Fire.fireScene state
    | SubScene.ManagementListMembers ->
        Management.MemberList.memberListScene state

/// Saves the game to the savegame file only if the screen is not the main menu,
/// character creator or band creator, which still have unreliable data or
/// might not have data at all.
let saveIfNeeded scene =
    match scene with
    | MainMenu _ -> ()
    | CharacterCreator _ -> ()
    | BandCreator _ -> ()
    | _ -> Savegame.save ()

/// Given a renderer, a state and a chain of actions, recursively renders all
/// the actions in the chain and applies any effects that come with them.
let rec runWith chain =
    chain
    |> Seq.iter
        (fun action ->
            match action with
            | Prompt prompt ->
                renderPrompt prompt
                |> fun input ->
                    match prompt.Content with
                    | ChoicePrompt content ->
                        match content with
                        | MandatoryChoiceHandler content ->
                            content.Choices
                            |> choiceById input
                            |> content.Handler
                        | OptionalChoiceHandler content ->
                            match input with
                            | "back" -> content.Handler Back
                            | _ ->
                                content.Choices
                                |> choiceById input
                                |> Choice
                                |> content.Handler
                    | ConfirmationPrompt handler ->
                        handler (input |> Convert.ToBoolean)
                    | NumberPrompt handler -> handler (input |> int)
                    | TextPrompt handler -> handler input
                    |> runWith
            | Message message -> renderMessage message
            | Figlet text -> renderFiglet text
            | ProgressBar content -> renderProgressBar content
            | Scene scene -> runScene scene
            | SceneAfterKey scene ->
                waitForInput
                <| TextConstant CommonPressKeyToContinue

                runScene scene
            | SubScene subScene ->
                subScene
                |> actionsFromSubScene (State.Root.get ())
                |> runWith
            | Effect effect -> State.Root.apply effect
            | GameInfo version -> renderGameInfo version
            | NoOp -> ())

/// Saves the game, clears the screen and runs the next scene with a separator
/// on top.
and runScene scene =
    saveIfNeeded scene
    clear ()
    separator ()
    runWith (actionsFromScene scene)
