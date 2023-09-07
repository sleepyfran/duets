module Duets.Cli.Scenes.Phone.Apps.Statistics.Albums

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Simulation.Queries

let rec albumsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state

    let releases = Albums.releasedByBand state band.Id

    if List.isEmpty releases then
        Phone.statisticsAppAlbumNoEntries |> showMessage
    else
        showAlbums releases

    statisticsApp ()

and private showAlbums releases =
    let tableColumns =
        [ Phone.statisticsAppAlbumNameHeader
          Phone.statisticsAppAlbumTypeHeader
          Phone.statisticsAppAlbumGenreHeader
          Phone.statisticsAppAlbumReleaseDateHeader
          Phone.statisticsAppAlbumStreamsHeader
          Phone.statisticsAppAlbumRevenueHeader ]

    let tableRows =
        releases
        |> List.map (fun releasedAlbum ->
            let innerAlbum = releasedAlbum.Album
            let revenue = Albums.revenue releasedAlbum

            [ Phone.statisticsAppAlbumName innerAlbum.Name
              Phone.statisticsAppAlbumType innerAlbum.Type
              Phone.statisticsAppGenre innerAlbum.Genre
              Phone.statisticsAppAlbumReleaseDate releasedAlbum.ReleaseDate
              Phone.statisticsAppAlbumStreams releasedAlbum.Streams
              Phone.statisticsAppAlbumRevenue revenue ])

    showTable tableColumns tableRows
