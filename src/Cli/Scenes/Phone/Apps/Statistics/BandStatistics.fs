module Cli.Scenes.Phone.Apps.Statistics.Band

open Agents
open Cli.Actions
open Cli.Text
open Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    seq {
        yield
            StatisticsAppBandName band.Name
            |> PhoneText
            |> I18n.translate
            |> Message

        yield
            StatisticsAppBandStartDate band.StartDate
            |> PhoneText
            |> I18n.translate
            |> Message

        yield
            StatisticsAppBandFame band.Fame
            |> PhoneText
            |> I18n.translate
            |> Message

        yield! statisticsApp ()
    }
