namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation
open Duets.Simulation.Interactions

[<RequireQualifiedAccess>]
module rec PutCommand =
    /// Gets the put command, which allows the player to place an item
    /// in another storage item.
    let get =
        { Name = "put"
          Description = Command.putDescription
          Handler =
            fun args ->
                let x = Parse.itemsSeparatedBy " in " args

                match x with
                | [ item1Name; item2Name ] ->
                    let item1 = Command.findItem item1Name
                    let item2 = Command.findItem item2Name

                    match item1, item2 with
                    | None, _ -> Items.itemNotFound item1Name |> showMessage
                    | _, None -> Items.itemNotFound item2Name |> showMessage
                    | Some item1, Some item2 ->
                        Items.place (State.get ()) item1 item2
                        |> showPlaceResult (Generic.itemName item2)
                | _ ->
                    Command.putUsage
                    |> Command.wrongUsage
                    |> Styles.error
                    |> showMessage

                Scene.World }

    let private showPlaceResult shelfName res =
        match res with
        | Result.Ok effects ->
            $"You left the item in {shelfName}."
            |> Styles.success
            |> showMessage

            effects |> Effect.applyMultiple
        | Result.Error(Items.ItemIsNotPlaceable) ->
            "You can't place that anywhere!" |> Styles.error |> showMessage
        | Result.Error(Items.StorageItemIsNotStorage) ->
            "You can't put that there!" |> Styles.error |> showMessage
