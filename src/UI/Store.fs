[<RequireQualifiedAccess>]
module UI.Store

open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open UI.SceneIndex

/// Defines all the UI-specific state that the game needs to function.
type SharedStore = {
    CurrentScene: IWritable<Scene>
    ViewStack: IWritable<IView list>
}

let shared = {
    CurrentScene = new State<Scene>(Scene.MainMenu)
    ViewStack = new State<IView list>([])
}
