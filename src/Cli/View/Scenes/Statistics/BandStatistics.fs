module Cli.View.Scenes.Statistics.Band

open Cli.View.Actions
open Cli.View.Text
open Simulation.Queries

let bandStatisticsSubScene () =
    let state = State.Root.get ()
    let band = Bands.currentBand state

    seq {
        yield
            StatisticsBandName band.Name
            |> StatisticsText
            |> I18n.translate
            |> Message

        yield
            StatisticsBandStartDate band.StartDate
            |> StatisticsText
            |> I18n.translate
            |> Message

        yield
            StatisticsBandFame band.Fame
            |> StatisticsText
            |> I18n.translate
            |> Message

        yield Scene Statistics
    }
