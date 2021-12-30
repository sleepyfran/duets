module Cli.View.Scenes.Statistics.Albums

open Cli.View.Actions
open Cli.View.Text
open Simulation.Queries

let albumsStatisticsSubScene () =
    let state = State.Root.get ()
    let band = Bands.currentBand state
    let releases = Albums.releasedByBand state band.Id

    seq {
        if releases.IsEmpty then
            yield
                Message
                <| I18n.translate (StatisticsText StatisticsAlbumNoEntries)
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
                                StatisticsAlbumName(
                                    innerAlbum.Name,
                                    innerAlbum.Type
                                )
                                |> StatisticsText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAlbumReleaseDate
                                    releasedAlbum.ReleaseDate
                                |> StatisticsText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAlbumStreams releasedAlbum.Streams
                                |> StatisticsText
                                |> I18n.translate
                                |> Message

                            yield
                                StatisticsAlbumRevenue revenue
                                |> StatisticsText
                                |> I18n.translate
                                |> Message
                        })
                |> Seq.concat

        yield Scene Statistics
    }
