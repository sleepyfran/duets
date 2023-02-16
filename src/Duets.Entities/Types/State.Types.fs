namespace Duets.Entities

open Duets.Entities.SituationTypes

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    type State =
        { Bands: Map<BandId, Band>
          BandAlbumRepertoire: BandAlbumRepertoire
          BandSongRepertoire: BandSongRepertoire
          BankAccounts: Map<BankAccountHolder, BankAccount>
          Career: Job option
          Characters: Map<CharacterId, Character>
          CharacterSkills: CharacterSkills
          Concerts: ConcertsByBand
          CurrentBandId: BandId
          CurrentPosition: WorldCoordinates
          Flights: Flight list
          GenreMarkets: GenreMarketByGenre
          CharacterInventory: Inventory
          PlayableCharacterId: CharacterId
          Situation: Situation
          Today: Date
          WorldItems: WorldItems }
