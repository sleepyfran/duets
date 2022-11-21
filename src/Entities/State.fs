module Entities.State

open Entities

/// Creates an empty state with all the fields set to its empty representation.
let empty = {
    Bands = Map.empty
    BandSongRepertoire = Band.SongRepertoire.empty
    BandAlbumRepertoire = Band.AlbumRepertoire.empty
    BankAccounts = Map.empty
    Characters = Map.empty
    CharacterSkills = Map.empty
    Concerts = Map.empty
    CurrentBandId = Identity.create () |> BandId
    CurrentPosition = (Prague, Identity.create () |> PlaceId)
    GenreMarkets = Map.empty
    CharacterInventory = List.empty
    PlayableCharacterId = Identity.create () |> CharacterId
    Situation = FreeRoam
    Today = Calendar.gameBeginning
}
