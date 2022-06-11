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
          Description = Command.outDescription
          Handler =
            (fun _ ->
                State.get ()
                |> Navigation.moveTo (Node exitNodeId)
                |> Result.switch Cli.Effect.apply Common.showEntranceError

                Scene.World) }
