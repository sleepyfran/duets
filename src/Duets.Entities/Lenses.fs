/// Defines all lenses into the types defined in `Types.fs`. These all need
/// to be written by hand since previously we used Myriad, but since it did not
/// support Aether style we switched back to writing them by hand.
module Duets.Entities.Lenses

open Aether.Operators
open Duets.Common
open Duets.Entities

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

    let peopleInCurrentPosition_ =
        (fun (s: State) -> s.PeopleInCurrentPosition),
        (fun v (s: State) -> { s with PeopleInCurrentPosition = v })

    let flights_ =
        (fun (s: State) -> s.Flights),
        (fun v (s: State) -> { s with Flights = v })

    let genreMarkets_ =
        (fun (s: State) -> s.GenreMarkets),
        (fun v (s: State) -> { s with GenreMarkets = v })

    let inventories_ =
        (fun (s: State) -> s.Inventories),
        (fun v (s: State) -> { s with Inventories = v })

    let merchPrices_ =
        (fun (s: State) -> s.MerchPrices),
        (fun v (s: State) -> { s with MerchPrices = v })

    let notifications_ =
        (fun (s: State) -> s.Notifications),
        (fun v (s: State) -> { s with Notifications = v })

    let relationships_ =
        (fun (s: State) -> s.Relationships),
        (fun v (s: State) -> { s with Relationships = v })

    let rentals_ =
        (fun (s: State) -> s.Rentals),
        (fun v (s: State) -> { s with Rentals = v })

    let situation_ =
        (fun (s: State) -> s.Situation),
        (fun v (s: State) -> { s with Situation = v })

    let socialNetworks_ =
        (fun (s: State) -> s.SocialNetworks),
        (fun v (s: State) -> { s with SocialNetworks = v })

    let today_ =
        (fun (s: State) -> s.Today), (fun v (s: State) -> { s with Today = v })

    let worldItems_ =
        (fun (s: State) -> s.WorldItems),
        (fun v (s: State) -> { s with WorldItems = v })

module Album =
    let streams_ =
        (fun (a: ReleasedAlbum) -> a.Streams),
        (fun v (a: ReleasedAlbum) -> { a with Streams = v })

    let reviews_ =
        (fun (a: ReleasedAlbum) -> a.Reviews),
        (fun v (a: ReleasedAlbum) -> { a with Reviews = v })

module Band =
    let id_ = (fun (c: Band) -> c.Id), (fun v (c: Band) -> { c with Id = v })

    let fans_ =
        (fun (b: Band) -> b.Fans), (fun v (b: Band) -> { b with Fans = v })

    let genre_ =
        (fun (b: Band) -> b.Genre), (fun v (b: Band) -> { b with Genre = v })

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

module Bands =
    let current_ =
        (fun (b: Bands) -> b.Current),
        (fun v (b: Bands) -> { b with Current = v })

    let characterBands_ =
        (fun (b: Bands) -> b.Character),
        (fun v (b: Bands) -> { b with Character = v })

    let characterBand_ id = characterBands_ >-> Map.key_ id

    let simulatedBands_ =
        (fun (b: Bands) -> b.Simulated),
        (fun v (b: Bands) -> { b with Simulated = v })

    let simulatedBand_ id = simulatedBands_ >-> Map.key_ id

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

    let moodlets_ =
        (fun (c: Character) -> c.Moodlets),
        (fun v (c: Character) -> { c with Moodlets = v })

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
            (fun (ScheduledConcert(c, _)) -> c.TicketsSold),
            (fun v (ScheduledConcert(c, s)) ->
                ScheduledConcert({ c with TicketsSold = v }, s))

    module Timeline =
        let scheduled_ =
            (fun (t: ConcertTimeline) -> t.ScheduledEvents),
            (fun v (t: ConcertTimeline) -> { t with ScheduledEvents = v })

        let pastEvents_ =
            (fun (t: ConcertTimeline) -> t.PastEvents),
            (fun v (t: ConcertTimeline) -> { t with PastEvents = v })

module Inventories =
    let character_ =
        (fun (s: Inventories) -> s.Character),
        (fun v (s: Inventories) -> { s with Character = v })

    let band_ =
        (fun (s: Inventories) -> s.Band),
        (fun v (s: Inventories) -> { s with Band = v })

module Relationships =
    let byCharacterId_ =
        (fun (s: Relationships) -> s.ByCharacterId),
        (fun v (s: Relationships) -> { s with ByCharacterId = v })

    let byMeetingCityId_ =
        (fun (s: Relationships) -> s.ByMeetingCity),
        (fun v (s: Relationships) -> { s with ByMeetingCity = v })

module SocializingState =
    let actions_ =
        (fun (s: SocializingState) -> s.Actions),
        (fun v (s: SocializingState) -> { s with Actions = v })

module SocialNetworks =
    let mastodon_ =
        (fun (s: SocialNetworks) -> s.Mastodon),
        (fun v (s: SocialNetworks) -> { s with Mastodon = v })

module SocialNetwork =
    let accounts_ =
        (fun (s: SocialNetwork) -> s.Accounts),
        (fun v (s: SocialNetwork) -> { s with Accounts = v })

    module Account =
        let posts_ =
            (fun (s: SocialNetworkAccount) -> s.Posts),
            (fun v (s: SocialNetworkAccount) -> { s with Posts = v })

module Song =
    let practice_ =
        (fun (s: Song) -> s.Practice),
        (fun v (s: Song) -> { s with Practice = v })

module World =
    module Graph =
        let startingNode_ =
            (fun (g: Graph<'a>) -> g.StartingNode),
            (fun v (g: Graph<'a>) -> { g with StartingNode = v })

        let connections_ =
            (fun (g: Graph<'a>) -> g.Connections),
            (fun v (g: Graph<'a>) -> { g with Connections = v })

        let nodes_ =
            (fun (g: Graph<'a>) -> g.Nodes),
            (fun v (g: Graph<'a>) -> { g with Nodes = v })

        let node_ nodeId = nodes_ >-> Map.key_ nodeId

        let nodeConnections_ nodeId =
            connections_ >-> Map.keyWithDefault_ nodeId Map.empty

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
            (fun (p: Place) -> p.PlaceType),
            (fun v (p: Place) -> { p with PlaceType = v })

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
            State.bankAccounts_ >-> Map.key_ id >?> BankAccount.balance_

    module Concerts =
        /// Lens into all the concerts currently scheduled.
        let allByBand_ bandId =
            State.concerts_
            >-> Map.keyWithDefault_
                    bandId
                    { ScheduledEvents = List.empty
                      PastEvents = List.empty }

    module GenreMarkets =
        /// Lens into a specific genre market given its genre ID.
        let genreMarket_ id = State.genreMarkets_ >-> Map.key_ id

    module Notifications =
        /// Lens into a specific date and day moment of the notifications.
        let forDateDayMoment_ date dayMoment =
            State.notifications_ >-> Map.keyWithDefault_ date Map.empty
            >?> Map.keyWithDefault_ dayMoment List.empty

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
