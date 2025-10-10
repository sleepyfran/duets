module Duets.Cli.Scenes.Phone.Apps.Duber.Root

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.Prompts
open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation.Vehicles
open FSharp.Data.UnitSystems.SI.UnitNames

let rec duberApp () =
    Phone.duberAppTitle |> Styles.title |> showMessage
    showSeparator None

    Phone.duberWelcome |> showMessage
    lineBreak ()

    let destination = showMap ()

    match destination with
    | None ->
        Phone.duberRideCancelled |> showMessage
        lineBreak ()
        Scene.Phone
    | Some place -> bookRide place

and private bookRide (destination: Place) =
    let state = State.get ()

    showSeparator None
    Phone.duberCalculatingFare |> showMessage
    lineBreak ()

    let bookingResult = Taxi.bookRide state destination

    match bookingResult with
    | Error Taxi.AlreadyAtDestination ->
        Phone.duberAlreadyAtDestination |> showMessage
        lineBreak ()
        duberApp ()
    | Error Taxi.CannotReachDestination ->
        Phone.duberCannotReachDestination |> showMessage
        lineBreak ()
        duberApp ()
    | Error(Taxi.NotEnoughFunds amount) ->
        Phone.duberNotEnoughFunds amount |> showMessage
        lineBreak ()
        duberApp ()
    | Ok(fare, travelTime, effects) ->
        Phone.duberFareEstimate fare travelTime destination.Name |> showMessage

        lineBreak ()

        let confirmed = showConfirmationPrompt Phone.duberConfirmRide

        if confirmed then
            let driverName, _ = Npcs.random ()
            takeRide destination.Name travelTime effects driverName
        else
            Phone.duberRideCancelled |> showMessage
            lineBreak ()
            duberApp ()

and private takeRide destinationName travelTime effects driverName =
    showSeparator None
    Phone.duberRideBooked |> showMessage
    lineBreak ()

    Phone.duberDriverArriving driverName |> showMessage
    wait 500<millisecond>
    lineBreak ()

    let greeting = Phone.duberDriverGreeting |> List.sample
    Phone.duberDriverSays driverName greeting |> showMessage
    lineBreak ()
    wait 1000<millisecond>

    // Build progress based on travel time and split into segments.
    if travelTime < 5<minute> then
        showProgressBarSync
            [ Styles.progress "Cruising through the streets..."
              Styles.progress "Arriving at destination..." ]
            2<second>
    elif travelTime < 15<minute> then
        showProgressBarSync
            [ Styles.progress "Getting comfortable in the car..."
              Styles.progress "Heading to destination..." ]
            2<second>

        lineBreak ()
        generateAndShowDriverConversation driverName destinationName

        showProgressBarSync [ Styles.progress "Getting closer..." ] 1<second>
    else
        showProgressBarSync
            [ Styles.progress "Settling in for the ride..."
              Styles.progress "Cruising through traffic..." ]
            2<second>

        lineBreak ()
        generateAndShowDriverConversation driverName destinationName

        showProgressBarSync
            [ Styles.progress "Making good progress..."
              Styles.progress "Almost there..." ]
            2<second>

    Effect.applyMultiple effects

    showSeparator None
    Phone.duberArrivedAtDestination destinationName |> showMessage
    lineBreak ()

    let farewell = Phone.duberDriverFarewell () |> List.sample
    Phone.duberDriverSays driverName farewell |> showMessage
    lineBreak ()

    Scene.WorldAfterMovement

and private generateAndShowDriverConversation driverName destinationName =
    let state = State.get ()

    Social.npcSaysPrefix driverName |> showInlineMessage

    Duber.createDriverConversationPrompt state driverName destinationName
    |> LanguageModel.streamMessage
    |> streamStyled Styles.dialog

    lineBreak ()
    lineBreak ()
