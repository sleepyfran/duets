module Cli.View.Scenes.Phone.Apps.SchedulerAssistant.Show

open Agents
open Cli.View.Actions
open Cli.View.Text
open Cli.View.Common
open Entities
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
                                    (dayMomentPrompt app)
                            BackText = Literal "More months" } }
    }

and private dayMomentPrompt app (choice: Choice) =
    let selectedDate =
        Calendar.Parse.date choice.Id |> Option.get

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
                                phoneOptionalChoiceHandler (
                                    cityPrompt app selectedDate
                                )
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private cityPrompt app date (choice: Choice) =
    let dateWithDayMoment =
        Calendar.Parse.dayMoment choice.Id
        |> Calendar.Transform.changeDayMoment' date

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
                                phoneOptionalChoiceHandler (
                                    venuePrompt app dateWithDayMoment
                                )
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private venuePrompt app date choice =
    let selectedCity =
        World.cityById (State.get ()) (System.Guid.Parse choice.Id)
        |> Option.get

    let concertSpaceOptions =
        World.allConcertSpacesOfCity (State.get ()) selectedCity.Id
        |> List.map
            (fun (id, concertSpace) ->
                { Id = id.ToString()
                  Text = I18n.constant concertSpace.Name })

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
                                phoneOptionalChoiceHandler (
                                    ticketPricePrompt app date selectedCity
                                )
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private ticketPricePrompt app date city choice =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppTicketPricePrompt
                      )
                  Content =
                      NumberPrompt(
                          handleTicketPrice
                              app
                              date
                              city
                              (Identity.from choice.Id)
                      ) }
    }

and private handleTicketPrice app date city venue ticketPrice =
    seq { yield $"{ticketPrice}" |> I18n.constant |> Message }
