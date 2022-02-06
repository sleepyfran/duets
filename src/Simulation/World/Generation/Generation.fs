namespace Simulation.World.Generation

module Root =
    open Entities

    /// Generates the game world. Right now only creates a hard-coded city with
    /// a bunch of places interconnected, in the future this should procedurally
    /// generate the world and all the cities in it.
    let rec generate () =
        let prague = Cities.Prague.generate ()
        World.create [ prague ]
