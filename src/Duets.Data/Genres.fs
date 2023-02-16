module Duets.Data.Genres

open Duets.Common
open Duets.Entities

/// Contains all available genres in the game
let all: Genre list = ResourceLoader.load Files.Genres
