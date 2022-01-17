module Cli.View.Scenes.Phone

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation

let phoneOptions =
    [ yield
        { Id = "bank"
          Text = I18n.translate (PhoneText PhoneOptionBank) }

      yield
          { Id = "statistics"
            Text = I18n.translate (PhoneText PhoneOptionStatistics) } ]

let rec phoneScene () =
    let currentDate = State.get () |> Queries.Calendar.today

    let dayMoment = Calendar.dayMomentOf currentDate

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
        match choice.Id with
        | "bank" -> yield Scene Bank
        | "statistics" -> yield Scene Statistics
        | _ -> yield NoOp
    }
