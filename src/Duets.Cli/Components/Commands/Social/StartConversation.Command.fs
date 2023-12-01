namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module StartConversationCommand =
    /// Command which starts a new conversation with an NPC.
    let create (npcs: Character list) =
        { Name = "start conversation"
          Description = "Starts a conversation with another character"
          Handler =
            fun args ->
                let input = args |> String.concat " "
                
                Scene.World }
