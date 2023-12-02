namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

module Social =
    let private startConversationInteractions state =
        let knownPeople, unknownPeople =
            Queries.World.peopleInCurrentPlace state

        let noPeopleInRoom =
            List.isEmpty knownPeople && List.isEmpty unknownPeople

        if noPeopleInRoom then
            []
        else
            SocialInteraction.StartConversation(knownPeople, unknownPeople)
            |> Interaction.Social
            |> List.singleton

    /// Returns all social interactions available in the current context.
    let internal interactions state =
        let situation = Queries.Situations.current state

        match situation with
        | Socializing _ -> [] (* TODO: Return socializing interactions. *)
        (* Might be able to start a new social situation if there are NPCs around. *)
        | Airport _
        | FreeRoam -> startConversationInteractions state
        (* Not available. *)
        | Concert _
        | PlayingMiniGame _ -> []
