namespace Duets.Entities

[<AutoOpen>]
module SocialNetworkTypes =
    [<RequireQualifiedAccess>]
    type SocialNetworkKey = Mastodon

    [<RequireQualifiedAccess>]
    type SocialNetworkAccountId =
        | Character of CharacterId
        | Band of BandId

    [<RequireQualifiedAccess>]
    type SocialNetworkCurrentAccountStatus =
        | NoAccountCreated
        | Account of SocialNetworkAccountId

    /// Defines a post that an account made in a social network of the game.
    type SocialNetworkPost =
        { AccountId: SocialNetworkAccountId
          Timestamp: Date
          Text: string
          Reposts: int }

    /// Defines an account for either a character or a band inside of a social
    /// network in the game.
    type SocialNetworkAccount =
        { Id: SocialNetworkAccountId
          Handle: string
          Followers: int
          Posts: SocialNetworkPost list }

    /// Represents a social network inside of the game, with all the accounts
    /// inside of it (which contain the posts), and the currently selected account.
    type SocialNetwork =
        { CurrentAccount: SocialNetworkCurrentAccountStatus
          Accounts: Map<SocialNetworkAccountId, SocialNetworkAccount> }

    /// Defines all social networks in the game, with their current state.
    type SocialNetworks = { Mastodon: SocialNetwork }
