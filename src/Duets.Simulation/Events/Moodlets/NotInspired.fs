module Duets.Simulation.Events.Moodlets.NotInspired

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Checks if the band has had at least a 7 day break in between finishing
/// two songs, otherwise gives a NotInspired moodlet to the playable character
/// to ensure that they slow down.
let applyIfNeeded bandId state =
    (*
    The state we get is already updated with the latest song, so instead of using
    it directly just query for the last two composed songs.
    *)
    let lastTwoFinishedSongs =
        Queries.Songs.finishedNonRecordedByBand state bandId
        |> List.ofMapValues
        |> List.sortByDescending Song.Finished.Metadata.finishDate
        |> List.truncate 2

    match lastTwoFinishedSongs with
    | [] -> [] (* This shouldn't happen, but do nothing. *)
    | [ _ ] -> [] (* This was the band's first song, do nothing. *)
    | [ lastSong; latestSong ] ->
        (*
        There's at least two songs, check that enough time has passed between them.
        *)
        let lastSongFinishDate = Song.Finished.Metadata.finishDate lastSong
        let latestSongFinishDate = Song.Finished.Metadata.finishDate latestSong

        let daysSinceLastSong =
            Calendar.Query.daysBetween lastSongFinishDate latestSongFinishDate

        let shouldApplyMoodlet =
            daysSinceLastSong
            <= Config.MusicSimulation.daysBetweenSongsToSlowDown

        if shouldApplyMoodlet then
            let moodlet =
                Character.Moodlets.createFromNow
                    state
                    MoodletType.NotInspired
                    (MoodletExpirationTime.AfterDays 7<days>)

            [ Character.Moodlets.apply state moodlet ]
        else
            []
    | _ -> [] (* This shouldn't happen, but do nothing. *)
