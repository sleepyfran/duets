module Duets.Entities.State

open Duets.Entities.SituationTypes

/// Creates an empty state with all the fields set to its empty representation.
let empty =
    { Bands = Map.empty
      BandSongRepertoire = Band.SongRepertoire.empty
      BandAlbumRepertoire = Band.AlbumRepertoire.empty
      BankAccounts = Map.empty
      Career = None
      Characters = Map.empty
      CharacterSkills = Map.empty
      Concerts = Map.empty
      CurrentBandId = Identity.create () |> BandId
      CurrentPosition = (Prague, Identity.create () |> PlaceId)
      Flights = []
      GenreMarkets = Map.empty
      CharacterInventory = List.empty
      PlayableCharacterId = Identity.create () |> CharacterId
      Rentals = Map.empty
      Situation = FreeRoam
      Today = Calendar.gameBeginning
      WorldItems = Map.empty }
