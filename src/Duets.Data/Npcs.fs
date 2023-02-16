module Duets.Data.Npcs

open Duets.Common
open Duets.Entities

/// Contains all pre-defined NPCs of the game.
let all: (string * Gender) list = ResourceLoader.load Files.Npcs

/// Returns a random NPC from the pre-defined list.
let random () = all |> List.sample
