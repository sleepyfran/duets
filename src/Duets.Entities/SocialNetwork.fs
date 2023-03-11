module rec Duets.Entities.SocialNetwork

/// Creates the initial state of all social networks for the given character
/// and band.
let create (character: Character) (band: Band) =
    let characterAccountId = SocialNetworkAccountId.Character character.Id
    let bandAccountId = SocialNetworkAccountId.Band band.Id

    { Mastodon =
        { CurrentAccount = characterAccountId
          Accounts =
            [ (SocialNetworkAccountId.Character character.Id,
               Account.createEmpty characterAccountId character.Name)
              (SocialNetworkAccountId.Band band.Id,
               Account.createEmpty bandAccountId band.Name) ]
            |> Map.ofList } }

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
