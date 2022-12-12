/// Defines all lenses into the types defined in `Types.fs`. These all need
/// to be written by hand since previously we used Myriad, but since it did not
/// support Aether style we switched back to writing them by hand.
module Entities.Lenses

open Aether.Operators
open Common
open Entities

module State =
    let bands_ =
        (fun (s: State) -> s.Bands), (fun v (s: State) -> { s with Bands = v })

    let bankAccounts_ =
        (fun (s: State) -> s.BankAccounts),
        (fun v (s: State) -> { s with BankAccounts = v })

    let bandSongRepertoire_ =
        (fun (s: State) -> s.BandSongRepertoire),
        (fun v (s: State) -> { s with BandSongRepertoire = v })

    let bandAlbumRepertoire_ =
        (fun (s: State) -> s.BandAlbumRepertoire),
        (fun v (s: State) -> { s with BandAlbumRepertoire = v })

    let playableCharacter_ =
        (fun (s: State) -> s.PlayableCharacterId),
        (fun v (s: State) -> { s with PlayableCharacterId = v })

    let characters_ =
        (fun (s: State) -> s.Characters),
        (fun v (s: State) -> { s with Characters = v })

    let characterSkills_ =
        (fun (s: State) -> s.CharacterSkills),
        (fun v (s: State) -> { s with CharacterSkills = v })

    let career_ =
        (fun (s: State) -> s.Career),
        (fun v (s: State) -> { s with Career = v })
    
    let concerts_ =
        (fun (s: State) -> s.Concerts),
        (fun v (s: State) -> { s with Concerts = v })

    let currentPosition_ =
        (fun (s: State) -> s.CurrentPosition),
        (fun v (s: State) -> { s with CurrentPosition = v })

    let flights_ =
        (fun (s: State) -> s.Flights),
        (fun v (s: State) -> { s with Flights = v })

    let genreMarkets_ =
        (fun (s: State) -> s.GenreMarkets),
        (fun v (s: State) -> { s with GenreMarkets = v })
    
    let characterInventory_ =
        (fun (s: State) -> s.CharacterInventory),
        (fun v (s: State) -> { s with CharacterInventory = v })

    let situation_ =
        (fun (s: State) -> s.Situation),
        (fun v (s: State) -> { s with Situation = v })

    let today_ =
        (fun (s: State) -> s.Today), (fun v (s: State) -> { s with Today = v })

module Album =
    let streams_ =
        (fun (a: ReleasedAlbum) -> a.Streams),
        (fun v (a: ReleasedAlbum) -> { a with Streams = v })

module Band =
    let id_ =
        (fun (c: Band) -> c.Id), (fun v (c: Band) -> { c with Id = v })

    let fans_ =
        (fun (b: Band) -> b.Fans), (fun v (b: Band) -> { b with Fans = v })

    let members_ =
        (fun (b: Band) -> b.Members),
        (fun v (b: Band) -> { b with Members = v })

    let pastMembers_ =
        (fun (b: Band) -> b.PastMembers),
        (fun v (b: Band) -> { b with PastMembers = v })

    module CurrentMember =
        let role_ =
            (fun (m: CurrentMember) -> m.Role),
            (fun v (m: CurrentMember) -> { m with Role = v })

module BankAccount =
    let holder_ =
        (fun (b: BankAccount) -> b.Holder),
        (fun v (b: BankAccount) -> { b with Holder = v })

    let balance_ =
        (fun (b: BankAccount) -> b.Balance),
        (fun v (b: BankAccount) -> { b with Balance = v })

module BandRepertoire =
    let unfinishedSongs_ =
        (fun (br: BandSongRepertoire) -> br.UnfinishedSongs),
        (fun v (br: BandSongRepertoire) -> { br with UnfinishedSongs = v })

    let finishedSongs_ =
        (fun (br: BandSongRepertoire) -> br.FinishedSongs),
        (fun v (br: BandSongRepertoire) -> { br with FinishedSongs = v })

    let unreleasedAlbums_ =
        (fun (br: BandAlbumRepertoire) -> br.UnreleasedAlbums),
        (fun v (br: BandAlbumRepertoire) -> { br with UnreleasedAlbums = v })

    let releasedAlbums_ =
        (fun (br: BandAlbumRepertoire) -> br.ReleasedAlbums),
        (fun v (br: BandAlbumRepertoire) -> { br with ReleasedAlbums = v })

