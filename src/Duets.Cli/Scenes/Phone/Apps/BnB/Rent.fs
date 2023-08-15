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
    let pricing =
        match place.PlaceType with
        | PlaceType.Hotel _ ->
            let price =
                Queries.Rentals.calculateOneTimeRentalPrice place 1<days>

            $"{Styles.money price}/day"
        | _ ->
            let price = Queries.Rentals.calculateMonthlyRentalPrice cityId place
            $"{Styles.money price}/month"

    $"""{Styles.header place.Name} ({place.Zone.Name}) - Price: {pricing}"""

/// Allows the player to rent a place in any city.
let rec rent bnbApp =
    option {
        let! selectedPlaceType =
            showOptionalChoicePrompt
                "Which type of place do you want to rent?"
                Generic.cancel
                (function
                | PlaceTypeIndex.Home -> "Home"
                | PlaceTypeIndex.Hotel -> "Hotel"
                | _ -> failwith "Rental not supported. Remove it from below :^)")
                [ PlaceTypeIndex.Home; PlaceTypeIndex.Hotel ]

        let! selectedCity =
            showCityPrompt "Where do you want to rent the place?"

        return toPlaceSelection selectedCity.Id selectedPlaceType
    }
    |> ignore

    bnbApp ()

and private toPlaceSelection cityId placeType =
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
        |> Option.map (displayPlaceInformation cityId)
        |> ignore

and private displayPlaceInformation cityId place =
    match place.PlaceType with
    | PlaceType.Home -> displayMonthlyRentalInformation cityId place
    | PlaceType.Hotel hotel -> toHotelDateSelection cityId place hotel
    | _ -> failwith "Not supported to rent"

and private displayMonthlyRentalInformation cityId place =
    let rentPrice = Queries.Rentals.calculateMonthlyRentalPrice cityId place
    let placeType = World.Place.Type.toIndex place.PlaceType

    $"This {placeType |> World.placeTypeName} is located in {place.Zone.Name}. It costs {Styles.money rentPrice} per month"
    |> showMessage

    let confirmed = showConfirmationPrompt "Do you want to rent it?"

    if confirmed then
        rentMonthlyPlace (State.get ()) cityId place
        |> showRentalResult cityId place
    else
        ()

and toHotelDateSelection cityId place hotel =
    $"A night in this hotel costs {Styles.money hotel.PricePerNight}. You can rent it for a maximum of 30 days"
    |> showMessage

    let firstAvailableDate = Queries.Calendar.today (State.get ())

    let selectedDate =
        showInteractiveDatePrompt
            "From which date do you want to rent it?"
            firstAvailableDate

    match selectedDate with
    | Some fromDate -> toLengthOfStaySelection cityId place fromDate
    | None -> ()

and private toLengthOfStaySelection cityId place fromDate =
    let numberOfDays =
        showRangedNumberPrompt 1 30 "How many days do you want to rent it for?"
        |> (*) 1<days>

    let totalPrice =
        Queries.Rentals.calculateOneTimeRentalPrice place numberOfDays

    let confirmed =
        showConfirmationPrompt
            $"The total price for your stay would be {Styles.money totalPrice}. Do you want to rent it?"

    if confirmed then
        rentOneTimePlace (State.get ()) cityId place fromDate numberOfDays
        |> showRentalResult cityId place
    else
        ()

and private showRentalResult cityId place rentResult =
    match rentResult with
    | Ok(rental, effects) ->
        effects |> Effect.applyMultiple

        match rental.RentalType with
        | Monthly nextPaymentDate ->
            $"You've started renting a {place.Name |> String.lowercase} in {Generic.cityName cityId |> Styles.place}, you can now access it. Your next payment date is {Date.simple nextPaymentDate}. You will get a notification before it's time to pay again, but make sure you do it otherwise the rent will expire and you won't be able to access the place anymore"
        | OneTime(fromDate, untilDate) ->
            $"You've rented {place.Name} in {Generic.cityName cityId} and you can now access it from {Date.simple fromDate} until {Date.simple untilDate}"
        |> Styles.success
        |> showMessage
    | Error(TransactionError(NotEnoughFunds amount)) ->
        $"You don't have {Styles.money amount} on your bank to pay for it"
        |> Styles.error
        |> showMessage
    | _ -> ()
