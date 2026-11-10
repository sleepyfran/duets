[<AutoOpen>]
module Duets.Cli.Components.Scene.Room.Show

open Duets.Entities

/// Defines how the crowd feels during a concert.
[<RequireQualifiedAccess>]
type CrowdMood =
    | Bored
    | Warm
    | Hyped
    | Ecstatic

/// Defines what the audience sees during a concert.
type Show =
    {
        /// How full the venue is, from 0 to 1.
        Attendance: float
        Mood: CrowdMood
        /// Energy of the song being played, if any.
        Energy: PerformEnergy option
    }

[<RequireQualifiedAccess>]
module Show =
    let private songsPlayed (ongoingConcert: OngoingConcert) =
        ongoingConcert.Events
        |> List.filter (function
            | PlaySong _ -> true
            | _ -> false)
        |> List.length

    /// Returns the mood of the crowd based on the points of the concert.
    let moodOf (ongoingConcert: OngoingConcert) =
        let points = int ongoingConcert.Points

        match songsPlayed ongoingConcert with
        | 0 -> CrowdMood.Warm
        | songs ->
            let perSong = points / songs

            match perSong with
            | _ when points >= 75 || perSong >= 12 -> CrowdMood.Ecstatic
            | _ when points >= 50 || perSong >= 7 -> CrowdMood.Hyped
            | _ when perSong >= 3 -> CrowdMood.Warm
            | _ -> CrowdMood.Bored

    /// Creates the show of the given concert in a venue of the given capacity.
    let fromConcert capacity (ongoingConcert: OngoingConcert) =
        { Attendance =
            float ongoingConcert.Concert.TicketsSold / float (max 1 capacity)
            |> min 1.0
          Mood = moodOf ongoingConcert
          Energy = None }
