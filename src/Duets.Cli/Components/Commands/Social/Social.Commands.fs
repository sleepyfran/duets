namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
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

[<RequireQualifiedAccess>]
module ChatCommand =
    let private topics =
        [ "the weather"
          "sports"
          "politics"
          "music"
          "movies"
          "books"
          "food"
          "travelling"
          "work"
          "family"
          "pets"
          "hobbies"
          "games"
          "fashion"
          "technology"
          "cars"
          "health"
          "fitness"
          "art"
          "science"
          "religion" ]

    /// Command which chats with the NPC of the current conversation.
    let create socializingState =
        SocialCommand.create
            {| Name = "chat"
               Description = "Chats about a random topic with the other person"
               Action =
                fun _ -> Social.Actions.chat (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let randomTopic = topics |> List.sample

                        $"You and {socializingState.Npc.Name} have a nice talk about {randomTopic}"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsNoAction ->
                        let gender = socializingState.Npc.Gender

                        $"You've run out of topics with this person, {Generic.subjectPronounForGender gender |> String.lowercase} {Generic.verbConjugationForGender Generic.Be gender |> String.lowercase} not so interested in chatting anymore"
                        |> Styles.warning
                        |> showMessage
                    | _ -> () |}
