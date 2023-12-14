module Duets.Simulation.Migrations.Migrations

/// Attempts to apply all needed migrations to the state.
let apply state =
    [ BandMemberRelationships.migrateIfNeeded ]
    |> List.fold (fun state fn -> fn state) state
