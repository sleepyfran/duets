module Cli.View.Scenes.Statistics.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let statisticOptions =
    [ { Id = "band"
        Text = TextConstant StatisticsSectionBand }
      { Id = "albums"
        Text = TextConstant StatisticsSectionAlbums } ]

let rec statisticsScene () =
    seq {
        yield
            Prompt
                { Title = TextConstant StatisticsSectionPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = statisticOptions
                            Handler =
                                phoneOptionalChoiceHandler <| processSelection
                            BackText = TextConstant CommonBackToPhone } }

    }

and processSelection selection =
    seq {
        match selection.Id with
        | "band" -> yield SubScene StatisticsOfBand
        | "albums" -> yield SubScene StatisticsOfAlbums
        | _ -> yield NoOp
    }
