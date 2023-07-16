namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

module ConcertSpace =
    let private startConcertInteraction state placeId =
        let band = Queries.Bands.currentBand state

        Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
        |> Option.map (fun concert ->
            [ Interaction.Concert(ConcertInteraction.StartConcert concert) ])
        |> Option.defaultValue []

    let private instrumentInteractions state ongoingConcert =
        let characterBandMember = Queries.Bands.currentPlayableMember state

        let stringInstrumentsCommonInteractions =
            [ Interaction.Concert(
                  ConcertInteraction.TuneInstrument ongoingConcert
              ) ]

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
            [ Interaction.Concert(
                  ConcertInteraction.MakeCrowdSing ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.TakeMic ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.PutMicOnStand ongoingConcert
              )
              Interaction.Concert(
                  ConcertInteraction.SpinDrumsticks ongoingConcert
              ) ]

    /// Returns all interactions available in the current concert room.
    let internal interactions state roomType defaultInteractions cityId placeId =
        let situation = Queries.Situations.current state

        match situation with
        | FreeRoam when roomType = RoomType.Stage ->
            defaultInteractions @ startConcertInteraction state placeId
        | Concert(InConcert ongoingConcert) when roomType = RoomType.Stage ->
            let instrumentSpecificInteractions =
                instrumentInteractions state ongoingConcert

            [ Interaction.Concert(ConcertInteraction.GiveSpeech ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.GreetAudience ongoingConcert
              )
              Interaction.Concert(ConcertInteraction.PlaySong ongoingConcert)
              Interaction.Concert(ConcertInteraction.GetOffStage ongoingConcert)
              Interaction.Concert(ConcertInteraction.FaceBand ongoingConcert)
              Interaction.Concert(ConcertInteraction.FaceCrowd ongoingConcert) ]
            @ instrumentSpecificInteractions
        | Concert(InConcert ongoingConcert) when roomType = RoomType.Backstage ->
            let backstageAllowedInteractions =
                Queries.InteractionCommon.filterOutMovementAndTime
                    defaultInteractions

            [ yield! backstageAllowedInteractions
              Interaction.Concert(ConcertInteraction.DoEncore ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.FinishConcert ongoingConcert
              ) ] (* TODO: Add interactions that are specific to only the backstage outside a concert. *)
        | _ -> Bar.interactions cityId roomType @ defaultInteractions
