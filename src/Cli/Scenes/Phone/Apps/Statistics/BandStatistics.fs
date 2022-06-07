module Cli.Scenes.Phone.Apps.Statistics.Band

open Agents
open Cli.Components
open Cli.Text
open Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let tableColumns =
        [ StatisticsAppBandNameHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppBandStartDateHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppBandFameHeader
          |> PhoneText
          |> I18n.translate ]

    let tableRows =
        [ StatisticsAppBandName band.Name
          |> PhoneText
          |> I18n.translate

          StatisticsAppBandStartDate band.StartDate
          |> PhoneText
          |> I18n.translate

          StatisticsAppBandFame band.Fame
          |> PhoneText
          |> I18n.translate ]

    showTable tableColumns tableRows

    statisticsApp ()
