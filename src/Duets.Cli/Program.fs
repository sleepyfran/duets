open Duets.Agents
open Duets.Cli.SceneIndex
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Scenes
open System.Globalization
open System.Threading

/// Determines whether the given scene is out of gameplay (main menu, creators,
/// etc.) or not.
let private outOfGameplayScene scene =
    match scene with
    | Scene.MainMenu _
    | Scene.CharacterCreator _
    | Scene.BandCreator _
    | Scene.SkillEditor _
    | Scene.WorldSelector _ -> true
    | _ -> false

/// Saves the game to the savegame file only if the screen is not the main menu,
/// character creator or band creator, which still have unreliable data or
/// might not have data at all.
let saveIfNeeded skipSaving scene =
    if not (outOfGameplayScene scene) && not skipSaving then
        Savegame.save ()
    else
        ()

let rec showScene skipSaving scene =
    saveIfNeeded skipSaving scene

    match scene with
    | Scene.MainMenu savegameState ->
        MainMenu.mainMenu savegameState |> showScene skipSaving
    | Scene.CharacterCreator ->
        NewGame.CharacterCreator.characterCreator () |> showScene skipSaving
    | Scene.BandCreator character ->
        NewGame.BandCreator.bandCreator character |> showScene skipSaving
    | Scene.SkillEditor(character, characterMember, band) ->
        NewGame.SkillEditor.skillEditor character characterMember band
        |> showScene skipSaving
    | Scene.WorldSelector(character, band, skills) ->
        NewGame.WorldSelector.worldSelector character band skills
        |> showScene skipSaving
    | Scene.Phone -> Phone.Root.phoneScene () |> showScene skipSaving
    | Scene.World ->
        World.worldScene World.WorldMode.IgnoreDescription
        |> showScene skipSaving
    | Scene.WorldAfterMovement ->
        World.worldScene World.WorldMode.ShowDescription |> showScene skipSaving
    | Scene.Exit exitMode ->
        match exitMode with
        | ExitMode.SaveGame when not skipSaving -> Savegame.saveSync ()
        | _ -> ()

let private parseNoSavingArg args =
    args |> Array.tryHead |> Option.exists (fun arg -> arg = "--no-saving")

[<EntryPoint>]
let main args =
    let skipSaving = parseNoSavingArg args

    if skipSaving then
        Styles.danger
            "--no-saving arg detected, all changes during gameplay won't be persisted!"
        |> showMessage

    clearScreen ()

    Stats.startTrackingTime ()

    // Set default culture to UK for sane defaults :)
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-UK")

    Savegame.load () |> Scene.MainMenu |> showScene skipSaving

    Stats.stopTrackingAndSave ()

    0
