module Cli.View.Scenes.Phone.Apps.Statistics.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text

let private statisticOptions =
    [ { Id = "band"
        Text = I18n.translate (PhoneText StatisticsAppSectionBand) }
      { Id = "albums"
        Text = I18n.translate (PhoneText StatisticsAppSectionAlbums) } ]

let rec statisticsApp () =
    seq {
        yield
            Prompt
                { Title = I18n.translate (PhoneText StatisticsAppSectionPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = statisticOptions
                            Handler =
                                phoneOptionalChoiceHandler <| processSelection
                            BackText =
                                I18n.translate (CommonText CommonBackToPhone) } }

    }

and private processSelection selection =
    seq {
        match selection.Id with
        | "band" -> yield! Band.bandStatisticsSubScene statisticsApp
        | "albums" -> yield! Albums.albumsStatisticsSubScene statisticsApp
        | _ -> yield NoOp
    }
