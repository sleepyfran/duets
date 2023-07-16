module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let london = Cities.London.Root.generate ()
    let madrid = Cities.Madrid.Root.generate ()
    let prague = Cities.Prague.Root.generate ()
    let newYork = Cities.NewYork.Root.generate ()

    World.create [ london; madrid; prague; newYork ]

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

let connectionBetween city1 city2 =
    cityNumericalId city1 + cityNumericalId city2

let distances =
    [ (connectionBetween London Madrid, 1260<km>)
      (connectionBetween London NewYork, 5570<km>)
      (connectionBetween London Prague, 1035<km>)
      (connectionBetween Madrid NewYork, 5768<km>)
      (connectionBetween Madrid Prague, 1780<km>)
      (connectionBetween NewYork Prague, 6570<km>) ]
    |> Map.ofList
