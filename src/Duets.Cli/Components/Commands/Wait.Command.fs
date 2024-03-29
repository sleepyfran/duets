namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module WaitCommand =
    /// Command which passes time without doing any other change.
    let get =
        { Name = "wait"
          Description = Command.waitDescription
          Handler =
            (fun _ ->
                Wait 1<dayMoments> |> Effect.apply
                Scene.World) }
