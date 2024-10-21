module rec Duets.Cli.Scenes.Phone.Apps.Statistics.Band

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

let bandStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Queries.Bands.currentBand state
    let totalFans = Queries.Bands.totalFans' band

    showOverviewTable state band totalFans

    if totalFans > 0<fans> then
        showFanDetailTable band

    statisticsApp ()

let private showOverviewTable state band totalFans =
    let estimatedFame = Queries.Bands.estimatedFameLevel state band.Id

    let tableColumns =
        [ Styles.header "Name"
          Styles.header "Genre"
          Styles.header "Start Date"
          Styles.header "Fans around the world" ]

    let tableRows =
        [ Styles.title band.Name
          band.Genre
          Styles.highlight band.StartDate.Year
          $"""{Styles.number totalFans} ({estimatedFame |> Styles.Level.from}%% fame)""" ]

    showTable tableColumns [ tableRows ]

let private showFanDetailTable band =
    let tableColumns = [ Styles.header "City"; Styles.header "Fans" ]

    let tableRows =
        band.Fans
        |> Map.fold
            (fun acc cityId fans ->
                acc @ [ [ Generic.cityName cityId; fans |> Styles.number ] ])
            []

    showTable tableColumns tableRows
