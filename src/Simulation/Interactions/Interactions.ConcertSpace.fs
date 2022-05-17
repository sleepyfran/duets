module Simulation.Interactions.ConcertSpace

open Entities
open Simulation

/// Returns all interactions available in the current concert room.
let internal availableCurrently state room =
    let situation =
        Queries.Situations.current state

    match room with
    | Room.Stage ->
        match situation with
        | Situation.InConcert ongoingConcert ->
            [ Interaction.Concert(
                  ConcertInteraction.FinishConcert ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.GiveSpeech ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.GreetAudience ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.PlaySong ongoingConcert)
              Interaction.Concert(ConcertInteraction.GetOffStage ongoingConcert) ]
        | _ -> []
    | Room.Backstage ->
        match situation with
        | Situation.InConcert ongoingConcert ->
            [ Interaction.Concert(ConcertInteraction.DoEncore ongoingConcert) ]
        | _ -> []
    | _ -> [ Interaction.FreeRoam FreeRoamInteraction.Wait ]
