module Cli.Scenes.Phone.Apps.Statistics.Albums

open Agents
open Cli.Components
open Cli.Text
open Simulation.Queries

let rec albumsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let releases =
        Albums.releasedByBand state band.Id

    if List.isEmpty releases then
        PhoneText StatisticsAppAlbumNoEntries
        |> I18n.translate
        |> showMessage
    else
        showAlbums releases

    statisticsApp ()

and private showAlbums releases =
    let tableColumns =
        [ StatisticsAppAlbumNameHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppAlbumTypeHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppAlbumReleaseDateHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppAlbumStreamsHeader
          |> PhoneText
          |> I18n.translate

          StatisticsAppAlbumRevenueHeader
          |> PhoneText
          |> I18n.translate ]

    let tableRows =
        releases
        |> List.fold
            (fun acc releasedAlbum ->
                let innerAlbum = releasedAlbum.Album
                let revenue = Albums.revenue releasedAlbum

                acc
                @ [ StatisticsAppAlbumName innerAlbum.Name
                    |> PhoneText
                    |> I18n.translate

                    StatisticsAppAlbumType innerAlbum.Type
                    |> PhoneText
                    |> I18n.translate

                    StatisticsAppAlbumReleaseDate releasedAlbum.ReleaseDate
                    |> PhoneText
                    |> I18n.translate

                    StatisticsAppAlbumStreams releasedAlbum.Streams
                    |> PhoneText
                    |> I18n.translate

                    StatisticsAppAlbumRevenue revenue
                    |> PhoneText
                    |> I18n.translate ])
            []

    showTable tableColumns tableRows
