module Duets.Simulation.Social.Actions

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Social.Common

/// Starts a conversation with the given NPC by setting the current situation to
/// socializing.
let startConversation state (npc: Character) =
    let currentRelationship = Queries.Relationship.withCharacter npc.Id state

    { Npc = npc
      Relationship = currentRelationship
      Actions = [] }
    |> Situations.socializing

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
