module Duets.Simulation.Social.Common

open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

/// Defines the result of performing a social action.
type SocialActionResult =
    /// Indicates that the action was performed successfully.
    | Done
    /// The action was performed and penalized because it was done too many
    /// times.
    | TooManyRepetitionsPenalized
    /// The action was not performed because it was done too many times.
    | TooManyRepetitionsNoAction

type SocialActionResponse =
    { Effects: Effect list
      SocializingState: SocializingState
      Result: SocialActionResult }

module internal Response =
    /// Returns an empty result with no effects and the given result.
    let withoutEffects result state =
        { Effects = []
          SocializingState = state
          Result = result }

    /// Adds the given effects to the response.
    let addEffects response effects =
        { response with
            Effects = response.Effects @ effects }

    /// Adds one effect to the response.
    let addEffect response effect = addEffects response [ effect ]

    /// Updates the result of the response.
    let changeResult result response = { response with Result = result }

/// Defines how the relationship between the player and the NPC changes when
/// performing a social action.
type internal RelationshipChange =
    | Positive of points: int
    | Negative of points: int
    | NoChange

/// Defines a limit in the amount of times that an action can be performed during
/// a conversation with an NPC. If NoLimit then the action can be performed
/// unlimited times, when the limit is penalized then a penalization is applied
/// when performing the action more than limit times and no action disallows the
/// execution of the action if it'd be over the limit.
type internal Limit =
    | NoLimit
    | Penalized of limit: int<times> * penalization: RelationshipChange
    | NoAction of limit: int<times>

/// Defines a social action that can be performed, with the kind of action that
/// will be performed and the limit of times that it can be performed. This
/// action can be passed into the `performAction` function and it takes care of
/// performing all the outcomes of that action.
type internal SocialAction =
    { Kind: SocialActionKind
      Limit: Limit
      RelationshipChange: RelationshipChange }

/// Performs the given socializing action and returns a response that contains
/// the updated socializing state and the effects that were produced by
/// performing the action.
let rec internal performAction state socializingState action =
    let timesPerformedAction =
        Social.State.timesDoneAction socializingState action.Kind

    match action.Limit with
    | NoLimit -> performAction' state socializingState action
    | Penalized(limit, penalization) ->
        if timesPerformedAction < limit then
            performAction' state socializingState action
        else
            applyPenalization state socializingState penalization action
    | NoAction limit ->
        if timesPerformedAction < limit then
            performAction' state socializingState action
        else
            Response.withoutEffects TooManyRepetitionsNoAction socializingState

and private applyPenalization state socializingState penalization action =
    { action with
        RelationshipChange = penalization }
    |> performAction' state socializingState
    |> Response.changeResult TooManyRepetitionsPenalized

and private performAction' state socializingState action =
    match action.RelationshipChange with
    | Positive points -> points
    | Negative points -> -points
    | NoChange -> 0
    |> responseFromPoints state socializingState
    |> addAction action.Kind
    |> addSituationEffect

and private responseFromPoints state socializingState points =
    let cityId, _, _ = Queries.World.currentCoordinates state
    let currentDate = Queries.Calendar.today state
    let points = points * 1<relationshipLevel>

    let updatedRelationship =
        socializingState.Relationship
        |> Option.map (fun relationship ->
            { relationship with
                LastIterationDate = currentDate
                Level = clampedSum relationship.Level points })
        |> Option.defaultValue
            { Character = socializingState.Npc.Id
              MeetingCity = cityId
              LastIterationDate = currentDate
              Level = clampedSum 0<relationshipLevel> points
              RelationshipType = Friend }

    let state =
        { socializingState with
            Relationship = Some updatedRelationship }

    state |> Response.withoutEffects Done

and private clampedSum currentLevel points =
    currentLevel + points
    |> Math.clamp 0<relationshipLevel> 100<relationshipLevel>

and private addAction actionKind response =
    { response with
        SocialActionResponse.SocializingState.Actions =
            actionKind :: response.SocializingState.Actions }

and private addSituationEffect response =
    Situation.Socializing response.SocializingState
    |> SituationChanged
    |> Response.addEffect response
