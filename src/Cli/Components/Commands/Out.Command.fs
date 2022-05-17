namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation

[<RequireQualifiedAccess>]
module OutCommand =
    let create exitNodeId =
        { Name = "out"
          Description = I18n.translate (CommandText CommandOutDescription)
          Handler =
            (fun _ ->
                State.get ()
                |> World.Navigation.moveTo (Node exitNodeId)
                |> Result.switch Cli.Effect.apply Common.showEntranceError

                Scene.World) }
