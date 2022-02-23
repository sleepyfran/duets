module Cli.Scenes.Phone.Apps.Statistics.Albums

open Agents
open Cli.Components
open Cli.Text
open Simulation.Queries

let rec albumsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state
    let releases = Albums.releasedByBand state band.Id

    if List.isEmpty releases then
        PhoneText StatisticsAppAlbumNoEntries
        |> I18n.translate
        |> showMessage
    else
        showAlbums releases

    statisticsApp ()

and private showAlbums releases =
    releases
    |> List.iter (fun releasedAlbum ->
        let innerAlbum = releasedAlbum.Album
        let revenue = Albums.revenue releasedAlbum

        showSeparator None

        StatisticsAppAlbumName(innerAlbum.Name, innerAlbum.Type)
        |> PhoneText
        |> I18n.translate
        |> showMessage

        StatisticsAppAlbumReleaseDate releasedAlbum.ReleaseDate
        |> PhoneText
        |> I18n.translate
        |> showMessage

        StatisticsAppAlbumStreams releasedAlbum.Streams
        |> PhoneText
        |> I18n.translate
        |> showMessage

        StatisticsAppAlbumRevenue revenue
        |> PhoneText
        |> I18n.translate
        |> showMessage)
