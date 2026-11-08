namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.Flavor
open Duets.Simulation
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module BuyCarCommand =
    /// Command to buy a car from a dealer that brings an interactive way of
    /// buying a car with a dealer and, upon buying, places the car in the street
    /// in front of the dealer.
    let rec create carDealer availableItems =
        { Name = "buy"
          Description = Command.buyCarDescription
          Handler =
            fun _ ->
                let state = State.get ()

                lineBreak ()

                CarDealer.greeting state carDealer
                |> Social.npcSays carDealer.Dealer.Name
                |> showMessage

                lineBreak ()

                let selectedCar =
                    showSearchableOptionalChoicePrompt
                        Shop.carSelectionPrompt
                        Generic.cancel
                        Shop.carInteractiveRow
                        availableItems

                match selectedCar with
                | None ->
                    handleDeparture carDealer None false
                    Scene.World
                | Some(car, price) -> showCarDetails carDealer car price }

    and private showCarDetails carDealer car price =
        showSeparator None
        lineBreak ()

        let carTitle = $"{car.Brand} {car.Name}"
        carTitle |> Styles.highlight |> showMessage
        lineBreak ()

        Shop.carPrice price |> showMessage
        lineBreak ()

        CarDealer.pitch carDealer car
        |> Social.npcSays carDealer.Dealer.Name
        |> showMessage

        lineBreak ()

        let confirmed = showConfirmationPrompt Shop.confirmCarPurchase

        if confirmed then
            processPurchase carDealer car price
        else
            handleDeparture carDealer (Some car) false
            Scene.World

    and private processPurchase carDealer car price =
        let state = State.get ()

        let orderResult = Shop.buyCar state (car, price)

        match orderResult with
        | Ok effects ->
            showSeparator None
            lineBreak ()

            Shop.processingPurchase |> showMessage
            wait 800<millisecond>
            lineBreak ()

            showProgressBarSync
                [ Styles.progress "Preparing paperwork..."
                  Styles.progress "Processing payment..."
                  Styles.progress "Getting the keys..." ]
                2<second>

            lineBreak ()

            handleDeparture carDealer (Some car) true

            Effect.applyMultiple effects

            Scene.World
        | Error _ ->
            showSeparator None

            Shop.paymentRejectNotEnoughFunds |> showMessage

            showSeparator None
            lineBreak ()

            CarDealer.insufficientFunds carDealer car
            |> Social.npcSays carDealer.Dealer.Name
            |> showMessage

            lineBreak ()

            Scene.World

    and private handleDeparture carDealer carOption purchased =
        showSeparator None
        lineBreak ()

        match carOption with
        | Some car -> CarDealer.closing car purchased
        | None -> Shop.carDealerGenericFarewell
        |> Social.npcSays carDealer.Dealer.Name
        |> showMessage

        lineBreak ()

        if purchased then
            Shop.carAvailableOutside |> Styles.success |> showMessage
            lineBreak ()
