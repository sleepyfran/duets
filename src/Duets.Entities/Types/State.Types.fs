namespace Duets.Entities

open Duets.Entities.SituationTypes

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    type State =
        { Bands: Bands
          BandAlbumRepertoire: BandAlbumRepertoire
          BandSongRepertoire: BandSongRepertoire
          BankAccounts: Map<BankAccountHolder, BankAccount>
          Career: Job option
          Characters: Map<CharacterId, Character>
          CharacterSkills: CharacterSkills
          Concerts: ConcertsByBand
          CurrentPosition: RoomCoordinates
          PeopleInCurrentPosition: Character list
          Flights: Flight list
          GenreMarkets: GenreMarketByGenre
          CharacterInventory: Inventory
          PlayableCharacterId: CharacterId
          Relationships: Relationships
          Rentals: CharacterRentals
          Situation: Situation
          SocialNetworks: SocialNetworks
          Today: Date
          WorldItems: WorldItems }
