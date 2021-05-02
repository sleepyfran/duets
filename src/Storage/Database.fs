module Storage.Database

open Common

/// Returns all available roles in the game.
let roles = Data.Roles.getNames

/// Returns all available genres in the game.
let genres = Data.Genres.getAll

/// Returns all available vocal styles in the game.
let vocalStyleNames = Data.VocalStyles.getNames

/// Returns all available names and genders of NPCs in the game.
let npcs = Data.Npcs.getAll

/// Returns a randomized name and gender from an NPC of the game.
let randomNpc () =
  let availableNpcs = npcs ()

  availableNpcs
  |> List.item (Random.between 0 (List.length availableNpcs - 1))
