module Cli.View.Scenes.Statistics.Albums

open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Queries

let albumsStatisticsSubScene state =
    let band = Bands.currentBand state
    let releases = Albums.releasedByBand state band.Id

    seq {
        if releases.IsEmpty then
            yield Message <| TextConstant StatisticsAlbumNoEntries
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
                                |> TextConstant
                                |> Message

                            yield
                                StatisticsAlbumReleaseDate
                                    releasedAlbum.ReleaseDate
                                |> TextConstant
                                |> Message

                            yield
                                StatisticsAlbumStreams releasedAlbum.Streams
                                |> TextConstant
                                |> Message

                            yield
                                StatisticsAlbumRevenue revenue
                                |> TextConstant
                                |> Message
                        })
                |> Seq.concat

        yield Scene Statistics
    }
