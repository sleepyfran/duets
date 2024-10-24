module Duets.Simulation.Albums.DailyUpdate

open Duets.Entities
open Duets.Simulation.Queries
open Duets.Simulation.Albums.DailyStreams
open Duets.Simulation.Albums.FanIncrease
open Duets.Simulation.Albums.Hype
open Duets.Simulation.Albums.Revenue
open Duets.Simulation.Bank.Operations

let private bandDailyUpdate state bandId albumsByBand =
    let band = Bands.ofCharacterById state bandId
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
        let updatedFanBase = applyFanIncrease band fanIncrease

        [ yield
              AlbumReleasedUpdate(
                  band,
                  Album.Released.update album streams recalculatedHype
              )

          if dailyRevenue > 0m<dd> then
              yield (income state bandAccount dailyRevenue)

          if fanIncrease > 0<fans> then
              yield BandFansChanged(band, Diff(band.Fans, updatedFanBase)) ])
    |> List.concat

/// Performs the daily update of albums from all bands. This generates the
/// daily streams of the album, updates the hype and generates the revenue
/// based on the daily streams.
let dailyUpdate state =
    Albums.allReleased state
    |> Map.fold
        (fun acc bandId albums -> acc @ bandDailyUpdate state bandId albums)
        []
