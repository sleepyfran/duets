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
        | Interaction.Concert(ConcertInteraction.FinishConcert _) ->
            2<dayMoments>
        | Interaction.Item(ItemInteraction.Interactive interactiveInteraction) ->
            match interactiveInteraction with
            | InteractiveItemInteraction.Sleep -> 2<dayMoments>
            | InteractiveItemInteraction.Play
            | InteractiveItemInteraction.Watch -> 1<dayMoments>
            | InteractiveItemInteraction.Cook _ -> 0<dayMoments>
        | Interaction.FreeRoam FreeRoamInteraction.Wait -> 1<dayMoments>
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
        | _ -> 0<dayMoments>
