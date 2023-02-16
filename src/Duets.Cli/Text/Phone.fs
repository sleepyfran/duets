[<RequireQualifiedAccess>]
module Duets.Cli.Text.Phone

open Duets.Entities

let title = "Phone"

let putDown = Styles.faded "Leave phone"

let prompt dateTime dayMoment =
    $"""{Styles.title "DuetsPhone v1.0"}
{Generic.dayMomentName dayMoment |> Styles.time} of {Date.simple dateTime |> Styles.time}"""

let optionPrompt = "What do you want to do?"

(* --- Bank --- *)

let bankAppTitle = "Bank"

let bankAppWelcome characterBalance bandBalance =
    $"""{Styles.highlight "You"} currently have {Styles.money characterBalance}. {Styles.highlight "Your band"} has {Styles.money bandBalance}"""

let bankAppPrompt = "What do you want to do?"

let bankAppTransferToBand = "Transfer money to band"

let bankAppTransferFromBand = "Transfer money from band"

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

let bankAppTransferNothingTransferred = Styles.success "Nothing transferred"

(* --- Flights --- *)
let flightsNoUpcomingWelcome = "No upcoming flights"

let flightsOriginHeader = Styles.header "Origin"

let flightsDestinationHeader = Styles.header "Destination"

let flightsDateHeader = Styles.header "Date"

let flightsCityRow city = Generic.cityName city |> Styles.place

let flightsDateRow = Generic.dateWithDayMoment

let flightsAppPrompt = "What do you want to do?"

let bookFlightOption = "Book a flight"

let bookFlightOriginPrompt = "Where are you flying from?"

let bookFlightDestinationPrompt = "What's your destination?"

let bookFlightDatePrompt destination =
    $"When are you flying to {Generic.cityName destination |> Styles.place}?"

let chooseFlightPrompt = "Choose a flight:"

let flightInformation (flight: Flight) =
    $"{Generic.dateWithDay flight.Date} @ {Generic.dayMomentName flight.DayMoment}, price: {Styles.money flight.Price}"

let flightPurchaseConfirmation (flight: Flight) =
    $"Are you sure you want to spend {Styles.money flight.Price} in a flight from {Generic.cityName flight.Origin |> Styles.place} to {Generic.cityName flight.Destination |> Styles.place}?"

let flightsNotEnoughFunds amount =
    $"You don't have enough money to buy the ticket. Make sure you have at least {Styles.money amount} in your bank."

(* --- Jobs --- *)
let currentJobDescription (job: Job) (placeName: string) =
    Styles.faded
        $"""You currently work as {Career.name job.Id} at {placeName} earning {Styles.money job.CurrentStage.BaseSalaryPerDayMoment} per day moment. {match job.Schedule with
                                                                                                                                                      | JobSchedule.Free _ -> "You don't have any schedule"}"""

let unemployed = Styles.faded "You are currently unemployed"

let findJobOption = "Find a new job"

let findJobTypePrompt = "What kind of job are you looking for?"

let findJobSelectPrompt = "Which job are you interested in applying?"

let findJobSelectItem (job: Job) (placeName: string) =
    $"{Career.name job.Id} job at {placeName}. Salary: {Styles.money job.CurrentStage.BaseSalaryPerDayMoment}/day moment. {Career.scheduleDescription job.Schedule}"

let findJobAcceptConfirmation careerId placeName =
    Styles.prompt
        $"Are you sure you want to work as {Career.name careerId} at {placeName}?"

let findJobAcceptLeaveConfirmation
    newCareerId
    newPlaceName
    currentCareerId
    currentPlaceName
    =
    $"""{findJobAcceptConfirmation newCareerId newPlaceName} {Styles.danger
                                                                  $"You will leave your job as {Career.name currentCareerId} at {currentPlaceName}"}"""

(* --- Scheduler --- *)

let concertAssistantAppPrompt = Styles.prompt "What do you want to book?"

let concertAssistantAppShow = "Book show"

let concertAssistantAppAgenda = "View schedule"

let concertAssistantAppVisualizeNoConcerts = "No concerts"

let concertAssistantAppVisualizeMoreDatesPrompt =
    "Do you want to see the next month?"

let concertAssistantAppShowDatePrompt = "When is the concert happening?"

let concertAssistantAppShowTimePrompt = "At what time?"

let concertAssistantAppShowCityPrompt = "In which city?"

let concertAssistantAppShowVenuePrompt = "In which venue?"

let concertAssistantAppTicketPricePrompt =
    $"""What will the price of each ticket be? {Styles.danger
                                                    "Keep in mind that putting high prices might affect how many people will go"}"""

let concertAssistantAppDateAlreadyBooked date =
    Styles.error $"You already have a concert on {Date.simple date}!"

let concertAssistantAppTicketPriceBelowZero price =
    Styles.error
        $"The price can't be below zero! {Styles.decimal price} is not valid"

let concertAssistantAppTicketPriceTooHigh price =
    Styles.error
        $"{Styles.decimal price} is a bit too high for a concert. Maybe a bit less?"

let concertAssistantAppTicketDone (place: Place) concert =
    $"""Done! You scheduled a concert in {Styles.place place.Name} on {Styles.highlight (Date.simple concert.Date)}. Be sure to be in the place at the moment of the concert, {Styles.danger "otherwise it'd fail miserably!"}"""

(* --- Statistics --- *)

let statisticsAppTitle = "Statistics"

let statisticsAppSectionPrompt =
    $"""{Styles.prompt "What data do you want to visualize?"}"""

let statisticsAppSectionBand = "Band's statistics"

let statisticsAppSectionAlbums = "Albums' statistics"

let statisticsAppBandNameHeader = Styles.header "Name"

let statisticsAppBandStartDateHeader = Styles.header "Playing since"

let statisticsAppBandFansHeader = Styles.header "Fans"

let statisticsAppBandName name = Styles.title name

let statisticsAppBandStartDate (date: Date) = Styles.highlight date.Year

let statisticsAppBandFans = Styles.number

let statisticsAppAlbumNoEntries = "No albums released yet"

let statisticsAppAlbumNameHeader = Styles.header "Album name"

let statisticsAppAlbumTypeHeader = Styles.header "Album type"

let statisticsAppAlbumReleaseDateHeader = Styles.header "Release date"

let statisticsAppAlbumStreamsHeader = Styles.header "Number of streams"

let statisticsAppAlbumRevenueHeader = Styles.header "Revenue"

let statisticsAppAlbumName name = Styles.information name
let statisticsAppAlbumType albumT = Generic.albumType albumT

let statisticsAppAlbumReleaseDate date =
    Styles.highlight (Date.simple date)

let statisticsAppAlbumStreams streams =
    Styles.highlight (Styles.number streams)

let statisticsAppAlbumRevenue revenue = Styles.money revenue
