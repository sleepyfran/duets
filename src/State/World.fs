namespace State

module World =
    open Aether
    open Entities

    let move map cityId nodeId =
        map (Optic.set Lenses.State.currentPosition_ (cityId, nodeId))
