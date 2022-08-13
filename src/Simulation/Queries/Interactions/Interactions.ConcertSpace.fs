namespace Simulation.Queries.Internal.Interactions


open Entities
open Simulation

module ConcertSpace =
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
    let internal availableCurrently
        state
        room
        navigationInteractions
        genericRoomInteractions
        defaultInteractions
        =
        let situation = Queries.Situations.current state

        match situation with
        | InConcert ongoingConcert ->
            match room with
            | RoomType.Stage ->
                let instrumentSpecificInteractions =
                    instrumentInteractions state ongoingConcert

                [ Interaction.Concert(
                      ConcertInteraction.FinishConcert ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.GiveSpeech ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.GreetAudience ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.PlaySong ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.GetOffStage ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.FaceBand ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.FaceCrowd ongoingConcert
                  ) ]
                @ instrumentSpecificInteractions
            | RoomType.Backstage ->
                [ Interaction.Concert(
                      ConcertInteraction.DoEncore ongoingConcert
                  )
                  Interaction.Concert(
                      ConcertInteraction.FinishConcert ongoingConcert
                  ) ]
            | _ ->
                navigationInteractions
                @ genericRoomInteractions @ defaultInteractions
        | _ ->
            navigationInteractions
            @ genericRoomInteractions @ defaultInteractions
