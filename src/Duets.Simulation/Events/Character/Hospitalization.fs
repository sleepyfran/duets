module Duets.Simulation.Events.Character.Hospitalization

open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

/// Hospitalizes the given character, cancelling any activity that they are doing
/// in the current moment.
let hospitalize characterId state =
    let currentCity = Queries.World.currentCity state

    let situation = Queries.Situations.current state

    let concertCancellationEffects =
        match situation with
        (* When we are in the middle of a concert we first need to cancel it. *)
        | Concert(InConcert ongoingConcert) ->
            let failConcertEffect =
                Concerts.Scheduler.failConcert
                    state
                    ongoingConcert.Concert
                    CharacterPassedOut
                |> List.ofItem

            failConcertEffect @ [ Situations.freeRoam ]
        | _ -> []

    let hospitalCoordinates =
        Queries.World.placeIdsByTypeInCity
            currentCity.Id
            PlaceTypeIndex.Hospital
        |> List.head (* We need at least one hospital in the city. *)
        |> Tuple.two currentCity.Id

    concertCancellationEffects
    @ [ CharacterHealthDepleted characterId
        CharacterHospitalized(characterId, hospitalCoordinates)
        CharacterAttributeChanged(
            characterId,
            CharacterAttribute.Health,
            Diff(0, 100)
        )
        CharacterAttributeChanged(
            characterId,
            CharacterAttribute.Hunger,
            Diff(0, 100)
        ) ]
