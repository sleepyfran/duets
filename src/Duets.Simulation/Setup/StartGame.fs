module Duets.Simulation.Setup

open Duets
open Duets.Common
open Duets.Data
open Duets.Data.Items
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Market

let private allInitialWorldItems =
    Queries.World.allCities
    |> List.map (fun city ->
        let homeId =
            Queries.World.placeIdsByTypeInCity city.Id PlaceTypeIndex.Home
            |> List.head

        let kitchenItems =
            [ fst Drink.SoftDrinks.cocaColaBottle
              fst Drink.Coffee.doubleEspresso
              fst Furniture.Stove.lgStove ]

        let bedroomItems = [ fst Furniture.Bed.ikeaBed ]

        let livingRoomItems =
            [ fst Electronics.Tv.lgTv; fst Electronics.GameConsole.xbox ]

        [ (city.Id, homeId, World.Ids.Home.bedroom), bedroomItems
          (city.Id, homeId, World.Ids.Home.kitchen), kitchenItems
          (city.Id, homeId, World.Ids.Home.livingRoom), livingRoomItems ])
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
    let initialPlaceId =
        Queries.World.placeIdsByTypeInCity initialCity.Id PlaceTypeIndex.Home
        |> List.head (* We need at least one home in the city. *)

    let initialPlace =
        (initialCity.Id, initialPlaceId) ||> Queries.World.placeInCityById

    let initialRental =
        Rentals.RentPlace.createRental
            Calendar.gameBeginning
            initialCity.Id
            initialPlace

    let initialSkillMap =
        initialSkills
        |> List.fold
            (fun acc (skill, level) -> Map.add skill.Id (skill, level) acc)
            Map.empty

    let initialGenreMarket = GenreMarket.create Data.Genres.all

    { Bands =
        { Current = band.Id
          Character = [ (band.Id, band) ] |> Map.ofList
          Simulated = Map.empty }
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
      CurrentPosition =
        initialCity.Id, initialPlaceId, World.Ids.Home.livingRoom
      Flights = []
      GenreMarkets = initialGenreMarket
      CharacterInventory = List.empty
      PlayableCharacterId = character.Id
      Rentals =
        [ (initialCity.Id, initialPlaceId), initialRental ] |> Map.ofList
      Situation = FreeRoam
      SocialNetworks = SocialNetwork.empty
      Today = Calendar.gameBeginning
      WorldItems = allInitialWorldItems }
    |> Bands.Generation.addInitialBandsToState
    |> GameCreated
