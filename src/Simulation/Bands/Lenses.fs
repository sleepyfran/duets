module Simulation.Bands.Lenses

open Aether
open Aether.Operators
open Entities

/// Lens into a specific band given the state and its id.
let band_ id = Lenses.State.bands_ >-> Map.key_ id

/// Lens into the members of a specific band given the state and its id.
let members_ id = band_ id >?> Lenses.Band.members_

/// Lens into the past members of a specific band given the state and its id.
let pastMembers_ id = band_ id >?> Lenses.Band.pastMembers_
