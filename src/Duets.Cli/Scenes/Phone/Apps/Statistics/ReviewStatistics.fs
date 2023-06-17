module Duets.Cli.Scenes.Phone.Apps.Statistics.AlbumReviews

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Queries

let rec reviewsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let releases = Albums.releasedByBand state band.Id

    if List.isEmpty releases then
        Phone.statisticsAppAlbumNoEntries |> showMessage
        statisticsApp ()
    else
        showAlbumSelection statisticsApp releases

and private showAlbumSelection statisticsApp albums =
    let selection =
        showOptionalChoicePrompt
            "Which album do you want to see reviews for?"
            Generic.backToPhone
            (fun album -> album.Album.Name)
            albums

    match selection with
    | Some album ->
        if List.isEmpty album.Reviews then
            "No one really cared about the album that much to write a review"
            |> Styles.error
            |> showMessage
        else
            showReviews album
        
        reviewsStatisticsSubScene statisticsApp
    | None -> statisticsApp ()
