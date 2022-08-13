namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Interactions

[<RequireQualifiedAccess>]
module DrinkCommand =
    /// Command to drink an item that the user has referenced by text.
    let get =
        { Name = "drink"
          Description = Command.drinkDescription
          Handler =
            (fun args ->
                let input = args |> String.concat " "

                let item = Queries.Inventory.findByName (State.get ()) input

                match item with
                | Some item ->
                    match Items.consume (State.get ()) item Items.Drink with
                    | Ok effects ->
                        Inventory.drunkItem |> showMessage
                        wait 1000<millisecond>
                        effects |> Cli.Effect.applyMultiple
                    | Error _ -> Inventory.itemNotDrinkable |> showMessage
                | None -> Inventory.itemNotFound input |> showMessage

                Scene.World) }
