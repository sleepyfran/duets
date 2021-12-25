module Simulation.Setup

open Common
open Entities
open Simulation.Market

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard and starts the generation process for the game simulation
/// which includes markets for the different genres available and the game world.
let startGame character (band: Band) =
    let world = World.Generation.generate ()

    let initialCity = Map.head world.Cities

    { Character = character
      CharacterSkills = [ (character.Id, Map.empty) ] |> Map.ofList
      CurrentBandId = band.Id
      CurrentPosition = (initialCity.Id, initialCity.Graph.StartingNode, None)
      Bands = [ (band.Id, band) ] |> Map.ofList
      BandSongRepertoire = Band.SongRepertoire.emptyFor band.Id
      BandAlbumRepertoire = Band.AlbumRepertoire.emptyFor band.Id
      BankAccounts =
          [ (Character character.Id,
             BankAccount.forCharacterWithBalance character.Id 10000<dd>)
            (Band band.Id, BankAccount.forBand band.Id) ]
          |> Map.ofSeq
      Today = Calendar.gameBeginning
      GenreMarkets = GenreMarket.create (Database.genres ())
      World = world }
    |> GameCreated
