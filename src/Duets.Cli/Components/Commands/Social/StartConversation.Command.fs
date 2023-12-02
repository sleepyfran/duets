namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module StartConversationCommand =
    /// Command which starts a new conversation with an NPC.
    let create (knownNpcs: Character list) (unknownNpcs: Character list) =
        { Name = "start conversation"
          Description = "Starts a conversation with another character"
          Handler =
            fun args ->
                let npc =
                    showOptionalChoicePrompt
                        "Who do you want to talk to?"
                        Generic.nothing
                        (fun npc ->
                            if List.contains npc knownNpcs then
                                npc.Name
                            else
                                $"Unknown ({npc.Gender})"
                            |> Styles.person)
                        (knownNpcs @ unknownNpcs)

                match npc with
                | Some npc -> Situations.socializing npc |> Effect.apply
                | None -> ()

                Scene.World }
