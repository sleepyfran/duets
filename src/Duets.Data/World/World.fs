module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let london = Cities.London.generate ()
    let madrid = Cities.Madrid.generate ()
    let mexicoCity = Cities.MexicoCity.generate ()
    let newYork = Cities.NewYork.generate ()
    let prague = Cities.Prague.generate ()
    let sydney = Cities.Sydney.generate ()
    let tokyo = Cities.Tokyo.generate ()

    World.create [ london; madrid; mexicoCity; newYork; prague; sydney; tokyo ]

/// Returns the game world. This function internally memos the calls to
/// the world generation so that it will only generate the game world once
/// and just return the cached version all the time afterwards.
let get =
    let mutable cachedWorld: World option = None

    let rec getOrGenerate () =
        match cachedWorld with
        | Some world -> world
        | None ->
            let world = generate ()

            cachedWorld <- Some world
            world

    getOrGenerate

/// Map of distances between cities in kilometers.
let private distances =
    [ ((London, Madrid), 1260<km>)
      ((London, NewYork), 5570<km>)
      ((London, MexicoCity), 8904<km>)
      ((London, Prague), 1035<km>)
      ((London, Sydney), 16900<km>)
      ((London, Tokyo), 9600<km>)
      ((Madrid, MexicoCity), 9066<km>)
      ((Madrid, NewYork), 5768<km>)
      ((Madrid, Prague), 1780<km>)
      ((Madrid, Sydney), 17864<km>)
      ((Madrid, Tokyo), 10500<km>)
      ((MexicoCity, NewYork), 3366<km>)
      ((MexicoCity, Prague), 9907<km>)
      ((MexicoCity, Sydney), 12982<km>)
      ((MexicoCity, Tokyo), 11312<km>)
      ((NewYork, Prague), 6570<km>)
      ((NewYork, Sydney), 15900<km>)
      ((NewYork, Tokyo), 10800<km>)
      ((Prague, Tokyo), 90904<km>)
      ((Prague, Sydney), 16084<km>)
      ((Sydney, Tokyo), 7818<km>) ]
    |> Map.ofList

/// Returns the distance between the given cities.
let distanceBetween city1 city2 =
    let key = if city1 < city2 then (city1, city2) else (city2, city1)
    distances |> Map.find key
