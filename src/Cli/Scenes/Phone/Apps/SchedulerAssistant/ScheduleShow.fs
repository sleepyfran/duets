module Cli.Scenes.Phone.Apps.SchedulerAssistant.Show

open Agents
open Cli.Actions
open Cli.Text
open Cli.Common
open Entities
open Entities.ConcertContext
open Simulation
open Simulation.Queries

let rec scheduleShow app =
    // Skip 5 days to give enough time for the scheduler to compute some ticket
    // purchases, otherwise the concert would be empty.
    let firstAvailableDay =
        Calendar.today (State.get ())
        |> Calendar.Ops.addDays 5

    scheduleShow' app firstAvailableDay

and private scheduleShow' app firstDate =
    let monthDays = Calendar.Query.monthDaysFrom firstDate

    let options =
        monthDays
        |> Seq.map
            (fun date ->
                { Id = date.ToString()
                  Text =
                      CommonDateWithDay date
                      |> CommonText
                      |> I18n.translate })
        |> List.ofSeq

    let nextMonthDate =
        Calendar.Query.firstDayOfNextMonth firstDate

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppShowDatePrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = options
                            Handler =
                                basicOptionalChoiceHandler
                                    (scheduleShow' app nextMonthDate)
                                    (fun dateChoice ->
                                        Calendar.Parse.date dateChoice.Id
                                        |> Option.get
                                        |> dayMomentPrompt app)
                            BackText =
                                PhoneText SchedulerAssistantCommonMoreDates
                                |> I18n.translate } }
    }

and private dayMomentPrompt app date =
    // Midnight is not taking into account since we don't want to allow
    // scheduling on the next day.
    let dayMomentOptions =
        Calendar.allDayMoments
        |> List.tail
        |> List.map
            (fun dayMoment ->
                { Id = dayMoment.ToString()
                  Text =
                      CommonDayMomentWithTime dayMoment
                      |> CommonText
                      |> I18n.translate })

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppShowTimePrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = dayMomentOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                    (fun dayMomentChoice ->
                                        Calendar.Parse.dayMoment
                                            dayMomentChoice.Id
                                        |> cityPrompt app date)
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private cityPrompt app date dayMoment =
    let cityOptions =
        World.allCities (State.get ())
        |> List.map
            (fun city ->
                { Id = city.Id.ToString()
                  Text = Literal city.Name })

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppShowCityPrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = cityOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                    (fun cityChoice ->
                                        Identity.from cityChoice.Id
                                        |> venuePrompt app date dayMoment)
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private venuePrompt app date dayMoment cityId =
    let concertSpaceOptions =
        World.allConcertSpacesOfCity (State.get ()) cityId
        |> List.map
            (fun (id, concertSpace) ->
                { Id = id.ToString()
                  Text = I18n.constant concertSpace.Name })

    let state = State.get ()

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppShowVenuePrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = concertSpaceOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                    (fun venueChoice ->
                                        Identity.from venueChoice.Id
                                        |> ticketPricePrompt
                                            app
                                            date
                                            dayMoment
                                            cityId)
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private ticketPricePrompt app date dayMoment cityId venue =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppTicketPricePrompt
                      )
                  Content =
                      NumberPrompt(
                          handleConcert app date dayMoment cityId venue
                      ) }
    }

and private handleConcert app date dayMoment cityId venueId ticketPrice =
    let state = State.get ()

    let scheduleResult =
        Scheduler.scheduleConcert
            state
            date
            dayMoment
            cityId
            venueId
            ticketPrice

    seq {
        match scheduleResult with
        | Ok effect ->
            yield Effect effect
            yield Separator
            yield! app ()
        | Error Scheduler.DateAlreadyScheduled ->
            yield
                SchedulerAssistantAppDateAlreadyBooked date
                |> PhoneText
                |> I18n.translate
                |> Message

            yield! scheduleShow app
        | Error (Scheduler.CreationError (InvalidTicketPrice price)) ->
            yield
                SchedulerAssistantAppTicketPriceInvalid price
                |> PhoneText
                |> I18n.translate
                |> Message

            yield! ticketPricePrompt app date dayMoment cityId venueId
    }
