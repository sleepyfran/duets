module Simulation.WorldGeneration.World

open Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let rec private generate () =
    let prague = Cities.Prague.generate ()
    World.create [ prague ]

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
