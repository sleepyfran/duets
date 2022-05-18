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
    | Scene.MainMenu _ -> true
    | Scene.CharacterCreator _ -> true
    | Scene.BandCreator _ -> true
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
        CharacterCreator.characterCreator () |> showScene
    | Scene.BandCreator character ->
        BandCreator.bandCreator character |> showScene
    | Scene.Phone -> Phone.Root.phoneScene () |> showScene
    | Scene.World -> World.worldScene () |> showScene
    | Scene.Exit ->
        Savegame.saveSync ()

        ()

[<EntryPoint>]
let main _ =
    clearScreen ()

    // Set default culture to UK for sane defaults :)
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-UK")

    Savegame.load () |> Scene.MainMenu |> showScene

    0
