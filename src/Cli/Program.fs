open Agents
open Cli.SceneIndex
open Cli.Components
open Cli.Scenes
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
let saveIfNeeded scene =
    if not (outOfGameplayScene scene) then
        Savegame.save ()
    else
        ()

let rec showScene scene =
    saveIfNeeded scene

    match scene with
    | Scene.MainMenu savegameState ->
        MainMenu.mainMenu savegameState |> showScene
    | Scene.CharacterCreator ->
        NewGame.CharacterCreator.characterCreator () |> showScene
    | Scene.BandCreator character ->
        NewGame.BandCreator.bandCreator character |> showScene
    | Scene.SkillEditor (character, characterMember, band) ->
        NewGame.SkillEditor.skillEditor character characterMember band
        |> showScene
    | Scene.WorldSelector (character, band, skills) ->
        NewGame.WorldSelector.worldSelector character band skills |> showScene
    | Scene.Phone -> Phone.Root.phoneScene () |> showScene
    | Scene.World ->
        World.worldScene World.WorldMode.IgnoreDescription |> showScene
    | Scene.WorldAfterMovement ->
        World.worldScene World.WorldMode.ShowDescription |> showScene
    | Scene.Exit exitMode ->
        match exitMode with
        | ExitMode.SaveGame -> Savegame.saveSync ()
        | _ -> ()

[<EntryPoint>]
let main _ =
    clearScreen ()

    // Set default culture to UK for sane defaults :)
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-UK")

    Savegame.load () |> Scene.MainMenu |> showScene

    0
