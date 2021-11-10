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
            let previousDayStreams = dailyStreams album
            let streams = previousDayStreams + album.Streams
            let dailyRevenue = albumRevenue previousDayStreams
            let recalculatedHype = reduceDailyHype album

            [ yield
                AlbumReleasedUpdate(
                    band,
                    Album.Released.update album streams recalculatedHype
                )
              if dailyRevenue > 0<dd> then
                  yield MoneyEarned(bandAccount, Incoming dailyRevenue) ])
    |> List.concat

/// Performs the daily update of albums from all bands. This generates the
/// daily streams of the album, updates the hype and generates the revenue
/// based on the daily streams.
let dailyUpdate state =
    Albums.allReleased state
    |> Map.fold
        (fun acc bandId albums -> acc @ bandDailyUpdate state bandId albums)
        []
