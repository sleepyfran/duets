namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module StartConversationCommand =
    /// Command which starts a new conversation with an NPC.
    let create (knownNpcs: PresentNpc list) (unknownNpcs: PresentNpc list) =
        { Name = "start conversation"
          Description = "Starts a conversation with another character"
          Handler =
            fun _ ->
                let npc =
                    showOptionalChoicePrompt
                        "Who do you want to talk to?"
                        Generic.nothing
                        (fun npc ->
                            let character = npc.Npc
                            let goal = npc.Goal

                            if List.contains npc knownNpcs then
                                character.Name
                            else
                                $"Unknown ({character.Gender}) (DEBUG: {goal})"
                            |> Styles.person)
                        (knownNpcs @ unknownNpcs)

                match npc with
                | Some { Npc = npc } ->
                    Social.Actions.startConversation (State.get ()) npc
                    |> Effect.apply
                | None -> ()

                Scene.World }

[<RequireQualifiedAccess>]
module StopConversationCommand =
    /// Command which stops the current conversation and puts the character
    /// back into FreeRoam mode.
    let get =
        { Name = "stop conversation"
          Description = "Stops the current conversation"
          Handler =
            fun _ ->
                Social.Actions.stopConversation (State.get ())
                |> Effect.applyMultiple

                Scene.World }
