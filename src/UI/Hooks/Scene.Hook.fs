module UI.Hooks.Scene

open Agents
open Avalonia.FuncUI
open UI
open UI.SceneIndex

type IComponentContext with
    /// Provides a function that can set the current scene to the specified
    /// one and performs any side-effect related to that (for example, saving
    /// the game).
    member this.useSceneSwitcher() : Scene -> unit =
        fun scene ->
            match scene with
            | Scene.InGame _ -> Savegame.save ()
            | _ -> ()

            Store.shared.CurrentScene.Set scene
