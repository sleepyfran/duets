module Cli.Scenes.Phone.Apps.Statistics.Band

open Agents
open Cli.Components
open Cli.Text
open Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    StatisticsAppBandName band.Name
    |> PhoneText
    |> I18n.translate
    |> showMessage

    StatisticsAppBandStartDate band.StartDate
    |> PhoneText
    |> I18n.translate
    |> showMessage

    StatisticsAppBandFame band.Fame
    |> PhoneText
    |> I18n.translate
    |> showMessage

    statisticsApp ()
