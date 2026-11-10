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
