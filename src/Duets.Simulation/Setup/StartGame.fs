module Duets.Simulation.Setup

open Duets
open Duets.Common
open Duets.Data.Items
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Market

let private allInitialWorldItems =
    Queries.World.allCities
    |> List.map (fun city ->
        let homeId =
            Queries.World.placeIdsOf city.Id PlaceTypeIndex.Home |> List.head

        let kitchenItems =
            [ fst Food.FastFood.genericBurger
              fst Drink.SoftDrinks.cocaColaBottle
              fst Drink.Coffee.doubleEspresso ]

        let bedroomItems = [ fst Furniture.Bed.ikeaBed ]

        let livingRoomItems =
            [ fst Electronics.Tv.lgTv; fst Electronics.GameConsole.xbox ]

        [ (city.Id, homeId),
          (List.concat [ kitchenItems; bedroomItems; livingRoomItems ]) ])
    |> List.concat
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
      WorldItems = allInitialWorldItems }
    |> GameCreated
