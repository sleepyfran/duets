module Storage.Database

/// Returns all available roles in the game.
let roles = Data.Queries.Roles.getNames

/// Returns all available genres in the game.
let genres = Data.Queries.Genres.getAll

/// Returns all available vocal styles in the game.
let vocalStyleNames = Data.Queries.VocalStyles.getNames
