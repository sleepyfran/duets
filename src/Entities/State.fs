module Entities.State

open Entities

/// Creates an empty state with all the fields set to its empty representation.
let empty =
    { Bands = Map.empty
      Character = Character.empty
      CharacterSkills = Map.empty
      CurrentBandId = Identity.create () |> BandId
      BandRepertoire = Band.Repertoire.empty
      BankAccounts = Map.empty
      Today = Calendar.gameBeginning
      GenreMarkets = Map.empty }
