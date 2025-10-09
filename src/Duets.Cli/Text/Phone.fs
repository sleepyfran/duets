[<RequireQualifiedAccess>]
module Duets.Cli.Text.Phone

open Duets.Common
open Duets.Entities
open Duets.Cli.Text

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

let bankAppTransferAmount holder =
    match holder with
    | Character _ -> "How much do you want to transfer to your band?"
    | Band _ -> "How much do you want to transfer from your band?"

let bankAppTransferSuccess holder transaction =
    match transaction with
    | Incoming(amount, _) ->
        $"Transferred {Styles.money amount} to {Generic.accountHolderName holder}'s account"
    | Outgoing(amount, _) ->
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
    $"{Generic.dateWithDay flight.Date} @ {Generic.dayMomentName flight.DayMoment}{Styles.Spacing.choicePromptNewLine}{Styles.money flight.Price}"

let flightPurchaseConfirmation (flight: Flight) =
    $"Are you sure you want to spend {Styles.money flight.Price} in a flight from {Generic.cityName flight.Origin |> Styles.place} to {Generic.cityName flight.Destination |> Styles.place}?"

let flightsNotEnoughFunds amount =
    $"You don't have enough money to buy the ticket. Make sure you have at least {Styles.money amount} in your bank."

(* --- Jobs --- *)
let currentJobDescription (job: Job) (placeName: string) =
    let scheduleText =
        match job.CurrentStage.Schedule with
        | JobSchedule.Free _ -> "You don't have any schedule"
        | JobSchedule.Fixed(workDays, workDayMoments, _) ->
            let dayNames = workDays |> List.map Generic.dayName

            let dayMomentNames =
                workDayMoments |> List.map Generic.dayMomentName

            let daysText = Generic.listOf dayNames id
            let momentsText = Generic.listOf dayMomentNames id
            $"You work on: {daysText} during {momentsText}"

    Styles.faded
        $"""You currently work as {Career.name job |> String.lowercase} at {placeName} earning {Styles.money job.CurrentStage.BaseSalaryPerDayMoment} per day moment. {scheduleText}"""

let unemployed = Styles.faded "You are currently unemployed"

let findJobOption = "Find a new job"

let findJobTypePrompt = "What kind of job are you looking for?"

let findJobSelectPrompt = "Which job are you interested in applying?"

let findJobSelectItem (job: Job) (placeName: string) =
    $"{Career.name job} job at {placeName}.{Styles.Spacing.choicePromptNewLine}Salary: {Styles.money job.CurrentStage.BaseSalaryPerDayMoment}/day moment.{Styles.Spacing.choicePromptNewLine}{Career.scheduleDescription job.CurrentStage.Schedule}\n"

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

let concertAssistantAppAgenda = "View schedule"

let concertAssistantAppVisualizeNoConcerts = "No concerts"

let concertAssistantAppVisualizeMoreDatesPrompt =
    "Do you want to see the next season?"

let concertAssistantAppShowDatePrompt = "When is the concert happening?"

let concertAssistantAppShowTimePrompt = "At what time?"

let concertAssistantAppShowCityPrompt = "In which city?"

let concertAssistantAppShowVenuePrompt maxCapacity =
    $"In which venue? (Recommended capacity: up to {maxCapacity})"

let concertAssistantAppTicketPricePrompt recommendedPrice =
    $"""What will the price of each ticket be? (Recommended: up to {recommendedPrice |> Styles.money})"""

let concertAssistantAppDateAlreadyBooked date =
    Styles.error $"You already have a concert on {Date.simple date}!"

let concertAssistantAppTicketPriceBelowZero price =
    Styles.error
        $"The price can't be below zero! {Styles.decimal price} is not valid"

let concertAssistantAppTicketPriceTooHigh price =
    Styles.error
        $"{Styles.decimal price} is a bit too high for a concert. Maybe a bit less?"

(* --- Statistics --- *)

let statisticsAppTitle = "Statistics"

let statisticsAppSectionPrompt =
    $"""{Styles.prompt "What data do you want to visualize?"}"""

let statisticsAppAlbumNoEntries = "No albums released yet"

let statisticsAppAlbumNameHeader = Styles.header "Album name"

let statisticsAppAlbumTypeHeader = Styles.header "Album type"

let statisticsAppAlbumGenreHeader = Styles.header "Genre"

let statisticsAppAlbumReleaseDateHeader = Styles.header "Release date"

let statisticsAppAlbumStreamsHeader = Styles.header "Number of streams"

let statisticsAppAlbumRevenueHeader = Styles.header "Revenue"

let statisticsAppAlbumName name = Styles.information name

let statisticsAppAlbumType albumT = Generic.albumType albumT

let statisticsAppGenre = Styles.information

let statisticsAppAlbumReleaseDate date = Styles.highlight (Date.simple date)

let statisticsAppAlbumStreams streams =
    Styles.highlight (Styles.number streams)

let statisticsAppAlbumRevenue revenue = Styles.money revenue

(* --- Duber --- *)
let duberAppTitle = "Duber"

let duberWelcome = "Where would you like to go?"

let duberCalculatingFare = "Calculating fare..."

let duberFareEstimate fare travelTime destination =
    $"""Your ride to {Styles.place destination} will cost approximately {Styles.money fare} and take about {Styles.time travelTime} minutes."""

let duberConfirmRide = "Book this ride?"

let duberRideBooked = "Your Duber is on the way!"

let duberArrivedAtDestination destination =
    $"You've arrived at {Styles.place destination}!"

let duberAlreadyAtDestination = Styles.error "You're already at this location!"

let duberCannotReachDestination =
    Styles.error "Sorry, we can't find a route to that destination."

let duberNotEnoughFunds amount =
    $"You don't have enough money for this ride. You need at least {Styles.money amount} in your account."
    |> Styles.error

let duberRideCancelled = "Ride cancelled" |> Styles.error

let duberDriverArriving driverName =
    $"Your driver {Styles.person driverName} is arriving..." |> Styles.hint

let duberDriverSays driverName message =
    $"{Styles.person driverName}: {Styles.dialog message}"

let duberDriverGreeting =
    [ "Hey there! Hop in!"
      "Good to see you! Let's get you there."
      "Welcome aboard! How's your day going?"
      "Hi! Ready to head out?"
      "Hey! Nice day for a ride, huh?"
      "Hello! Make yourself comfortable."
      "Yo! Let's hit the road!"
      "Hey friend! Buckle up!" ]

let duberDriverFarewell () =
    [ Styles.success "Here we are! Have a great day!"
      Styles.success "All set! Take care now!"
      Styles.success "We're here! Hope you enjoyed the ride."
      Styles.success "Destination reached! Rock on!"
      Styles.success "And we're here! Good luck with everything!"
      Styles.success "Here you go! Stay safe!"
      Styles.success "Made it! Have an awesome day!"
      Styles.success "All done! Catch you later!" ]
