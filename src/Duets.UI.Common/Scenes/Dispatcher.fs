module Duets.UI.Common.Scenes.Dispatcher

open Duets.Agents
open Duets.Entities
open Duets.UI.Common

let rec run (displayLabel: InteractionWithMetadata -> string) (current: Navigate) : Scene<unit> =
    fun renderer ->
        async {
            do! clear renderer

            match current with
            | Navigate.Exit -> System.Environment.Exit(0)
            | Navigate.MainMenu ->
                let! next = (MainMenu.scene ()) renderer
                do! (run displayLabel next) renderer
            | Navigate.NewGame ->
                let! next = (NewGame.scene ()) renderer
                Savegame.save ()
                do! (run displayLabel next) renderer
            | Navigate.InGame ->
                let! next = (InGame.scene displayLabel) renderer
                do! (run displayLabel next) renderer
        }
