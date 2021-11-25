module Cli.View.Scenes.Statistics.Band

open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Queries

let bandStatisticsSubScene state =
    let band = Bands.currentBand state

    seq {
        yield
            StatisticsBandName band.Name
            |> TextConstant
            |> Message

        yield
            StatisticsBandStartDate band.StartDate
            |> TextConstant
            |> Message

        yield
            StatisticsBandFame band.Fame
            |> TextConstant
            |> Message
            
        yield Scene Statistics
    }
