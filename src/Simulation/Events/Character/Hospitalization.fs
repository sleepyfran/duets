module Simulation.Events.Character.Hospitalization

open Common
open Entities
open Simulation

/// Hospitalizes the given character, cancelling any activity that they are doing
/// in the current moment.
let hospitalize character state =
    let currentPosition =
        Queries.World.Common.currentPosition state

    let situation =
        Queries.Situations.current state

    let concertCancellationEffects =
        match situation with
        (* When we are in the middle of a concert we first need to cancel it. *)
        | Situation.InConcert ongoingConcert ->
            let failConcertEffect =
                Concerts.Scheduler.failConcert
                    state
                    ongoingConcert.Concert
                    CharacterPassedOut
                |> List.ofItem

            failConcertEffect @ [ Situations.freeRoam ]
        | _ -> []

    let hospitalCoordinates =
        Queries.World.Common.coordinatesOf state SpaceTypeIndex.Hospital
        |> List.head
        |> Node
        |> Tuple.two currentPosition.City.Id

    let oneWeekLater =
        Queries.Calendar.today state
        |> Calendar.Ops.addDays 7

    concertCancellationEffects
    @ [ CharacterHealthDepleted character
        CharacterHospitalized(character, hospitalCoordinates)
        TimeAdvanced oneWeekLater
        CharacterAttributeChanged(character, CharacterAttribute.Health, 100) ]
