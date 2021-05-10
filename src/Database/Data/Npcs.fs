module Storage.Data.Npcs

open Entities

/// Exposes the whole pool of available names with the associated gender that
/// the name might have.
let getAll () =
    [ ("Bruce Ross", Male)
      ("Lana Hunt", Female)
      ("Joyce Parker", Other)
      ("Wilson Edwards", Male)
      ("Olivia Bennet", Female) ]
