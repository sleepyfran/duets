namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
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
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to greet them properly yet."
                        |> Styles.warning
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

                        $"You and {socializingState.Npc.Name} have a nice talk about {randomTopic}."
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsNoAction ->
                        let gender = socializingState.Npc.Gender

                        $"You've run out of topics with this person, {Generic.subjectPronounForGender gender |> String.lowercase} {Generic.verbConjugationForGender Generic.Be gender |> String.lowercase} not so interested in chatting anymore."
                        |> Styles.warning
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to chat with them about personal topics yet."
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

                        $"{socializingState.Npc.Name} tells you that {Generic.subjectPronounForGender gender |> String.lowercase} {thingDone} today."
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"You've already asked about {Generic.possessiveAdjectiveForGender gender}... why... why are you doing it again?"
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to ask about their personal day yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module TellStoryCommand =
    let private storyTopic =
        [ "a funny thing that happened to you"
          "a funny thing that happened to a friend"
          "a strange thing that happened to you"
          "an adventure you had"
          "a memorable trip"
          "a childhood memory"
          "a dream you had yesterday"
          "a book you read recently"
          "a movie you watched the other day"
          "a concert you attended the other day"
          "a hobby of yours"
          "a skill you learned"
          "your favorite food"
          "your favorite place"
          "your favorite animal"
          "your favorite sport"
          "your favorite game"
          "your favorite song"
          "your favorite artist"
          "your favorite actor"
          "your favorite author"
          "your favorite holiday"
          "your favorite season" ]

    /// Command which asks the NPC about their day.
    let create socializingState =
        SocialCommand.create
            {| Name = "tell story"
               Description = "Tells a story to the other person"
               Action =
                fun _ ->
                    Social.Actions.tellStory (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let topic = storyTopic |> List.sample

                        $"You tell {socializingState.Npc.Name} a story about {topic}."
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"You've told too many stories and {socializingState.Npc.Name} is too bored to listen to you anymore."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to share stories with them yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module ComplimentCommand =
    let private complimentTypes =
        [ "your style"
          "your hair"
          "your smile"
          "your sense of humor"
          "your intelligence"
          "your kindness"
          "your talent"
          "your personality"
          "your achievements"
          "your positivity" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "compliment"
               Description = "Give a compliment to the other person"
               Action =
                fun _ -> Social.Actions.compliment (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let compliment = complimentTypes |> List.sample
                        $"You compliment {socializingState.Npc.Name} on {compliment}. They seem pleased!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"Your compliments are starting to sound insincere to {socializingState.Npc.Name}."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to give them compliments yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module TellJokeCommand =
    let private jokeTypes =
        [ "a pun"
          "a knock-knock joke"
          "a funny story"
          "a witty observation"
          "a silly riddle"
          "a clever wordplay"
          "an amusing anecdote" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "tell joke"
               Description = "Tell a joke to make the other person laugh"
               Action =
                fun _ -> Social.Actions.tellJoke (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let joke = jokeTypes |> List.sample
                        $"You tell {socializingState.Npc.Name} {joke}. They burst out laughing!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"Your jokes aren't landing anymore. {socializingState.Npc.Name} gives you a polite smile."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to tell them jokes yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module GossipCommand =
    let private gossipTopics =
        [ "recent celebrity drama"
          "rumors about a mutual acquaintance"
          "interesting neighborhood news"
          "workplace drama"
          "social media trends"
          "local happenings" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "gossip"
               Description = "Share some gossip with the other person"
               Action =
                fun _ -> Social.Actions.gossip (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let topic = gossipTopics |> List.sample
                        $"You and {socializingState.Npc.Name} whisper about {topic}. They lean in with interest!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"{socializingState.Npc.Name} seems uncomfortable with all the gossip and changes the subject."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't trust {socializingState.Npc.Name} enough to share gossip with them yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module ArgueCommand =
    let private argumentTopics =
        [ "politics"
          "sports teams"
          "movie preferences"
          "music tastes"
          "social issues"
          "lifestyle choices" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "argue"
               Description = "Start a heated debate with the other person"
               Action =
                fun _ -> Social.Actions.argue (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let topic = argumentTopics |> List.sample
                        $"You get into a heated discussion with {socializingState.Npc.Name} about {topic}. Tensions rise!"
                        |> Styles.error
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"{socializingState.Npc.Name} is getting really annoyed with your argumentative attitude!"
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to start arguments with them."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module HugCommand =
    let create socializingState =
        SocialCommand.create
            {| Name = "hug"
               Description = "Give the other person a warm hug"
               Action =
                fun _ -> Social.Actions.hug (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        $"You give {socializingState.Npc.Name} a warm, friendly hug. They hug you back!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"{socializingState.Npc.Name} seems a bit uncomfortable with all the physical affection."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You're not close enough to {socializingState.Npc.Name} to give them a hug yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module FlirtCommand =
    let private flirtActions =
        [ "wink playfully"
          "give a charming smile"
          "make suggestive eye contact"
          "compliment their appearance"
          "use some smooth pickup lines"
          "playfully tease them" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "flirt"
               Description = "Flirt romantically with the other person"
               Action =
                fun _ -> Social.Actions.flirt (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let action = flirtActions |> List.sample
                        $"You {action} at {socializingState.Npc.Name}. They blush and seem interested!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"Your flirting is becoming too much for {socializingState.Npc.Name}. They seem overwhelmed."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to flirt with them yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module DiscussInterestsCommand =
    let private interests =
        [ "music and bands"
          "movies and TV shows"
          "books and literature"
          "sports and fitness"
          "travel and adventure"
          "cooking and food"
          "art and creativity"
          "technology and gadgets"
          "nature and outdoors"
          "gaming and entertainment" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "discuss interests"
               Description = "Talk about common hobbies and interests"
               Action =
                fun _ -> Social.Actions.discussInterests (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let interest = interests |> List.sample
                        $"You and {socializingState.Npc.Name} have an engaging conversation about {interest}. You find a lot in common!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsNoAction ->
                        $"You've exhausted all your common interests with {socializingState.Npc.Name}."
                        |> Styles.warning
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to discuss deeper interests yet."
                        |> Styles.warning
                        |> showMessage
                    | _ -> () |}

[<RequireQualifiedAccess>]
module AskAboutCareerCommand =
    let private careerAspects =
        [ "their current job"
          "their career goals"
          "their work experiences"
          "their professional challenges"
          "their industry insights"
          "their workplace stories" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "ask about career"
               Description = "Inquire about their work and professional life"
               Action =
                fun _ -> Social.Actions.askAboutCareer (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let aspect = careerAspects |> List.sample
                        $"You ask {socializingState.Npc.Name} about {aspect}. They share some interesting insights!"
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"{socializingState.Npc.Name} seems tired of talking about work all the time."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You don't know {socializingState.Npc.Name} well enough to ask about their career yet."
                        |> Styles.warning
                        |> showMessage
                    | TooManyRepetitionsNoAction -> () |}

[<RequireQualifiedAccess>]
module ShareMemoryCommand =
    let private memoryTypes =
        [ "a cherished childhood memory"
          "a meaningful family moment"
          "an unforgettable adventure"
          "a life-changing experience"
          "a proud achievement"
          "a touching friendship story"
          "a romantic memory"
          "a lesson learned the hard way" ]

    let create socializingState =
        SocialCommand.create
            {| Name = "share memory"
               Description = "Share a personal memory with the other person"
               Action =
                fun _ -> Social.Actions.shareMemory (State.get ()) socializingState
               Handler =
                fun result ->
                    match result with
                    | Done ->
                        let memory = memoryTypes |> List.sample
                        $"You open up to {socializingState.Npc.Name} and share {memory}. They listen intently and seem moved."
                        |> Styles.success
                        |> showMessage
                    | TooManyRepetitionsPenalized ->
                        $"You've shared so many personal stories that {socializingState.Npc.Name} seems overwhelmed by the intimacy."
                        |> Styles.error
                        |> showMessage
                    | RelationshipLevelTooLow ->
                        $"You're not close enough to {socializingState.Npc.Name} to share personal memories yet."
                        |> Styles.warning
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
        | SocialActionKind.TellStory -> TellStoryCommand.create socializingState
        | SocialActionKind.Compliment -> ComplimentCommand.create socializingState
        | SocialActionKind.TellJoke -> TellJokeCommand.create socializingState
        | SocialActionKind.Gossip -> GossipCommand.create socializingState
        | SocialActionKind.Argue -> ArgueCommand.create socializingState
        | SocialActionKind.Hug -> HugCommand.create socializingState
        | SocialActionKind.Flirt -> FlirtCommand.create socializingState
        | SocialActionKind.DiscussInterests -> DiscussInterestsCommand.create socializingState
        | SocialActionKind.AskAboutCareer -> AskAboutCareerCommand.create socializingState
        | SocialActionKind.ShareMemory -> ShareMemoryCommand.create socializingState
