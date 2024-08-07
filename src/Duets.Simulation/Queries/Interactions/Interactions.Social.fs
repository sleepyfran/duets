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

    let private getSocializingInteractions socializingState =
        Union.allCasesOf<SocialActionKind> ()
        |> List.map (fun action ->
            SocialInteraction.Action(socializingState, action)
            |> Interaction.Social)

    /// Returns all social interactions available in the current context.
    let internal interactions state =
        let situation = Queries.Situations.current state

        match situation with
        | Socializing socializingState ->
            [ yield! getSocializingInteractions socializingState
              SocialInteraction.StopConversation |> Interaction.Social ]
        (* Might be able to start a new social situation if there are NPCs around. *)
        | Airport _
        | FreeRoam -> startConversationInteractions state
        (* Not available. *)
        | Concert _
        | Focused _
        | PlayingMiniGame _ -> []
