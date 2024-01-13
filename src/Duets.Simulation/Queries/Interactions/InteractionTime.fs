namespace Duets.Simulation.Queries

open Duets.Entities

module InteractionTime =
    /// Returns how many day moments a certain interaction should advance.
    let timeRequired interaction =
        match interaction with
        | Interaction.Airport(AirportInteraction.WaitUntilLanding flight) ->
            Flights.flightDayMoments flight
        | Interaction.Career(CareerInteraction.Work job) ->
            Career.jobDuration job
        | Interaction.Concert(ConcertInteraction.SetupMerchStand _) ->
            1<dayMoments>
        | Interaction.Concert(ConcertInteraction.FinishConcert _) ->
            2<dayMoments>
        | Interaction.Item(itemInteraction) ->
            match itemInteraction with
            | ItemInteraction.Exercise
            | ItemInteraction.Play
            | ItemInteraction.Read
            | ItemInteraction.Watch -> 1<dayMoments>
            | ItemInteraction.Cook _
            | ItemInteraction.Drink
            | ItemInteraction.Eat
            | ItemInteraction.Open
            | ItemInteraction.Put
            | ItemInteraction.Sleep (* Sleeping asks how long to sleep. *) ->
                0<dayMoments>
        | Interaction.FreeRoam FreeRoamInteraction.Wait -> 1<dayMoments>
        | Interaction.MiniGame(MiniGameInteraction.StartGame _)
        | Interaction.MiniGame(MiniGameInteraction.InGame(MiniGameInGameInteraction.Leave _)) ->
            1<dayMoments>
        | Interaction.Rehearsal rehearsalInteraction ->
            match rehearsalInteraction with
            | RehearsalInteraction.ComposeNewSong
            | RehearsalInteraction.ImproveSong _
            | RehearsalInteraction.PracticeSong _ -> 1<dayMoments>
            | _ -> 0<dayMoments>
        | Interaction.Studio studioInteraction ->
            match studioInteraction with
            | StudioInteraction.CreateAlbum _
            | StudioInteraction.AddSongToAlbum _ -> 2<dayMoments>
            | _ -> 0<dayMoments>
        | Interaction.Social socialInteraction ->
            match socialInteraction with
            | SocialInteraction.StartConversation _
            | SocialInteraction.StopConversation -> 1<dayMoments>
            | _ -> 0<dayMoments>
        | _ -> 0<dayMoments>
