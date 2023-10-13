module Duets.Cli.Scenes.Phone.Apps.Statistics.Band

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Simulation.Queries

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state
    let estimatedFame = Bands.estimatedFameLevel state band.Id

    let tableColumns =
        [ Styles.header "Name"
          Styles.header "Genre"
          Styles.header "Start Date"
          Styles.header "Fans" ]

    let tableRows =
        [ Styles.title band.Name
          band.Genre
          Styles.highlight band.StartDate.Year
          $"""{Styles.number band.Fans} ({estimatedFame |> Styles.Level.from}%% fame)""" ]

    showTable tableColumns [ tableRows ]

    statisticsApp ()
