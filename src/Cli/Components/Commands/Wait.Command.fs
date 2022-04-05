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
          Description = I18n.translate (CommandText CommandWaitDescription)
          Handler =
              (fun args ->
                  match args with
                  | timeAmount :: _ ->
                      match System.Int32.TryParse timeAmount with
                      | true, num -> Wait num |> Effect.apply
                      | _ ->
                          showMessage (
                              CommandWaitInvalidTimes timeAmount
                              |> CommandText
                              |> I18n.translate
                          )
                  | _ -> Wait 1 |> Effect.apply

                  Scene.World) }
