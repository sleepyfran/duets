module Duets.Simulation.Social.Actions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Social.Common
open Duets.Simulation.Time

/// Starts a conversation with the given NPC by setting the current situation to
/// socializing.
let startConversation state (npc: Character) =
    let currentRelationship = Queries.Relationship.withCharacter npc.Id state

    { Npc = npc
      Relationship = currentRelationship
      Actions = [] }
    |> Situations.socializing

/// Stops the current conversation if the player is currently socializing,
/// otherwise, does nothing.
let stopConversation state =
    let currentSituation = Queries.Situations.current state

    match currentSituation with
    | Socializing socializingState ->
        let timeAdvanceEffects =
            Queries.InteractionTime.timeRequired (
                SocialInteraction.StopConversation |> Interaction.Social
            )
            |> AdvanceTime.advanceDayMoment' state

        let relationshipUpdateEffects =
            match socializingState.Relationship with
            | Some relationship ->
                [ (relationship.Character,
                   relationship.MeetingCity,
                   Some relationship)
                  |> RelationshipChanged ]
            | None -> []

        [ yield! relationshipUpdateEffects
          yield! timeAdvanceEffects
          Situations.freeRoam ]
    | _ -> [] (* Not socializing, nothing to do. *)

/// Greets the NPC of the current conversation.
let greet state socializingState =
    { Kind = SocialActionKind.Greet
      Limit = Penalized(1<times>, Negative(5))
      RelationshipChange = NoChange }
    |> performAction state socializingState

/// Has a chat with the NPC of the current conversation.
let chat state socializingState =
    { Kind = SocialActionKind.Chat
      Limit = NoAction(5<times>)
      RelationshipChange = Positive(1) }
    |> performAction state socializingState

/// Asks the NPC of the current conversation about their day.
let askAboutDay state socializingState =
    { Kind = SocialActionKind.AskAboutDay
      Limit = Penalized(1<times>, Negative(5))
      RelationshipChange = Positive(2) }
    |> performAction state socializingState
