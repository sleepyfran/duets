module Cli.View.Scenes.Phone.Apps.SchedulerAssistant.Show

open Agents
open Cli.View.Actions
open Cli.View.Text
open Cli.View.Common
open Entities
open Simulation.Queries

let rec scheduleShow app =
    let today = Calendar.today (State.get ())

    scheduleShow' app today

and private scheduleShow' app firstDate =
    let monthDays = Calendar.monthDaysFrom firstDate

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

    let nextMonthDate = Calendar.firstDayOfNextMonth firstDate

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
                                    (processDate app)
                            BackText = Literal "More months" } }
    }

and private processDate app (choice: Choice) =
    let selectedDate = Calendar.parse choice.Id |> Option.get

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
                                    processCity app selectedDate
                                )
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private processCity app date choice =
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
                          PhoneText SchedulerAssistantAppShowCityPrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = concertSpaceOptions
                            Handler =
                                phoneOptionalChoiceHandler (
                                    processVenue app date selectedCity
                                )
                            BackText =
                                (CommonText CommonCancel |> I18n.translate) } }
    }

and private processVenue app date city choice =
    let selectedVenue =
        World.concertSpaceById (State.get ()) city.Id (Identity.from choice.Id)
        |> Option.get

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText SchedulerAssistantAppTicketPricePrompt
                      )
                  Content =
                      NumberPrompt(
                          processTicketPrice app date city selectedVenue
                      ) }
    }

and private processTicketPrice app date city venue ticketPrice =
    seq { yield $"{ticketPrice}" |> I18n.constant |> Message }
