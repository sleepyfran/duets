namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation
open Duets.Simulation.Interactions

[<RequireQualifiedAccess>]
module rec OpenCommand =
    /// Gets the open command, which allows the player to peek the contents
    /// of a storage item and take an item from there.
    let get =
        { Name = "open"
          Description = Command.openDescription
          Handler =
            fun args ->
                let input = args |> String.concat " "
                let storageItem = input |> Command.findItem

                match storageItem with
                | Some storageItem ->
                    let storedItems = Items.from storageItem

                    match storedItems with
                    | Result.Ok items when List.isEmpty items ->
                        "There's nothing in there!"
                        |> Styles.error
                        |> showMessage
                    | Result.Ok items -> showItemSelector storageItem items
                    | _ ->
                        "You can't take anything from there!"
                        |> Styles.error
                        |> showMessage
                | _ ->
                    $"There's no item called {input} here."
                    |> Command.wrongUsage
                    |> Styles.error
                    |> showMessage

                Scene.World }

    let private showItemSelector storageItem storedItems =
        let selectedItem =
            showSearchableOptionalChoicePrompt
                "What would you like to take?"
                Generic.nothing
                Generic.itemDetailedName
                storedItems

        match selectedItem with
        | Some item ->
            Items.take (State.get ()) item storageItem |> showTakeResult
        | None -> ()

    let private showTakeResult res =
        match res with
        | Ok effects -> effects |> Effect.applyMultiple
        | _ ->
            "You can't take anything from there!" |> Styles.error |> showMessage
