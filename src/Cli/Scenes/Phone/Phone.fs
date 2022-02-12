module Cli.Scenes.Phone.Root

open Agents
open Cli.Actions
open Cli.Common
open Cli.Text
open Entities
open Simulation

let private phoneOptions =
    [ yield
        { Id = "bank"
          Text = I18n.translate (PhoneText PhoneOptionBank) }

      yield
          { Id = "statistics"
            Text = I18n.translate (PhoneText PhoneOptionStatistics) }

      yield
          { Id = "scheduler"
            Text = I18n.constant "Scheduler assistant" } ]

let rec phoneScene () =
    let currentDate = State.get () |> Queries.Calendar.today

    let dayMoment = Calendar.Query.dayMomentOf currentDate

    seq {
        yield
            Prompt
                { Title =
                      PhonePrompt(currentDate, dayMoment)
                      |> PhoneText
                      |> I18n.translate
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = phoneOptions
                            Handler =
                                worldOptionalChoiceHandler <| processSelection
                            BackText =
                                I18n.translate (CommonText CommonBackToWorld) } }
    }

and processSelection choice =
    seq {
        yield Separator

        match choice.Id with
        | "bank" -> yield! Apps.Bank.Root.bankApp ()
        | "statistics" -> yield! Apps.Statistics.Root.statisticsApp ()
        | "scheduler" ->
            yield! Apps.SchedulerAssistant.Root.schedulerAssistantApp ()
        | _ -> yield NoOp
    }
