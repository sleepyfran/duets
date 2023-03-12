module rec Duets.Entities.SocialNetwork

open Duets.Common

/// Creates the initial state of all social networks for the given character
/// and band.
let empty =
    { Mastodon =
        { CurrentAccount = SocialNetworkCurrentAccountStatus.NoAccountCreated
          Accounts = Map.empty } }

module Account =
    let private removeTrailingAts handle =
        if handle |> String.startsWith "@" then
            handle.Substring 1
        else
            handle

    /// Creates an empty social network account for the given ID.
    let createEmpty id handle =
        { Id = id
          Handle = handle |> removeTrailingAts
          Followers = 0
          Posts = List.empty }

    /// Creates a social network account for the given ID with the handle and
    /// followers specified.
    let create id handle followers =
        { Id = id
          Handle = handle |> removeTrailingAts
          Followers = followers
          Posts = List.empty }

module Post =
    /// Creates a post the given account, date and text, with no reposts.
    let create accountId date text =
        { AccountId = accountId
          Timestamp = date
          Text = text
          Reposts = 0 }
