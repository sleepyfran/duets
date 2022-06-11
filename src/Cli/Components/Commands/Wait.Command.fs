namespace Cli.Components.Commands

open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

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
                    | true, num -> Wait num |> Effect.apply
                    | _ -> showMessage (Command.waitInvalidTimes timeAmount)
                | _ -> Wait 1 |> Effect.apply

                Scene.World) }
