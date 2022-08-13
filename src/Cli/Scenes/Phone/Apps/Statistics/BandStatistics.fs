module Cli.Scenes.Phone.Apps.Statistics.Band

open Agents
open Cli.Components
open Cli.Text
open Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let tableColumns =
        [ Phone.statisticsAppBandNameHeader
          Phone.statisticsAppBandStartDateHeader
          Phone.statisticsAppBandFansHeader ]

    let tableRows =
        [ Phone.statisticsAppBandName band.Name
          Phone.statisticsAppBandStartDate band.StartDate
          $"{band.Fans}" ]

    showTable tableColumns [ tableRows ]

    statisticsApp ()
