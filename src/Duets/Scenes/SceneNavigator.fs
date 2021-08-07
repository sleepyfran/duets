module Duets.Scenes.Navigator

open Nez

open Duets.Scenes.BandCreator
open Duets.Scenes.CharacterCreator
open Duets.Scenes.Game
open Duets.Scenes.MainMenu
open Duets.Types

type SceneNavigator() =
    member private this.SceneFromScreen screen =
        match screen with
        | MainMenu -> MainMenuScene(this) :> Scene
        | CharacterCreator -> CharacterCreatorScene(this) :> Scene
        | BandCreator character -> BandCreatorScene(this, character) :> Scene
        | Game -> GameScene(this) :> Scene

    interface INavigator with
        member this.Navigate screen =
            Core.Scene <- this.SceneFromScreen screen

        member this.NavigateWithTransition screen =
            FadeTransition(fun () -> this.SceneFromScreen screen)
            |> Core.StartSceneTransition
            |> ignore
