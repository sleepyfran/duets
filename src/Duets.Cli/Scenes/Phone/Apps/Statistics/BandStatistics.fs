module Duets.Cli.Scenes.Phone.Apps.Statistics.Band

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let tableColumns =
        [ Phone.statisticsAppBandNameHeader
          Phone.statisticsAppBandGenreHeader
          Phone.statisticsAppBandStartDateHeader
          Phone.statisticsAppBandFansHeader ]

    let tableRows =
        [ Phone.statisticsAppBandName band.Name
          band.Genre
          Phone.statisticsAppBandStartDate band.StartDate
          Phone.statisticsAppBandFans band.Fans ]

    showTable tableColumns [ tableRows ]

    statisticsApp ()
