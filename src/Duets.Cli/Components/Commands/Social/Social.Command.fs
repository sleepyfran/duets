namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Simulation.Social.Common

[<RequireQualifiedAccess>]
module SocialCommand =
    /// Creates a command that calls the given action and applies all the
    /// effects returned by the response, then returns the world scene.
    let create
        (args:
            {| Name: string
               Description: string
               Action: unit -> SocialActionResponse
               Handler: SocialActionResult -> unit |})
        =
        { Name = args.Name
          Description = args.Description
          Handler =
            fun _ ->
                let response = args.Action()
                response.Result |> args.Handler
                response.Effects |> Effect.applyMultiple

                Scene.World }
