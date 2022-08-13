module Simulation.Albums.DailyUpdate

open Entities
open Simulation.Queries
open Simulation.Albums.DailyStreams
open Simulation.Albums.FanIncrease
open Simulation.Albums.Hype
open Simulation.Albums.Revenue
open Simulation.Bank.Operations

let private bandDailyUpdate state bandId albumsByBand =
    let band = Bands.byId state bandId
    let bandAccount = Band band.Id

    albumsByBand
    |> List.map (fun album ->
        let previousDayFanStreams, previousDayNonFanStreams =
            dailyStreams state album

        let previousDayStreams =
            previousDayFanStreams + previousDayNonFanStreams

        let streams = previousDayStreams + album.Streams

        let dailyRevenue = albumRevenue previousDayStreams

        let recalculatedHype = reduceDailyHype album

        let fanIncrease = calculateFanIncrease previousDayNonFanStreams

        [ yield
              AlbumReleasedUpdate(
                  band,
                  Album.Released.update album streams recalculatedHype
              )

          if dailyRevenue > 0<dd> then
              yield (income state bandAccount dailyRevenue)

          if fanIncrease > 0 then
              yield
                  BandFansChanged(
                      band,
                      Diff(band.Fans, band.Fans + fanIncrease)
                  ) ])
    |> List.concat

/// Performs the daily update of albums from all bands. This generates the
/// daily streams of the album, updates the hype and generates the revenue
/// based on the daily streams.
let dailyUpdate state =
    Albums.allReleased state
    |> Map.fold
        (fun acc bandId albums -> acc @ bandDailyUpdate state bandId albums)
        []
