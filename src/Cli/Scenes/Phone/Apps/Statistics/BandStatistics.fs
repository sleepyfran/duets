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
          Phone.statisticsAppBandFameHeader ]

    let tableRows =
        [ Phone.statisticsAppBandName band.Name
          Phone.statisticsAppBandStartDate band.StartDate
          Phone.statisticsAppBandFame band.Fame ]

    showTable tableColumns [ tableRows ]

    statisticsApp ()
