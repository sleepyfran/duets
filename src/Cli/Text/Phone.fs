[<RequireQualifiedAccess>]
module Cli.Text.Phone

open Entities

let title = "Phone"
let optionBank = "Bank App"
let optionFlights = "Flights App"
let optionStatistics = "Statistics App"
let optionScheduler = "Scheduler App"

let putDown = Styles.faded "Leave phone"

let prompt dateTime dayMoment =
    $"""{Styles.title "DuetsPhone v1.0"}
{Generic.dayMomentName dayMoment |> Styles.time} of {Generic.formatDate dateTime |> Styles.time}"""

(* --- Bank --- *)

let bankAppTitle = "Bank"

let bankAppWelcome characterBalance bandBalance =
    $"""{Styles.highlight "You"} currently have {Styles.money characterBalance}. {Styles.highlight "Your band"} has {Styles.money bandBalance}"""

let bankAppPrompt =
    "What do you want to do?"

let bankAppTransferToBand =
    "Transfer money to band"

let bankAppTransferFromBand =
    "Transfer money from band"

let bankAppTransferAmount holder =
    match holder with
    | Character _ -> "How much do you want to transfer to your band?"
    | Band _ -> "How much do you want to transfer from your band?"

let bankAppTransferSuccess holder transaction =
    match transaction with
    | Incoming (amount, _) ->
        $"Transferred {Styles.money amount} to {Generic.accountHolderName holder}'s account"
    | Outgoing (amount, _) ->
        $"Transferred {Styles.money amount} from {Generic.accountHolderName holder}'s account"

let bankAppTransferNotEnoughFunds =
    Styles.error "Not enough funds in the sender account"

let bankAppTransferNothingTransferred =
    Styles.success "Nothing transferred"

(* --- Flights --- *)
let flightsNoUpcomingWelcome =
    "No upcoming flights"

let flightsOriginHeader =
    Styles.header "Origin"

let flightsDestinationHeader =
    Styles.header "Destination"

let flightsDateHeader = Styles.header "Date"

let flightsCityRow city = Generic.cityName city |> Styles.place

let flightsDateRow =
    Generic.dateWithDayMoment

let flightsAppPrompt =
    "What do you want to do?"

let bookFlightOption = "Book a flight"

let bookFlightOriginPrompt =
    "Where are you flying from?"

let bookFlightDestinationPrompt =
    "What's your destination?"

let bookFlightDatePrompt destination =
    $"When are you flying to {Generic.cityName destination |> Styles.place}?"

let chooseFlightPrompt = "Choose a flight:"

let flightInformation (flight: Flight) =
    $"{Generic.dateWithDay flight.Date} @ {Generic.dayMomentName flight.DayMoment}, price: {Styles.money flight.Price}"

let flightPurchaseConfirmation (flight: Flight) =
    $"Are you sure you want to spend {Styles.money flight.Price} in a flight from {Generic.cityName flight.Origin |> Styles.place} to {Generic.cityName flight.Destination
                                                                                                                                       |> Styles.place}?"

let flightsNotEnoughFunds amount =
    $"You don't have enough money to buy the ticket. Make sure you have at least {Styles.money amount} in your bank."

(* --- Scheduler --- *)

let schedulerAssistantAppPrompt =
    Styles.prompt "What do you want to book?"

let schedulerAssistantAppShow = "Book show"

let schedulerAssistantAppAgenda =
    "View schedule"

let schedulerAssistantAppVisualizeConcertInfo
    dayMoment
    (place: Place)
    (city: City)
    ticketsSold
    =
    $"""{Styles.highlight $"*{Generic.dayMomentName dayMoment}"}: Concert at {Styles.place place.Name}, {Styles.place (Generic.cityName city.Id)}. Sold {Styles.information ticketsSold} tickets"""

let schedulerAssistantAppVisualizeNoConcerts =
    "No concerts"

let schedulerAssistantAppVisualizeMoreDatesPrompt =
    "Do you want to see the next month?"

let schedulerAssistantAppShowDatePrompt =
    "When is the concert happening?"

let schedulerAssistantAppShowTimePrompt =
    "At what time?"

let schedulerAssistantAppShowCityPrompt =
    "In which city?"

let schedulerAssistantAppShowVenuePrompt =
    "In which venue?"

let schedulerAssistantAppTicketPricePrompt =
    $"""What will the price of each ticket be? {Styles.danger
                                                    "Keep in mind that putting high prices might affect how many people will go"}"""

let schedulerAssistantAppDateAlreadyBooked date =
    Styles.error $"You already have a concert on {Generic.formatDate date}!"

let schedulerAssistantAppTicketPriceBelowZero price =
    Styles.error
        $"The price can't be below zero! {Styles.number price} is not valid"

let schedulerAssistantAppTicketPriceTooHigh price =
    Styles.error
        $"{Styles.number price} is a bit too high for a concert. Maybe a bit less?"

let schedulerAssistantAppTicketDone (place: Place) concert =
    $"""Done! You scheduled a concert in {Styles.place place.Name} on {Styles.highlight (Generic.formatDate concert.Date)}. Be sure to be in the place at the moment of the concert, {Styles.danger "otherwise it'd fail miserably!"}"""

(* --- Statistics --- *)

let statisticsAppTitle = "Statistics"

let statisticsAppSectionPrompt =
    $"""{Styles.prompt "What data do you want to visualize?"}"""

let statisticsAppSectionBand =
    "Band's statistics"

let statisticsAppSectionAlbums =
    "Albums' statistics"

let statisticsAppBandNameHeader =
    Styles.header "Name"

let statisticsAppBandStartDateHeader =
    Styles.header "Playing since"

let statisticsAppBandFansHeader =
    Styles.header "Fans"

let statisticsAppBandName name = Styles.title name

let statisticsAppBandStartDate (date: Date) = Styles.highlight date.Year

let statisticsAppBandFans = Styles.number

let statisticsAppAlbumNoEntries =
    "No albums released yet"

let statisticsAppAlbumNameHeader =
    Styles.header "Album name"

let statisticsAppAlbumTypeHeader =
    Styles.header "Album type"

let statisticsAppAlbumReleaseDateHeader =
    Styles.header "Release date"

let statisticsAppAlbumStreamsHeader =
    Styles.header "Number of streams"

let statisticsAppAlbumRevenueHeader =
    Styles.header "Revenue"

let statisticsAppAlbumName name = Styles.information name
let statisticsAppAlbumType albumT = Generic.albumType albumT

let statisticsAppAlbumReleaseDate date =
    Styles.highlight (Generic.formatDate date)

let statisticsAppAlbumStreams streams =
    Styles.highlight (Styles.number streams)

let statisticsAppAlbumRevenue revenue = Styles.money revenue
