[<RequireQualifiedAccess>]
module UI.Store

open Avalonia.FuncUI
open UI.SceneIndex

/// Defines all the UI-specific state that the game needs to function.
type SharedStore = { CurrentScene: IWritable<Scene> }

let shared =
    {
        CurrentScene = new State<Scene>(Scene.MainMenu)
    }
