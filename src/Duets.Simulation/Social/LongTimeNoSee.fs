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
            Calendar.Query.daysBetween
                relationship.LastIterationDate
                currentDate

        daysSinceLastInteraction > 14<days>)
    |> List.map (fun relationship ->
        let npc = Queries.Characters.find state relationship.Character

        let updatedLevel =
            relationship.Familiarity - 5<familiarity>
            |> Math.clamp 0<familiarity> 100<familiarity>

        let updatedRelationship =
            match relationship.Familiarity, updatedLevel with
            | 0<familiarity>, 0<familiarity> -> None
            | _ ->
                Some
                    { relationship with
                        Familiarity = updatedLevel
                        (*
                        Artificially change the last interaction time so that we don't apply
                        this again until two weeks later.
                        *)
                        LastIterationDate = currentDate }

        (npc, relationship.MeetingCity, updatedRelationship)
        |> RelationshipChanged)
