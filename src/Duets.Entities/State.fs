module Duets.Entities.State

open Duets.Entities.SituationTypes

/// Creates an empty state with all the fields set to its empty representation.
let empty =
    { Bands =
        { Current = Identity.create () |> BandId
          Character = Map.empty
          Simulated = Map.empty }
      BandSongRepertoire = Band.SongRepertoire.empty
      BandAlbumRepertoire = Band.AlbumRepertoire.empty
      BankAccounts = Map.empty
      Career = None
      Characters = Map.empty
      CharacterSkills = Map.empty
      Concerts = Map.empty
      CurrentPosition = (Prague, Identity.create () |> PlaceId, 0)
      Flights = []
      GenreMarkets = Map.empty
      CharacterInventory = List.empty
      PlayableCharacterId = Identity.create () |> CharacterId
      Rentals = Map.empty
      Situation = FreeRoam
      SocialNetworks =
        { Mastodon =
            { CurrentAccount =
                SocialNetworkCurrentAccountStatus.NoAccountCreated
              Accounts = Map.empty } }
      Today = Calendar.gameBeginning
      WorldItems = Map.empty }
