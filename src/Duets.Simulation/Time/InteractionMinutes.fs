module Duets.Simulation.Time.InteractionMinutes

open Duets.Entities
open Duets.Simulation

/// Returns the number of minutes that a given effect takes to perform.
let effectMinutes =
    function
    | AlbumStarted _
    | AlbumSongAdded _ -> 2 * Config.Time.minutesPerDayMoment
    | Ate _ -> 10<minute>
    | BookRead _ -> 45<minute>
    | CareerShiftPerformed(_, shiftDuration, _) ->
        shiftDuration |> Calendar.DayMoments.toMinutes
    | ConcertFinished _ -> Config.Time.minutesPerDayMoment
    | ConcertSoundcheckPerformed -> 30<minute>
    | Drank _ -> 5<minute>
    | FlightLanded flight ->
        Queries.Flights.flightTime flight |> Calendar.Seconds.toMinutes
    | GamePlayed _ -> 25<minute>
    | MerchStandSetup -> 25<minute>
    | MiniGamePlayed _ -> 30<minute>
    | SocialActionPerformed(_, actionKind) ->
        match actionKind with
        | SocialActionKind.Greet -> 5<minute>
        | SocialActionKind.Chat -> 15<minute>
        | SocialActionKind.AskAboutDay -> 10<minute>
        | SocialActionKind.TellStory -> 30<minute>
    | SongImproved _
    | SongPracticed _
    | SongStarted _ -> 90<minute>
    | WatchedTv _ -> 15<minute>
    | Wait dayMoments -> dayMoments |> Calendar.DayMoments.toMinutes
    | _ -> 0<minute>