module Character =
    let id_ =
        (fun (c: Character) -> c.Id),
        (fun v (c: Character) -> { c with Id = v })

    let birthday_ =
        (fun (c: Character) -> c.Birthday),
        (fun v (c: Character) -> { c with Birthday = v })

    let attributes_ =
        (fun (c: Character) -> c.Attributes),
        (fun v (c: Character) -> { c with Attributes = v })

    let attribute_ attr =
        attributes_ >-> Map.keyWithDefault_ attr 0

module Concerts =
    module Ongoing =
        let events_ =
            (fun (o: OngoingConcert) -> o.Events),
            (fun v (o: OngoingConcert) -> { o with Events = v })

        let points_ =
            (fun (o: OngoingConcert) -> o.Points),
            (fun v (o: OngoingConcert) -> { o with Points = v })

    module Scheduled =
        let ticketsSold_ =
            (fun (ScheduledConcert (c, _)) -> c.TicketsSold),
            (fun v (ScheduledConcert (c, s)) ->
                ScheduledConcert({ c with TicketsSold = v }, s))

    module Timeline =
        let scheduled_ =
            (fun (t: ConcertTimeline) -> t.ScheduledEvents),
            (fun v (t: ConcertTimeline) -> { t with ScheduledEvents = v })

        let pastEvents_ =
            (fun (t: ConcertTimeline) -> t.PastEvents),
            (fun v (t: ConcertTimeline) -> { t with PastEvents = v })

module Song =
    let practice_ =
        (fun (s: Song) -> s.Practice),
        (fun v (s: Song) -> { s with Practice = v })

module World =
    module City =
        let placeByTypeIndex_ =
            (fun (c: City) -> c.PlaceByTypeIndex),
            (fun v (c: City) -> { c with PlaceByTypeIndex = v })

        let placeIndex_ =
            (fun (c: City) -> c.PlaceIndex),
            (fun v (c: City) -> { c with PlaceIndex = v })

        let zoneIndex_ =
            (fun (c: City) -> c.ZoneIndex),
            (fun v (c: City) -> { c with ZoneIndex = v })

    module Place =
        let type_ =
            (fun (p: Place) -> p.Type),
            (fun v (p: Place) -> { p with Type = v })

    let cities_ =
        (fun (w: World) -> w.Cities),
        (fun v (w: World) -> { w with Cities = v })

    /// Lenses to the current city in the world given its ID.
    let city_ cityId = cities_ >-> Map.key_ cityId

module FromState =
    module Albums =
        let unreleasedByBand_ bandId =
            State.bandAlbumRepertoire_
            >-> BandRepertoire.unreleasedAlbums_
            >-> Map.keyWithDefault_ bandId Map.empty

        let releasedByBand_ bandId =
            State.bandAlbumRepertoire_
            >-> BandRepertoire.releasedAlbums_
            >-> Map.keyWithDefault_ bandId Map.empty

    module BankAccount =
        /// Lens into a specific account.
        let account_ id = State.bankAccounts_ >-> Map.key_ id

        /// Lens into the balance of a specific account.
        let balanceOf_ id =
            State.bankAccounts_ >-> Map.key_ id
            >?> BankAccount.balance_

    module Bands =
        /// Lens into a specific band given the state and its id.
        let band_ id = State.bands_ >-> Map.key_ id

        /// Lens into the members of a specific band given the state and its id.
        let members_ id = band_ id >?> Band.members_

        /// Lens into the past members of a specific band given the state and its id.
        let pastMembers_ id = band_ id >?> Band.pastMembers_

    module Concerts =
        /// Lens into all the concerts currently scheduled.
        let allByBand_ bandId =
            State.concerts_
            >-> Map.keyWithDefault_
                    bandId
                    { ScheduledEvents = Set.empty
                      PastEvents = Set.empty }

    module GenreMarkets =
        /// Lens into a specific genre market given its genre ID.
        let genreMarket_ id = State.genreMarkets_ >-> Map.key_ id

    module Songs =
        /// Lenses to the unfinished field of a specific band in its repertoire.
        let unfinishedByBand_ bandId =
            State.bandSongRepertoire_
            >-> BandRepertoire.unfinishedSongs_
            >-> Map.key_ bandId

        /// Lenses to the finished field of a specific band in its repertoire.
        let finishedByBand_ bandId =
            State.bandSongRepertoire_
            >-> BandRepertoire.finishedSongs_
            >-> Map.key_ bandId
