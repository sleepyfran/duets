namespace Entities

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    type State =
        { Bands: Map<BandId, Band>
          BandSongRepertoire: BandSongRepertoire
          BandAlbumRepertoire: BandAlbumRepertoire
          BankAccounts: Map<BankAccountHolder, BankAccount>
          Character: Character
          CharacterSkills: CharacterSkills
          Concerts: ConcertContext
          CurrentBandId: BandId
          CurrentPosition: Coordinates
          GenreMarkets: GenreMarketByGenre
          Schedule: ScheduledEvents
          Today: Date
          World: World }
