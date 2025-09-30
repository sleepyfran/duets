namespace Duets.Entities

[<AutoOpen>]
module SocialTypes =
    /// Defines all the possible social actions that can be performed during
    /// a social interaction.
    [<RequireQualifiedAccess>]
    type SocialActionKind =
        | Greet
        | Chat
        | AskAboutDay
        | TellStory
        | Compliment
        | TellJoke
        | Gossip
        | Argue
        | Hug
        | Flirt
        | DiscussInterests
        | AskAboutCareer
        | ShareMemory

    /// Defines a state for a current social interaction.
    type SocializingState =
        { Npc: Character
          Relationship: Relationship option
          Actions: SocialActionKind list }
