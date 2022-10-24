module Simulation.Setup

open Common
open Data.World
open Entities
open Simulation
open Simulation.Market

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard and starts the generation process for the game simulation
/// which includes markets for the different genres available and the game world.
let startGame (character: Character) (band: Band) =
    let world = World.get ()
    let initialCity = Map.head world.Cities

    let initialPlace =
        Queries.World.placeIdsOf initialCity.Id PlaceTypeIndex.RehearsalSpace
        |> List.head (* We need at least one rehearsal space in the city. *)

    {
        Bands = [ (band.Id, band) ] |> Map.ofList
        BandAlbumRepertoire = Band.AlbumRepertoire.emptyFor band.Id
        BandSongRepertoire = Band.SongRepertoire.emptyFor band.Id
        BankAccounts =
            [
                (Character character.Id,
                 BankAccount.forCharacterWithBalance character.Id 10000<dd>)
                (Band band.Id, BankAccount.forBand band.Id)
            ]
            |> Map.ofSeq
        Characters = [ (character.Id, character) ] |> Map.ofList
        CharacterSkills = [ (character.Id, Map.empty) ] |> Map.ofList
        Concerts =
            [ (band.Id, Concert.Timeline.empty) ]
            |> Map.ofList
        CurrentBandId = band.Id
        CurrentPosition = initialCity.Id, initialPlace
        GenreMarkets = GenreMarket.create Data.Genres.all
        CharacterInventory = List.empty
        PlayableCharacterId = character.Id
        Situation = FreeRoam
        Today = Calendar.gameBeginning
    }
    |> GameCreated
