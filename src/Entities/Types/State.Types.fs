namespace Entities

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    type State =
        { Bands: Map<BandId, Band>
          Character: Character
          CharacterSkills: CharacterSkills
          CurrentBandId: BandId
          CurrentPosition: WorldCoordinates
          BandSongRepertoire: BandSongRepertoire
          BandAlbumRepertoire: BandAlbumRepertoire
          BankAccounts: Map<BankAccountHolder, BankAccount>
          Today: Date
          GenreMarkets: GenreMarketByGenre
          World: World }
