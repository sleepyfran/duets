module Duets.Simulation.Concerts.LateAlert

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Checks whether the current time matches a scheduled concert's start time
/// and, if so, returns a CharacterRunningLateToConcert effect to alert the
/// player. Fires exactly once: at the DayMoment transition to the concert slot.
let checkIfRunningLate state date =
    let currentBand = Queries.Bands.currentBand state

    let scheduledConcerts = Queries.Concerts.allScheduled state currentBand.Id

    scheduledConcerts
    |> Seq.choose (fun (ScheduledConcert(concert, _)) ->
        let concertDate =
            concert.Date
            |> Calendar.Transform.changeDayMoment concert.DayMoment

        if date = concertDate then
            let otherMembers =
                Queries.Bands.currentBandMembersWithoutPlayableCharacter state

            let callerOpt =
                otherMembers
                |> List.trySample
                |> Option.map (fun m -> Queries.Characters.find state m.CharacterId)

            Some(CharacterRunningLateToConcert(callerOpt, concert))
        else
            None)
    |> List.ofSeq
