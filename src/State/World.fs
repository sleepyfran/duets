namespace State

module World =
    open Aether
    open Entities

    let move map cityId nodeId roomId =
        map (Optic.set Lenses.State.currentPosition_ (cityId, nodeId, roomId))
