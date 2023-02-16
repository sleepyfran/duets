namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
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
            (fun args ->
                match args with
                | timeAmount :: _ ->
                    match System.Int32.TryParse timeAmount with
                    | true, num -> num * 1<dayMoments> |> Wait |> Effect.apply
                    | _ -> showMessage (Command.waitInvalidTimes timeAmount)
                | _ -> Wait 1<dayMoments> |> Effect.apply

                Scene.World) }
