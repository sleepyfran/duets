namespace State

module Calendar =
    open Aether
    open Entities

    let setTime map time =
        map (Optic.set Lenses.State.today_ time)
