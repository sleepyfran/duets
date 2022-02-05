namespace Entities

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    /// TODO: Go over all the fields to make sure that references are only IDs
    /// instead of the whole object.
    type State =
        { Bands: Map<BandId, Band>
          BandSongRepertoire: BandSongRepertoire
          BandAlbumRepertoire: BandAlbumRepertoire
          BankAccounts: Map<BankAccountHolder, BankAccount>
          Character: Character
          CharacterSkills: CharacterSkills
          Concerts: ConcertsByBand
          CurrentBandId: BandId
          CurrentPosition: WorldCoordinates
          GenreMarkets: GenreMarketByGenre
          ScheduledEvents: ScheduledEvents
          Today: Date
          World: World }
