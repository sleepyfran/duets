module Entities.State

open Entities

/// Creates an empty state with all the fields set to its empty representation.
let empty =
    { Bands = Map.empty
      Character = Character.empty
      CharacterSkills = Map.empty
      CurrentBandId = Identity.create () |> BandId
      BandSongRepertoire = Band.SongRepertoire.empty
      BandAlbumRepertoire = Band.AlbumRepertoire.empty
      BankAccounts = Map.empty
      Today = Calendar.gameBeginning
      GenreMarkets = Map.empty }
