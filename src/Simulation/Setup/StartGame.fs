module Simulation.Setup

open Common
open Data.Items
open Data.World
open Entities
open Simulation
open Simulation.Market

let private addItemsIn city spaceType roomType (items: Item list) =
    Queries.World.Common.coordinatesOfRoomsIn city spaceType roomType
    |> List.map (fun coords -> ((city.Id, Room coords), items))

let private initialWorldItems initialCity =
    let kitchenItems =
        addItemsIn
            initialCity
            SpaceTypeIndex.Home
            RoomTypeIndex.Kitchen
            [ fst Food.FastFood.genericBurger
              fst Drink.Beer.kozelPint ]

    let bedroomItems =
        addItemsIn
            initialCity
            SpaceTypeIndex.Home
            RoomTypeIndex.Bedroom
            [ fst Furniture.Bed.ikeaBed ]

    let livingRoomItems =
        addItemsIn
            initialCity
            SpaceTypeIndex.Home
            RoomTypeIndex.LivingRoom
            [ fst Electronics.Tv.lgTv
              fst Electronics.GameConsole.xbox ]

    kitchenItems @ bedroomItems @ livingRoomItems
    |> Map.ofList

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard and starts the generation process for the game simulation
/// which includes markets for the different genres available and the game world.
let startGame (character: Character) (band: Band) =
    let world = World.get ()

    let initialCity = Map.head world.Cities

    { Bands = [ (band.Id, band) ] |> Map.ofList
      BandAlbumRepertoire = Band.AlbumRepertoire.emptyFor band.Id
      BandSongRepertoire = Band.SongRepertoire.emptyFor band.Id
      BankAccounts =
        [ (Character character.Id,
           BankAccount.forCharacterWithBalance character.Id 10000<dd>)
          (Band band.Id, BankAccount.forBand band.Id) ]
        |> Map.ofSeq
      Characters = [ (character.Id, character) ] |> Map.ofList
      CharacterSkills = [ (character.Id, Map.empty) ] |> Map.ofList
      Concerts =
        [ (band.Id, Concert.Timeline.empty) ]
        |> Map.ofList
      CurrentBandId = band.Id
      CurrentPosition = initialCity.Id, Node initialCity.Graph.StartingNode
      GenreMarkets = GenreMarket.create Data.Genres.all
      CharacterInventory = List.empty
      PlayableCharacterId = character.Id
      Situation = FreeRoam
      Today = Calendar.gameBeginning
      WorldItems = initialWorldItems initialCity }
    |> GameCreated
