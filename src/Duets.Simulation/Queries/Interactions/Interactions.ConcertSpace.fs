namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
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

    let private setupMerchStandInteraction state checklist =
        let band = Queries.Bands.currentBand state
        let merch = Queries.Inventory.band state

        let itemsWithoutPrice =
            merch
            |> List.ofMapKeys
            |> List.choose (fun item ->
                let mainItemProperty =
                    Item.Property.tryMain item
                    |> Option.value (* Let's hope that we only pass items generated during the merch creation process. *)

                let assignedPrice =
                    Queries.Merch.itemPrice band.Id mainItemProperty state

                match assignedPrice with
                | Some _ -> None
                | None -> Some item)

        if merch |> Map.isEmpty then
            (* Nothing to do, band has not ordered any merch. *)
            []
        else if checklist.MerchStandSetup then
            (* Nothing to do, band has already setup the stand. *)
            []
        else
            [ ConcertInteraction.SetupMerchStand(checklist, itemsWithoutPrice)
              |> Interaction.Concert ]

    let private soundcheckInteraction checklist =
        if checklist.SoundcheckDone then
            (* Nothing to do, band has already done the soundcheck. *)
            []
        else
            [ ConcertInteraction.PerformSoundCheck(checklist)
              |> Interaction.Concert ]

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
    let internal interactions state roomType cityId placeId =
        let situation = Queries.Situations.current state

        match situation with
        | Concert(Preparing checklist) when roomType = RoomType.Bar ->
            setupMerchStandInteraction state checklist
        | Concert(Preparing checklist) when roomType = RoomType.Stage ->
            [ yield! soundcheckInteraction checklist
              yield! startConcertInteraction state placeId ]
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
            [ Interaction.Concert(ConcertInteraction.DoEncore ongoingConcert)
              Interaction.Concert(
                  ConcertInteraction.FinishConcert ongoingConcert
              ) ] (* TODO: Add interactions that are specific to only the backstage outside a concert. *)
        | _ -> Bar.interactions cityId roomType
