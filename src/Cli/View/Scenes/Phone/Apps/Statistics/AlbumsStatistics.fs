module Cli.View.Scenes.Phone.Apps.Statistics.Albums

open Agents
open Cli.View.Actions
open Cli.View.Text
open Simulation.Queries

let albumsStatisticsSubScene statisticsApp =
    let state = State.get ()
    let band = Bands.currentBand state
    let releases = Albums.releasedByBand state band.Id

    seq {
        if releases.IsEmpty then
            yield
                Message
                <| I18n.translate (PhoneText StatisticsAppAlbumNoEntries)
        else
            yield!
                releases
                |> Seq.map
                    (fun releasedAlbum ->
                        let innerAlbum = releasedAlbum.Album
                        let revenue = Albums.revenue releasedAlbum

                        seq {
                            yield Separator

                            yield
                                StatisticsAppAlbumName(
                                    innerAlbum.Name,
                                    innerAlbum.Type
                                )
                                |> PhoneText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAppAlbumReleaseDate
                                    releasedAlbum.ReleaseDate
                                |> PhoneText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAppAlbumStreams releasedAlbum.Streams
                                |> PhoneText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAppAlbumRevenue revenue
                                |> PhoneText
                                |> I18n.translate
                                |> Message
                        })
                |> Seq.concat

        yield! statisticsApp ()
    }
