namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module LookCommand =
    let create (items: Item list) =
        { Name = "look"
          Description = Command.lookDescription
          Handler =
            (fun _ ->
                let state = State.get ()
                let currentPlace = state |> Queries.World.currentPlace
                let currentRoom = state |> Queries.World.currentRoom

                World.placeDescription currentPlace currentRoom
                |> showMessage

                match items with
                | [] -> Command.lookNoObjectsAround |> showMessage
                | items ->
                    items
                    |> List.map (fun item ->
                        Generic.itemName item |> Items.lookItem)
                    |> Items.lookItems
                    |> showMessage

                Scene.WorldAfterMovement) }
