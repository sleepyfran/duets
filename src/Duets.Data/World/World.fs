module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let london = Cities.London.generate ()
    let madrid = Cities.Madrid.generate ()
    let newYork = Cities.NewYork.generate ()
    let prague = Cities.Prague.generate ()
    let sydney = Cities.Sydney.generate ()
    let tokyo = Cities.Tokyo.generate ()

    World.create [ london; madrid; newYork; prague; sydney; tokyo ]

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

/// Assigns a numerical ID to a city ID so that we can sum the IDs to obtain
/// a connection inside of the distances map.
let private cityNumericalId cityId =
    match cityId with
    | London -> 0
    | Madrid -> 1
    | NewYork -> 2
    | Prague -> 3
    | Sydney -> 4
    | Tokyo -> 5

let connectionBetween city1 city2 =
    cityNumericalId city1 + cityNumericalId city2

let distances =
    [ (connectionBetween London Madrid, 1260<km>)
      (connectionBetween London NewYork, 5570<km>)
      (connectionBetween London Prague, 1035<km>)
      (connectionBetween London Sydney, 16900<km>)
      (connectionBetween London Tokyo, 9600<km>)
      (connectionBetween Madrid NewYork, 5768<km>)
      (connectionBetween Madrid Prague, 1780<km>)
      (connectionBetween Madrid Sydney, 17864<km>)
      (connectionBetween Madrid Tokyo, 10500<km>)
      (connectionBetween NewYork Prague, 6570<km>)
      (connectionBetween NewYork Sydney, 15900<km>)
      (connectionBetween NewYork Tokyo, 10800<km>)
      (connectionBetween Prague Tokyo, 90904<km>)
      (connectionBetween Prague Sydney, 16084<km>)
      (connectionBetween Sydney Tokyo, 7818<km>) ]
    |> Map.ofList
