namespace Cli.Components.Commands

open Agents
open Cli
open Cli.Components
open Cli.Text
open Cli.SceneIndex
open Common
open Entities
open Simulation
open Simulation.Navigation

[<RequireQualifiedAccess>]
module rec MapCommand =
    let private placeWithOpenInfo place =
        let currentlyOpen =
            Queries.World.placeCurrentlyOpen (State.get ()) place

        match currentlyOpen with
        | true -> World.placeWithZone place
        | false -> Styles.faded $"{place.Name} ({place.Zone.Name}) - Closed"

    let private showOpeningHours place =
        match place.OpeningHours with
        | PlaceOpeningHours.OpeningHours (daysOfWeek, dayMoments) ->
            let openingDays =
                match daysOfWeek with
                | days when days = Calendar.everyDay -> "Everyday"
                | days when days = Calendar.weekday -> "Monday to Friday"
                | _ -> Generic.listOf daysOfWeek Generic.dayName

            let openingHours = Generic.listOf dayMoments Generic.dayMomentName

            $"{Styles.place place.Name} opens {Styles.time openingDays} @ {openingHours}"
            |> showMessage
        | _ ->
            (* Obviously if it's always open this shouldn't happen :) *)
            ()

    let private showPlaceChoice placesInCity places =
        let selectedPlace =
            showOptionalChoicePrompt
                Command.mapChoosePlace
                Generic.back
                placeWithOpenInfo
                places

        match selectedPlace with
        | Some place -> Some place
        | None -> showPlaceTypeChoice placesInCity

    let private showPlaceTypeChoice
        (placesInCity: Map<PlaceTypeIndex, Place list>)
        =
        let availablePlaceTypes =
            placesInCity |> List.ofSeq |> List.map (fun kvp -> kvp.Key)

        showOptionalChoicePrompt
            Command.mapChoosePlaceTypePrompt
            Generic.cancel
            World.placeTypeName
            availablePlaceTypes
        |> Option.bind (fun placeType ->
            placesInCity |> Map.find placeType |> showPlaceChoice placesInCity)

    let private moveToPlace availablePlaces (place: Place) =
        let navigationResult = Navigation.moveTo place.Id (State.get ())

        match navigationResult with
        | Ok effect -> effect |> Effect.apply
        | Error PlaceEntranceError.CannotEnterOutsideOpeningHours ->
            showSeparator None

            Styles.error
                $"{place.Name} is currently closed. Try again during their opening hours"
            |> showMessage

            showOpeningHours place

            showSeparator None

            askForPlace availablePlaces

    let private askForPlace availablePlaces =
        let selectedPlace = availablePlaces |> showPlaceTypeChoice

        match selectedPlace with
        | Some place -> moveToPlace availablePlaces place
        | None -> ()

    /// Creates a command that allows the player to navigate to different
    /// parts of the game world inside the current city.
    let get =
        { Name = "map"
          Description = Command.mapDescription
          Handler =
            fun _ ->
                Queries.World.currentCity (State.get ())
                |> fun city -> city.Id
                |> Command.mapCurrentCity
                |> showMessage

                Command.mapTip |> showMessage
                lineBreak ()

                Queries.World.allPlacesInCurrentCity (State.get ())
                |> askForPlace

                lineBreak ()

                Scene.WorldAfterMovement }
