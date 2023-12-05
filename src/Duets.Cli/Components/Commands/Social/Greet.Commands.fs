namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Social.Common

[<RequireQualifiedAccess>]
module GreetCommand =
    /// Command which greets the NPC of the current conversation.
    let create socializingState =
        SocialCommand.create
            {| Name = "greet"
               Description = "Greets the other person"
               Action =
                fun _ -> Social.Actions.greet (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        $"{socializingState.Npc.Name} says hello back!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        "You've already greeted this person... why... why are you doing it again?"
                        |> Styles.error
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}
