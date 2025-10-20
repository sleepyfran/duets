namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.Prompts
open Duets.Entities
open Duets.Simulation

type private DriveChoice =
    | DriveWithinCity
    | DriveToCity of CityId

[<RequireQualifiedAccess>]
module DriveCommand =
    /// Command that allows the player to travel within a city and to another
    /// city (if connected by road) by driving a car.
    let rec create currentCarPosition car reachableCities =
        { Name = "drive"
          Description =
            "Allows you to drive your car to a place within the current city, or to a different, reachable city"
          Handler =
            fun _ ->
                showSeparator None
                lineBreak ()

                // Ask if driving within city or to another city
                let choice = chooseDriveType reachableCities

                match choice with
                | None ->
                    Travel.driveCancelled |> showMessage
                    lineBreak ()
                    Scene.World
                | Some DriveWithinCity -> driveWithinCity currentCarPosition car
                | Some(DriveToCity destinationCityId) ->
                    driveToCity destinationCityId currentCarPosition car }

    and private chooseDriveType reachableCities =
        let choices =
            [| ("Within current city", DriveWithinCity)
               yield!
                   reachableCities
                   |> List.map (fun (cityId, _) ->
                       (Generic.cityName cityId, DriveToCity cityId)) |]

        showOptionalChoicePrompt
            "Where do you want to drive to?"
            Generic.cancel
            fst
            choices
        |> Option.map snd

    and private driveWithinCity currentCarPosition car =
        let destination = showMap ()

        match destination with
        | None ->
            Travel.driveCancelled |> showMessage
            lineBreak ()
            Scene.World
        | Some place ->
            planAndConfirmDriveWithinCity place currentCarPosition car

    and private planAndConfirmDriveWithinCity
        (destination: Place)
        currentCarCoords
        car
        =
        let state = State.get ()

        showSeparator None
        Travel.driveCalculatingRoute |> showMessage
        lineBreak ()

        let planResult = Vehicles.Car.planWithinCityDrive state destination

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
                driveToDestinationWithinCity destination currentCarCoords car
            else
                Travel.driveCancelled |> showMessage
                lineBreak ()
                Scene.World

    and private driveToDestinationWithinCity
        (destination: Place)
        currentCarCoords
        car
        =
        let state = State.get ()

        showSeparator None
        Travel.driveStarting destination.Name |> showMessage
        lineBreak ()

        wait 500<millisecond>

        takeWithinCityDrive destination.Name car

        let moveEffects =
            Vehicles.Car.driveWithinCity state destination currentCarCoords car

        Effect.applyMultiple moveEffects

        Scene.WorldAfterMovement

    and private driveToCity destinationCityId currentCarPosition car =
        let state = State.get ()

        let planResult =
            Vehicles.Car.planIntercityDrive state destinationCityId car

        match planResult with
        | Error Vehicles.Car.AlreadyAtDestination ->
            Travel.driveAlreadyAtDestination |> showMessage
            lineBreak ()
            Scene.World
        | Error Vehicles.Car.CannotReachDestination ->
            Travel.driveCannotReachDestination |> showMessage
            lineBreak ()
            Scene.World
        | Ok(distance, travelTime, tripDuration) ->
            showSeparator None

            Travel.driveIntercityEstimate
                (Generic.cityName destinationCityId)
                distance
                travelTime
            |> showMessage

            lineBreak ()

            Travel.driveIntercityWarning |> showMessage
            lineBreak ()

            let confirmed = showConfirmationPrompt Travel.driveConfirmRoute

            if confirmed then
                executeIntercityDrive
                    destinationCityId
                    currentCarPosition
                    car
                    tripDuration
            else
                Travel.driveCancelled |> showMessage
                lineBreak ()
                Scene.World

    and private executeIntercityDrive
        destinationCityId
        currentCarPosition
        car
        tripDuration
        =
        let state = State.get ()

        showSeparator None
        Travel.driveStarting (Generic.cityName destinationCityId) |> showMessage
        lineBreak ()

        wait 500<millisecond>

        takeIntercityDrive destinationCityId car

        let moveEffects =
            Vehicles.Car.driveToCity
                state
                destinationCityId
                currentCarPosition
                car
                tripDuration

        Effect.applyMultiple moveEffects

        Scene.WorldAfterMovement

    and private takeWithinCityDrive destinationName car =
        let state = State.get ()

        generateWithinCityDrivingMoment state destinationName car
        wait 2000<millisecond>

        showSeparator None
        Travel.driveArrivedAtDestination destinationName |> showMessage
        lineBreak ()

    and private takeIntercityDrive destinationCityId car =
        let state = State.get ()
        let originCityId, _, _ = state.CurrentPosition

        generateIntercityDrivingMoment state originCityId destinationCityId car
        wait 2000<millisecond>

        showSeparator None

        Travel.driveArrivedAtDestination (Generic.cityName destinationCityId)
        |> showMessage

        lineBreak ()

    and private generateWithinCityDrivingMoment state destinationName car =
        Driving.createWithinCityDrivingMomentPrompt state destinationName car
        |> LanguageModel.streamMessage
        |> streamStyled Styles.event

        lineBreak ()

    and private generateIntercityDrivingMoment
        state
        originCityId
        destinationCityId
        car
        =
        Driving.createIntercityDrivingMomentPrompt
            state
            originCityId
            destinationCityId
            car
        |> LanguageModel.streamMessage
        |> streamStyled Styles.event

        lineBreak ()
