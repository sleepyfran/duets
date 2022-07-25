module Data.Genres

open Common
open Entities

/// Contains all available genres in the game
let all : Genre list = ResourceLoader.load Files.Genres
