module Duets.Cli.Scenes.Phone.Apps.BnB.Rent

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Rentals.RentPlace
open FsToolkit.ErrorHandling

let rec private placeInformationText cityId (place: Place) =
    let rentPrice = Queries.Rentals.calculateRentalPrice cityId place
    $"""{Styles.header place.Name} ({place.Zone.Name}) - Price: {Styles.money rentPrice}/month"""

/// Allows the player to rent a place in any city.
let rec rent bnbApp =
    option {
        let! selectedPlaceType =
            showOptionalChoicePrompt
                "Which type of place do you want to rent?"
                Generic.cancel
                (function
                | PlaceTypeIndex.Home -> "Home"
                | _ -> failwith "Rental not supported. Remove it from below :^)")
                [ PlaceTypeIndex.Home ]

        let! selectedCity =
            showCityPrompt "Where do you want to rent the place?"

        return toPlaceSelection selectedCity.Id selectedPlaceType
    }
    |> ignore

    bnbApp ()

and toPlaceSelection cityId placeType =
    let availablePlaces =
        Queries.Rentals.placesAvailableForRentInCity
            (State.get ())
            cityId
            placeType

    match availablePlaces with
    | [] ->
        Styles.error
            $"There are no places of this type left to rent in {Generic.cityName cityId |> Styles.place}"
        |> showMessage
    | _ ->
        availablePlaces
        |> showOptionalChoicePrompt
            $"These are the places available in {cityId |> Generic.cityName |> Styles.place}. Select one to see more information and rent it:"
            Generic.cancel
            (placeInformationText cityId)
        |> Option.map (displayPlaceInformation cityId placeType)
        |> ignore

and displayPlaceInformation cityId placeType place =
    let rentPrice = Queries.Rentals.calculateRentalPrice cityId place

    $"This {placeType |> World.placeTypeName} is located in {place.Zone.Name}. It costs {Styles.money rentPrice} per month"
    |> showMessage

    let confirmed = showConfirmationPrompt "Do you want to rent it?"

    if confirmed then
        let rentResult = rentPlace (State.get ()) cityId place

        match rentResult with
        | Ok(rental, effects) ->
            effects |> Effect.applyMultiple

            match rental.RentalType with
            | Monthly nextPaymentDate ->
                $"You've started renting a {place.Name |> String.lowercase} in {Generic.cityName cityId |> Styles.place}, you can now access it. Your next payment date is {Date.simple nextPaymentDate}. You will get a notification before it's time to pay again, but make sure you do it otherwise the rent will expire and you won't be able to access the place anymore"
            | OneTime untilDate ->
                $"You've rented {place.Name} in {Generic.cityName cityId} and you can now access it until {Date.simple untilDate}"
            |> Styles.success
            |> showMessage
        | Error(TransactionError(NotEnoughFunds amount)) ->
            $"You don't have {Styles.money amount} on your bank to pay for it"
            |> Styles.error
            |> showMessage
        | _ -> ()
    else
        ()
