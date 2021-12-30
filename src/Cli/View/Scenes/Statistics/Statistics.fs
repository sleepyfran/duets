module Cli.View.Scenes.Statistics.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text

let statisticOptions =
    [ { Id = "band"
        Text = I18n.translate (StatisticsText StatisticsSectionBand) }
      { Id = "albums"
        Text = I18n.translate (StatisticsText StatisticsSectionAlbums) } ]

let rec statisticsScene () =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (StatisticsText StatisticsSectionPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = statisticOptions
                            Handler =
                                phoneOptionalChoiceHandler <| processSelection
                            BackText =
                                I18n.translate (CommonText CommonBackToPhone) } }

    }

and processSelection selection =
    seq {
        match selection.Id with
        | "band" -> yield! Band.bandStatisticsSubScene ()
        | "albums" -> yield! Albums.albumsStatisticsSubScene ()
        | _ -> yield NoOp
    }
