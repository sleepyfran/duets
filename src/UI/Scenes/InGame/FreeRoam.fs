module rec UI.Scenes.InGame.FreeRoam

open Avalonia.Controls.Documents
open Avalonia.FuncUI.DSL
open Entities
open Simulation.Navigation
open UI.Scenes.InGame.Types

let runFreeRoamInteraction state interaction =
    match interaction with
    | FreeRoamInteraction.GoOut _ -> unimplemented ()
    | FreeRoamInteraction.Look _ -> unimplemented ()
    | FreeRoamInteraction.Move (_, coordinates) ->
        let navigationResult =
            state |> Navigation.moveTo coordinates

        match navigationResult with
        | Ok effect -> Effects [ effect ]
        | Error error -> createEntranceError error |> Message
    | FreeRoamInteraction.Wait -> unimplemented ()
    | _ -> failwith "Not supported"

let private unimplemented () = failwith "Oops"

let private createEntranceError error =
    match error with
    | EntranceError.CannotEnterBackstageOutsideConcert -> [
        Run.create [
            Run.text
                "Initially the people in the bar were looking weirdly at you thinking what were you doing in there. Then the "
        ]
        Bold.simple "bouncer"
        Run.create [
            Run.text " came and kicked you out with a warning: "
        ]
        Bold.simple
            "\"Do not get into the stage again if you're not part of the band playing\""
      ]
    | EntranceError.CannotEnterStageOutsideConcert -> [
        Run.create [
            Run.text "You tried to sneak into the backstage, but the "
        ]
        Bold.simple "bouncers"
        Run.create [
            Run.text
                " catch you as soon as you enter and kicked you out with a warning."
        ]
      ]
