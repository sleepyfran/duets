module Simulation.Interactions.ConcertSpace

open Entities
open Simulation

(*
TODO: Move this logic to a simulation event when player goes to stage.
This is needed because if we leave it like this we have no way of seeing that
the band is inside a concert and showing that in the CLI before the first event
of the concert is done.
*)
let private concertIfOngoing state placeId =
    let situation =
        Queries.Situations.current state

    match situation with
    | Situation.InConcert ongoingConcert -> Some ongoingConcert
    | _ ->
        let band = Queries.Bands.currentBand state

        (* Check whether we have a concert scheduled and, if so, initialize a new OngoingConcert *)
        Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
        |> Option.map (fun scheduledConcert ->
            let concert =
                Concert.fromScheduled scheduledConcert

            { Events = []
              Points = 0<quality>
              Concert = concert })

let private instrumentInteractions state ongoingConcert =
    let characterBandMember =
        Queries.Bands.currentPlayableMember state

    let stringInstrumentsCommonInteractions =
        [ Interaction.Concert(ConcertInteraction.TuneInstrument ongoingConcert) ]

    match characterBandMember.Role with
    | Guitar ->
        [ Interaction.Concert(ConcertInteraction.GuitarSolo ongoingConcert) ]
        @ stringInstrumentsCommonInteractions
    | Bass ->
        [ Interaction.Concert(ConcertInteraction.BassSolo ongoingConcert) ]
        @ stringInstrumentsCommonInteractions
    | Drums ->
        [ Interaction.Concert(ConcertInteraction.DrumSolo ongoingConcert)
          Interaction.Concert(ConcertInteraction.AdjustDrums ongoingConcert) ]
    | Vocals ->
        [ Interaction.Concert(ConcertInteraction.MakeCrowdSing ongoingConcert)
          Interaction.Concert(ConcertInteraction.TakeMic ongoingConcert)
          Interaction.Concert(ConcertInteraction.PutMicOnStand ongoingConcert)
          Interaction.Concert(ConcertInteraction.SpinDrumsticks ongoingConcert) ]

/// Returns all interactions available in the current concert room.
let internal availableCurrently
    state
    placeId
    room
    navigationInteractions
    defaultInteractions
    =
    let ongoingConcert =
        concertIfOngoing state placeId

    match ongoingConcert with
    | Some ongoingConcert ->
        match room with
        | Room.Stage ->
            let instrumentSpecificInteractions =
                instrumentInteractions state ongoingConcert

            [ Interaction.Concert(
                  ConcertInteraction.FinishConcert ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.GiveSpeech ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.GreetAudience ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.PlaySong ongoingConcert)
              Interaction.Concert(ConcertInteraction.GetOffStage ongoingConcert)
              Interaction.Concert(ConcertInteraction.FaceBand ongoingConcert)
              Interaction.Concert(ConcertInteraction.FaceCrowd ongoingConcert) ]
            @ instrumentSpecificInteractions
        | Room.Backstage ->
            [ Interaction.Concert(ConcertInteraction.DoEncore ongoingConcert) ]
        | _ -> navigationInteractions @ defaultInteractions
    | None -> navigationInteractions @ defaultInteractions
