module Duets.Simulation.Social.LongTimeNoSee

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Goes through the relationships of the character and reduces the level
/// of all the ones that haven't interacted in the past two weeks.
let applyIfNeeded state =
    let currentDate = Queries.Calendar.today state

    Queries.Relationship.all state
    |> List.ofMapValues
    |> List.filter (fun relationship ->
        let daysSinceLastInteraction =
            relationship.LastIterationDate - currentDate |> _.Days |> abs

        daysSinceLastInteraction > 14)
    |> List.map (fun relationship ->
        let npc = Queries.Characters.find state relationship.Character

        let updatedLevel =
            relationship.Level - 5<relationshipLevel>
            |> Math.clamp 0<relationshipLevel> 100<relationshipLevel>

        let updatedRelationship =
            match relationship.Level, updatedLevel with
            | 0<relationshipLevel>, 0<relationshipLevel> -> None
            | _ ->
                Some
                    { relationship with
                        Level = updatedLevel
                        (*
                        Artificially change the last interaction time so that we don't apply
                        this again until two weeks later.
                        *)
                        LastIterationDate = currentDate }

        (npc, relationship.MeetingCity, updatedRelationship)
        |> RelationshipChanged)
