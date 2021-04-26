module Storage.Savegame

type SavegameState =
  | Available
  | NotAvailable

/// Returns whether there's some savegame available or not.
let savegameState () = NotAvailable
