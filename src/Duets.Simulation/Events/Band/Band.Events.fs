module rec Duets.Simulation.Events.Band.Band

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Events

/// Runs all the events associated with bands. For example, when the fan base
/// changes, the engine might generate new reviews for their albums.
let internal run effect =
    match effect with
    | BandFansChanged(band, Diff(prevFans, currentFans)) ->
        let previousFans = Queries.Bands.totalFans prevFans
        let currentFans = Queries.Bands.totalFans currentFans

        let minimumFanBaseForReviews =
            Config.MusicSimulation.minimumFanBaseForReviews * 1<fans>

        let hasEnoughFansForReviews =
            previousFans < minimumFanBaseForReviews
            && currentFans >= minimumFanBaseForReviews

        if hasEnoughFansForReviews then
            [ Reviews.generateReviewsAfterFanIncrease band.Id ]
            |> ContinueChain
            |> Some
        else
            None
    | MemberHired(_, character, _, _) ->
        [ Relationships.addWithMember character ] |> ContinueChain |> Some
    | MemberFired(_, bandMember, _) ->
        [ Relationships.removeWithMember bandMember.CharacterId ]
        |> ContinueChain
        |> Some
    | _ -> None
