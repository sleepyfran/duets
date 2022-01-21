module Entities.State

open Entities

/// Creates an empty state with all the fields set to its empty representation.
let empty =
    { Bands = Map.empty
      BandSongRepertoire = Band.SongRepertoire.empty
      BandAlbumRepertoire = Band.AlbumRepertoire.empty
      BankAccounts = Map.empty
      Character = Character.empty
      CharacterSkills = Map.empty
      Concerts = ConcertContext.empty
      CurrentBandId = Identity.create () |> BandId
      CurrentPosition = (Identity.create (), Identity.create ())
      Schedule = Schedule.empty
      Today = Calendar.gameBeginning
      GenreMarkets = Map.empty
      World = World.empty }
