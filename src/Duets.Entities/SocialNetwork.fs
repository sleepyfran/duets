module rec Duets.Entities.SocialNetwork

/// Creates the initial state of all social networks for the given character
/// and band.
let empty =
    { Mastodon =
        { CurrentAccount = SocialNetworkCurrentAccountStatus.NoAccountCreated
          Accounts = Map.empty } }

module Account =
    /// Creates an empty social network account for the given ID.
    let createEmpty id handle =
        { Id = id
          Handle = handle
          Followers = 0
          Posts = List.empty }

module Post =
    /// Creates a post the given account, date and text, with no reposts.
    let create accountId date text =
        { AccountId = accountId
          Timestamp = date
          Text = text
          Reposts = 0 }
