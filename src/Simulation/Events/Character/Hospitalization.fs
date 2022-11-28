module Simulation.Events.Character.Hospitalization

open Common
open Entities
open Simulation

/// Hospitalizes the given character, cancelling any activity that they are doing
/// in the current moment.
let hospitalize characterId state =
    let currentCity =
        Queries.World.currentCity state

    let situation =
        Queries.Situations.current state

    let concertCancellationEffects =
        match situation with
        (* When we are in the middle of a concert we first need to cancel it. *)
        | Concert (InConcert ongoingConcert)
        | Concert (InBackstage (Some ongoingConcert)) ->
            let failConcertEffect =
                Concerts.Scheduler.failConcert
                    state
                    ongoingConcert.Concert
                    CharacterPassedOut
                |> List.ofItem

            failConcertEffect @ [ Situations.freeRoam ]
        | _ -> []

    let hospitalCoordinates =
        Queries.World.placeIdsOf currentCity.Id PlaceTypeIndex.Hospital
        |> List.head (* We need at least one hospital in the city. *)
        |> Tuple.two currentCity.Id

    let oneWeekLater =
        Queries.Calendar.today state
        |> Calendar.Ops.addDays 7

    concertCancellationEffects
    @ [ CharacterHealthDepleted characterId
        CharacterHospitalized(characterId, hospitalCoordinates)
        TimeAdvanced oneWeekLater
        CharacterAttributeChanged(
            characterId,
            CharacterAttribute.Health,
            Diff(0, 100)
        ) ]
