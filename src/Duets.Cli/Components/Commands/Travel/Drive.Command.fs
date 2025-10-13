namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.Prompts
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module DriveCommand =
    /// Command that allows the player to travel within a city and to another
    /// city (if connected by road) by driving a car.
    let rec create currentCarPosition car =
        { Name = "drive"
          Description =
            "Allows you to drive your car to a place within the current city, or to a different, reachable city"
          Handler =
            fun _ ->
                showSeparator None
                lineBreak ()

                let destination = showMap ()

                match destination with
                | None ->
                    Travel.driveCancelled |> showMessage
                    lineBreak ()
                    Scene.World
                | Some place -> planAndConfirmDrive place currentCarPosition car }

    and private planAndConfirmDrive (destination: Place) currentCarCoords car =
        let state = State.get ()

        showSeparator None
        Travel.driveCalculatingRoute |> showMessage
        lineBreak ()

        let planResult = Vehicles.Car.planDrive state destination

        match planResult with
        | Error Vehicles.Car.AlreadyAtDestination ->
            Travel.driveAlreadyAtDestination |> showMessage
            lineBreak ()
            Scene.World
        | Error Vehicles.Car.CannotReachDestination ->
            Travel.driveCannotReachDestination |> showMessage
            lineBreak ()
            Scene.World
        | Ok(_, travelTime) ->
            Travel.driveRouteEstimate travelTime destination.Name |> showMessage
            lineBreak ()

            let confirmed = showConfirmationPrompt Travel.driveConfirmRoute

            if confirmed then
                driveToDestination destination currentCarCoords car
            else
                Travel.driveCancelled |> showMessage
                lineBreak ()
                Scene.World

    and private driveToDestination (destination: Place) currentCarCoords car =
        let state = State.get ()

        showSeparator None
        Travel.driveStarting destination.Name |> showMessage
        lineBreak ()

        wait 500<millisecond>

        takeDrive destination.Name car

        let moveEffects =
            Vehicles.Car.drive state destination currentCarCoords car

        Effect.applyMultiple moveEffects

        Scene.WorldAfterMovement

    and private takeDrive destinationName car =
        let state = State.get ()

        generateDrivingMoment state destinationName car
        wait 2000<millisecond>

        showSeparator None
        Travel.driveArrivedAtDestination destinationName |> showMessage
        lineBreak ()

    and private generateDrivingMoment state destinationName car =
        Driving.createDrivingMomentPrompt state destinationName car
        |> LanguageModel.streamMessage
        |> streamStyled Styles.event

        lineBreak ()
