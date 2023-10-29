module Duets.Simulation.Events.Moodlets.TiredOfTouring

open Duets.Common
open Duets.Entities
open Duets.Simulation

let private concertsToCheck = 5

/// Checks if the band has had at least a 7 day break between their last 4
/// concerts. If not, applies the TiredOfTouring moodlet.
let applyIfNeeded bandId state =
    let lastFourConcerts =
        Queries.Concerts.allPast state bandId
        |> Seq.sortBy (fun event ->
            let concert = Concert.fromPast event
            concert.Date)
        |> Seq.truncate concertsToCheck
        |> Seq.toList

    match lastFourConcerts with
    | concerts when concerts.Length = concertsToCheck ->
        (*
        Check that the span between the first and fourth concert has been of
        at least a week.
        *)
        let firstConcert = concerts |> List.head |> Concert.fromPast
        let lastConcert = concerts |> List.last |> Concert.fromPast

        let daysInBetween =
            Calendar.Query.daysBetween lastConcert.Date firstConcert.Date

        let shouldApplyMoodlet = daysInBetween < 7<days>

        if shouldApplyMoodlet then
            let moodlet =
                Character.Moodlets.createFromNow
                    state
                    MoodletType.TiredOfTouring
                    (MoodletExpirationTime.AfterDays 14<days>)

            [ Character.Moodlets.apply state moodlet ]
        else
            []
    | _ -> [] (* Not enough concerts have happened yet, do nothing. *)
