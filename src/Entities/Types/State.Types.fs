namespace Entities

[<AutoOpen>]
module StateTypes =
    /// Shared state of the game. Contains all state that is common to every part
    /// of the game.
    type State = {
        Bands: Map<BandId, Band>
        BandAlbumRepertoire: BandAlbumRepertoire
        BandSongRepertoire: BandSongRepertoire
        BankAccounts: Map<BankAccountHolder, BankAccount>
        Characters: Map<CharacterId, Character>
        CharacterSkills: CharacterSkills
        Concerts: ConcertsByBand
        CurrentBandId: BandId
        CurrentPosition: WorldCoordinates
        GenreMarkets: GenreMarketByGenre
        CharacterInventory: Inventory
        PlayableCharacterId: CharacterId
        Situation: Situation
        Today: Date
    }
