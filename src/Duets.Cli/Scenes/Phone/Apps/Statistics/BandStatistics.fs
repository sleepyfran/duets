module Duets.Cli.Scenes.Phone.Apps.Statistics.Band

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Simulation

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Queries.Bands.currentBand state
    let estimatedFame = Queries.Bands.estimatedFameLevel state band.Id

    let tableColumns =
        [ Styles.header "Name"
          Styles.header "Genre"
          Styles.header "Start Date"
          Styles.header "Fans around the world" ]

    let totalFans = Queries.Bands.totalFans' band

    let tableRows =
        [ Styles.title band.Name
          band.Genre
          Styles.highlight band.StartDate.Year
          $"""{Styles.number totalFans} ({estimatedFame |> Styles.Level.from}%% fame)""" ]

    showTable tableColumns [ tableRows ]

    statisticsApp ()
