module Duets.Data.Roles

open Duets.Common
open Duets.Entities

/// List of all available roles in the game.
let all = Union.allCasesOf<InstrumentType> ()
