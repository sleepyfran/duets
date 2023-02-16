namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module LookCommand =
    let create (items: Item list) =
        { Name = "look"
          Description = Command.lookDescription
          Handler =
            (fun _ ->
                match items with
                | [] -> Command.lookNoObjectsAround |> showMessage
                | items ->
                    items
                    |> List.map (fun item ->
                        Generic.itemName item |> Items.lookItem)
                    |> Items.lookItems
                    |> showMessage

                Scene.WorldAfterMovement) }
