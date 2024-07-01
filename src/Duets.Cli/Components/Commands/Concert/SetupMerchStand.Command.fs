namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts
open Duets.Simulation.Merchandise.SetPrice
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module SetupMerchStandCommand =
    let private promptForItemPrice item =
        let state = State.get ()
        let band = Queries.Bands.currentBand state

        let recommendedPrice =
            Queries.Merch.recommendedItemPrice' state band.Id item

        let price =
            $"How much do you want to charge for {Generic.itemDetailedName item |> Styles.item}? (Recommended: {Styles.money recommendedPrice})"
            |> showRangedDecimalPrompt 1m 1000m
            |> Amount.fromDecimal

        ConcertMerchSetPrice
            {| Band = band
               Item = item
               Price = price |}

    let rec private promptForMissingPrices items =
        items |> List.iter (promptForItemPrice >> Effect.applyAction)

    /// Returns a command that marks the merch stand as done in the current concert's
    /// checklist.
    let create checklist itemsWithoutPrice =
        { Name = "setup merch stand"
          Description =
            "Allows you to set-up your band's merch stand. This will increase the amount of money you make at the end of the concert if the fans buy your merch"
          Handler =
            fun _ ->
                match itemsWithoutPrice with
                | [] -> () (* All prices assigned, nothing to do. *)
                | _ ->
                    "You haven't assigned a price to all your merch items yet. Let's do it before setting up the stand."
                    |> Styles.warning
                    |> showMessage

                    promptForMissingPrices itemsWithoutPrice

                showProgressBarSync
                    [ "Setting up the table..." |> Styles.progress
                      "Hanging up stuff..." |> Styles.progress
                      "Writing prices..." |> Styles.progress ]
                    1<second>

                Live.Actions.setupMerchStand (State.get ()) checklist
                |> Effect.applyMultiple

                Scene.World }
