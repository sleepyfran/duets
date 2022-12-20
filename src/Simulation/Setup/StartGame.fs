module Simulation.Setup

open Common
open Data.Items
open Entities
open Entities.SituationTypes
open Simulation
open Simulation.Market

let private initialWorldItems (initialCity: City) =
    let homeId =
        Queries.World.placeIdsOf initialCity.Id PlaceTypeIndex.Home |> List.head

    let kitchenItems =
        [ fst Food.FastFood.genericBurger; fst Drink.Beer.kozelPint ]

    let bedroomItems = [ fst Furniture.Bed.ikeaBed ]

    let livingRoomItems =
        [ fst Electronics.Tv.lgTv; fst Electronics.GameConsole.xbox ]

    [ (initialCity.Id, homeId),
      (List.concat [ kitchenItems; bedroomItems; livingRoomItems ]) ]
    |> Map.ofList

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard and starts the generation process for the game simulation
/// which includes markets for the different genres available and the game world.
let startGame
    (character: Character)
    (band: Band)
    (initialSkills: SkillWithLevel list)
    (initialCity: City)
    =
    let initialPlace =
        Queries.World.placeIdsOf initialCity.Id PlaceTypeIndex.Home
        |> List.head (* We need at least one home in the city. *)

    let initialSkillMap =
        initialSkills
        |> List.fold
            (fun acc (skill, level) -> Map.add skill.Id (skill, level) acc)
            Map.empty

    { Bands = [ (band.Id, band) ] |> Map.ofList
      BandAlbumRepertoire = Band.AlbumRepertoire.emptyFor band.Id
      BandSongRepertoire = Band.SongRepertoire.emptyFor band.Id
      BankAccounts =
        [ (Character character.Id,
           BankAccount.forCharacterWithBalance character.Id 1000m<dd>)
          (Band band.Id, BankAccount.forBand band.Id) ]
        |> Map.ofSeq
      Career = None
      Characters = [ (character.Id, character) ] |> Map.ofList
      CharacterSkills = [ (character.Id, initialSkillMap) ] |> Map.ofList
      Concerts = [ (band.Id, Concert.Timeline.empty) ] |> Map.ofList
      CurrentBandId = band.Id
      CurrentPosition = initialCity.Id, initialPlace
      Flights = []
      GenreMarkets = GenreMarket.create Data.Genres.all
      CharacterInventory = List.empty
      PlayableCharacterId = character.Id
      Situation = FreeRoam
      Today = Calendar.gameBeginning
      WorldItems = initialWorldItems initialCity }
    |> GameCreated
