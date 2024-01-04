namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities

[<RequireQualifiedAccess>]
module BandInventoryCommand =
    /// Command that displays the inventory of the band.
    let create (items: (Item * int<quantity>) list) =
        { Name = "band inventory"
          Description = "Shows all the merchandise your band has in stock"
          Handler =
            (fun _ ->
                "Your band currently has:" |> showMessage

                items
                |> List.iter (fun (item, quantity) ->
                    $"- {quantity} {Generic.simplePluralOf (item.Name |> String.lowercase) quantity |> Styles.item}"
                    |> showMessage)

                Scene.World) }
