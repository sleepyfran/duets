module Simulation.Albums.DailyUpdate

open Entities
open Simulation.Queries
open Simulation.Albums.DailyStreams
open Simulation.Albums.Hype
open Simulation.Albums.Revenue

let private bandDailyUpdate state bandId albumsByBand =
    let band = Bands.byId state bandId
    let bandAccount = Band band.Id

    albumsByBand
    |> List.map
        (fun album ->
            let streams = dailyStreams album + album.Streams
            let revenue = albumRevenue streams
            let recalculatedHype = reduceDailyHype album

            [ AlbumReleasedUpdate(
                band,
                Album.Released.update album streams recalculatedHype
              )
              MoneyEarned(bandAccount, Incoming revenue) ])
    |> List.concat

/// Performs the daily update of albums from all bands. This generates the
/// daily streams of the album, updates the hype and generates the revenue
/// based on the daily streams.
let dailyUpdate state =
    Albums.allReleased state
    |> Map.fold
        (fun acc bandId albums -> acc @ bandDailyUpdate state bandId albums)
        []
