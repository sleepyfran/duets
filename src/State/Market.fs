namespace State

module Market =
    open Aether
    open Entities

    let set map genreMarkets =
        map (Optic.set Lenses.State.genreMarkets_ genreMarkets)
