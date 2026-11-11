module Duets.Simulation.Tests.Interactions.ConcertSpace

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Data.World
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let private merch =
    { Brand = "DuetsMerch"
      Name = "T-shirt"
      Properties = [ Wearable TShirt ] }

let private preparingIn roomId =
    { dummyState with
        CurrentPosition = Prague, dummyVenue.Id, roomId
        Situation =
            Concert(
                Preparing
                    { MerchStandSetup = false
                      SoundcheckDone = false }
            ) }
    |> State.Inventory.addToBand dummyBand.Id merch 10<quantity>

let private canSetupMerchStand state =
    Queries.Interactions.availableCurrently state
    |> List.exists (fun interaction ->
        match interaction.Interaction with
        | Interaction.Concert(ConcertInteraction.SetupMerchStand _) -> true
        | _ -> false)

[<Test>]
let ``merch stand can be set up in the lobby while preparing a concert`` () =
    preparingIn Ids.Common.lobby |> canSetupMerchStand |> should equal true

[<Test>]
let ``merch stand cannot be set up in the bar`` () =
    preparingIn Ids.Common.bar |> canSetupMerchStand |> should equal false

let private inConcertOnStageAs roles =
    let band =
        { dummyBand with
            Members = [ Band.Member.from dummyCharacter.Id roles dummyToday ] }

    match Setup.startGame dummyCharacter band [] dummyCity with
    | GameCreated state ->
        { state with
            CurrentPosition = Prague, dummyVenue.Id, Ids.ConcertSpace.stage
            Situation = Concert(InConcert dummyOngoingConcert) }
    | _ -> failwith "Unexpected effect while creating game."

let private concertInteractions state =
    Queries.Interactions.availableCurrently state
    |> List.choose (fun interaction ->
        match interaction.Interaction with
        | Interaction.Concert concertInteraction -> Some concertInteraction
        | _ -> None)

[<Test>]
let ``member with multiple roles gets the concert interactions of all of them``
    ()
    =
    let interactions =
        inConcertOnStageAs [ Guitar; Vocals ] |> concertInteractions

    interactions
    |> List.exists (fun i -> i = ConcertInteraction.Solo(dummyOngoingConcert, Guitar))
    |> should equal true

    interactions
    |> List.exists (fun i -> i = ConcertInteraction.TakeMic dummyOngoingConcert)
    |> should equal true

[<Test>]
let ``interactions shared between roles are only listed once`` () =
    inConcertOnStageAs [ Guitar; Vocals ]
    |> concertInteractions
    |> List.filter (fun i ->
        i = ConcertInteraction.TuneInstrument dummyOngoingConcert)
    |> List.length
    |> should equal 1

[<Test>]
let ``drummers get a drum solo and can adjust their drums but not tune them`` () =
    let interactions = inConcertOnStageAs [ Drums ] |> concertInteractions

    interactions
    |> should contain (ConcertInteraction.Solo(dummyOngoingConcert, Drums))

    interactions
    |> should contain (ConcertInteraction.AdjustDrums dummyOngoingConcert)

    interactions
    |> should not' (contain (ConcertInteraction.TuneInstrument dummyOngoingConcert))

[<Test>]
let ``guitar and harmonica players get a solo for each instrument`` () =
    let interactions =
        inConcertOnStageAs [ Guitar; Harmonica ] |> concertInteractions

    interactions
    |> should contain (ConcertInteraction.Solo(dummyOngoingConcert, Guitar))

    interactions
    |> should contain (ConcertInteraction.Solo(dummyOngoingConcert, Harmonica))

[<Test>]
let ``violinists can tune their instrument`` () =
    inConcertOnStageAs [ Violin ]
    |> concertInteractions
    |> should contain (ConcertInteraction.TuneInstrument dummyOngoingConcert)

[<Test>]
let ``saxophonists get a solo but can't tune their instrument`` () =
    let interactions = inConcertOnStageAs [ Saxophone ] |> concertInteractions

    interactions
    |> should contain (ConcertInteraction.Solo(dummyOngoingConcert, Saxophone))

    interactions
    |> should not' (contain (ConcertInteraction.TuneInstrument dummyOngoingConcert))
