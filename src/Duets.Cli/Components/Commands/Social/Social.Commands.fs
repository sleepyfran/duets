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

[<RequireQualifiedAccess>]
module AskAboutDayCommand =
    let private thingsDoneInDay =
        [ "went to work"
          "went to school"
          "went to the gym"
          "went to the park"
          "went to the beach"
          "went to the movies"
          "went to the mall"
          "went to the library"
          "went to the museum"
          "went to a party"
          "had a date"
          "had a meeting"
          "had a job interview"
          "had a doctor's appointment"
          "had a dentist's appointment"
          "had a haircut" ]

    /// Command which asks the NPC about their day.
    let create socializingState =
        SocialCommand.create
            {| Name = "ask about day"
               Description = "Asks the other person about their day"
               Action =
                fun _ ->
                    Social.Actions.askAboutDay (State.get ()) socializingState
               Handler =
                fun result ->
                    let gender = socializingState.Npc.Gender

                    match result with
                    | Done ->
                        let thingDone = thingsDoneInDay |> List.sample

                        $"{socializingState.Npc.Name} tells you that {Generic.subjectPronounForGender gender |> String.lowercase} {thingDone} today"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"You've already asked about {Generic.possessiveAdjectiveForGender gender}... why... why are you doing it again?"
                        |> Styles.error
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

module SocialActionCommand =
    /// Creates a command for the given social action.
    let forAction socializingState (action: SocialActionKind) =
        match action with
        | SocialActionKind.Greet -> GreetCommand.create socializingState
        | SocialActionKind.Chat -> ChatCommand.create socializingState
        | SocialActionKind.AskAboutDay ->
            AskAboutDayCommand.create socializingState
