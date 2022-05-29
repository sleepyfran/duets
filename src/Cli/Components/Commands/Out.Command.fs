namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Navigation

[<RequireQualifiedAccess>]
module OutCommand =
    let create exitNodeId =
        { Name = "out"
          Description = I18n.translate (CommandText CommandOutDescription)
          Handler =
            (fun _ ->
                State.get ()
                |> Navigation.moveTo (Node exitNodeId)
                |> Result.switch Cli.Effect.apply Common.showEntranceError

                Scene.World) }
