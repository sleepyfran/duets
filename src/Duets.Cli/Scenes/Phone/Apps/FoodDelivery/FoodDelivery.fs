module rec Duets.Cli.Scenes.Phone.Apps.FoodDelivery

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

let foodDeliveryApp () =
    let currentCity = Queries.World.currentCity (State.get ())

    let restaurants =
        Queries.World.placesByTypeInCity
            currentCity.Id
            PlaceTypeIndex.Restaurant
        |> List.map (fun place ->
            let currentlyOpen =
                Queries.World.placeCurrentlyOpen' (State.get ()) place

            place, currentlyOpen)

    let selectedRestaurant =
        showOptionalChoicePrompt
            $"""From {Styles.prompt "where"} do you want to order?"""
            Generic.cancel
            (fun (place, currentlyOpen) ->
                World.placeNameWithOpeningInfo' place currentlyOpen)
            restaurants

    match selectedRestaurant with
    | Some(place, currentlyOpen) when currentlyOpen = true ->
        let menu = Queries.Shop.menuOfPlace currentCity.Id place

        match menu with
        | Ok menu -> selectFood menu
        | Error _ -> ()

        Scene.Phone
    | Some(place, _) ->
        World.placeClosedError place |> showMessage
        World.placeOpeningHours place |> showMessage

        foodDeliveryApp ()
    | None -> Scene.Phone

let private selectFood menu =
    let selectedItem =
        showSearchableOptionalChoicePrompt
            $"""What do you want to {Styles.action "order"}?"""
            Generic.nothing
            Shop.itemInteractiveRow
            menu

    match selectedItem with
    | Some item ->
        let orderResult = Shop.order (State.get ()) item

        match orderResult with
        | Ok effects ->
            showProgressBarSync
                [ "Patiently waiting while they confirm order..."
                  "Waiting while they prepare it..."
                  "Waiting while they deliver it..." ]
                1<second>

            Duets.Cli.Effect.applyMultiple effects
        | Error _ -> Shop.notEnoughFunds |> showMessage
    | None -> ()
