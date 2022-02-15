[<RequireQualifiedAccess>]
module Database

open Common
open Entities

let private readKey key =
    Files.dataFile key
    |> Files.readAll
    |> Option.defaultValue ""
    |> Serializer.deserialize

/// Returns all available roles in the game.
let roles = Data.Roles.getNames

/// Returns all available vocal styles in the game.
let vocalStyles = Data.VocalStyles.getWithNames

/// Returns all available genres in the game.
let genres () : Genre list = readKey Files.Genres

/// Returns all available names and genders of NPCs in the game.
let npcs () : (string * Gender) list = readKey Files.Npcs

/// Returns a randomized name and gender from an NPC of the game.
let randomNpc () = npcs () |> List.sample

/// Returns all available studios of the game.
let studios () : Studio list = readKey Files.Studios
