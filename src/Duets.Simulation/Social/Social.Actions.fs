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
        let relationshipUpdateEffects =
            match socializingState.Relationship with
            | Some relationship ->
                [ (socializingState.Npc,
                   relationship.MeetingCity,
                   Some relationship)
                  |> RelationshipChanged ]
            | None -> []

        [ yield! relationshipUpdateEffects; Situations.freeRoam ]
    | _ -> [] (* Not socializing, nothing to do. *)

/// Greets the NPC of the current conversation.
let greet state socializingState =
    { Kind = SocialActionKind.Greet
      Limit = Penalized(1<times>, Negative(5))
      RelationshipChange = NoChange
      MinimumLevel = 0<relationshipLevel> }
    |> performAction state socializingState

/// Has a chat with the NPC of the current conversation.
let chat state socializingState =
    { Kind = SocialActionKind.Chat
      Limit = NoAction(5<times>)
      RelationshipChange = Positive(1)
      MinimumLevel = 0<relationshipLevel> }
    |> performAction state socializingState

/// Asks the NPC of the current conversation about their day.
let askAboutDay state socializingState =
    { Kind = SocialActionKind.AskAboutDay
      Limit = Penalized(1<times>, Negative(5))
      RelationshipChange = Positive(2)
      MinimumLevel = 5<relationshipLevel> }
    |> performAction state socializingState

/// Tells a story to the NPC of the current conversation.
let tellStory state socializingState =
    { Kind = SocialActionKind.TellStory
      Limit = Penalized(3<times>, Negative(2))
      RelationshipChange = Positive(2)
      MinimumLevel = 10<relationshipLevel> }
    |> performAction state socializingState

/// Gives a compliment to the NPC of the current conversation.
let compliment state socializingState =
    { Kind = SocialActionKind.Compliment
      Limit = Penalized(2<times>, Negative(1))
      RelationshipChange = Positive(3)
      MinimumLevel = 5<relationshipLevel> }
    |> performAction state socializingState

/// Tells a joke to the NPC of the current conversation.
let tellJoke state socializingState =
    { Kind = SocialActionKind.TellJoke
      Limit = Penalized(2<times>, Negative(3))
      RelationshipChange = Positive(4)
      MinimumLevel = 10<relationshipLevel> }
    |> performAction state socializingState

/// Gossips with the NPC about other people.
let gossip state socializingState =
    { Kind = SocialActionKind.Gossip
      Limit = Penalized(1<times>, Negative(4))
      RelationshipChange = Positive(2)
      MinimumLevel = 20<relationshipLevel> }
    |> performAction state socializingState

/// Argues with the NPC about something.
let argue state socializingState =
    { Kind = SocialActionKind.Argue
      Limit = Penalized(1<times>, Negative(6))
      RelationshipChange = Negative(5)
      MinimumLevel = 15<relationshipLevel> }
    |> performAction state socializingState

/// Gives the NPC a friendly hug.
let hug state socializingState =
    { Kind = SocialActionKind.Hug
      Limit = Penalized(1<times>, Negative(2))
      RelationshipChange = Positive(5)
      MinimumLevel = 30<relationshipLevel> }
    |> performAction state socializingState

/// Flirts with the NPC.
let flirt state socializingState =
    { Kind = SocialActionKind.Flirt
      Limit = Penalized(2<times>, Negative(3))
      RelationshipChange = Positive(6)
      MinimumLevel = 25<relationshipLevel> }
    |> performAction state socializingState

/// Discusses common interests with the NPC.
let discussInterests state socializingState =
    { Kind = SocialActionKind.DiscussInterests
      Limit = NoAction(3<times>)
      RelationshipChange = Positive(3)
      MinimumLevel = 15<relationshipLevel> }
    |> performAction state socializingState

/// Asks the NPC about their career and work.
let askAboutCareer state socializingState =
    { Kind = SocialActionKind.AskAboutCareer
      Limit = Penalized(1<times>, Negative(2))
      RelationshipChange = Positive(2)
      MinimumLevel = 20<relationshipLevel> }
    |> performAction state socializingState

/// Shares a personal memory with the NPC.
let shareMemory state socializingState =
    { Kind = SocialActionKind.ShareMemory
      Limit = Penalized(2<times>, Negative(1))
      RelationshipChange = Positive(4)
      MinimumLevel = 40<relationshipLevel> }
    |> performAction state socializingState
