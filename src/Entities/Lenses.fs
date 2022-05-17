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

    let concerts_ =
        (fun (s: State) -> s.Concerts),
        (fun v (s: State) -> { s with Concerts = v })

    let currentPosition_ =
        (fun (s: State) -> s.CurrentPosition),
        (fun v (s: State) -> { s with CurrentPosition = v })

    let genreMarkets_ =
        (fun (s: State) -> s.GenreMarkets),
        (fun v (s: State) -> { s with GenreMarkets = v })

    let situation_ =
        (fun (s: State) -> s.Situation),
        (fun v (s: State) -> { s with Situation = v })

    let today_ =
        (fun (s: State) -> s.Today), (fun v (s: State) -> { s with Today = v })

    let world_ =
        (fun (s: State) -> s.World), (fun v (s: State) -> { s with World = v })

module Album =
    let streams_ =
        (fun (a: ReleasedAlbum) -> a.Streams),
        (fun v (a: ReleasedAlbum) -> { a with Streams = v })

module Band =
    let id_ =
        (fun (c: Band) -> c.Id), (fun v (c: Band) -> { c with Id = v })

    let fame_ =
        (fun (b: Band) -> b.Fame), (fun v (b: Band) -> { b with Fame = v })

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

    let status_ =
        (fun (c: Character) -> c.Status),
        (fun v (c: Character) -> { c with Status = v })

    module Status =
        let energy_ =
            (fun (c: CharacterStatus) -> c.Energy),
            (fun v (c: CharacterStatus) -> { c with Energy = v })

        let health_ =
            (fun (c: CharacterStatus) -> c.Health),
            (fun v (c: CharacterStatus) -> { c with Health = v })

    let energy_ = status_ >-> Status.energy_

    let health_ = status_ >-> Status.health_

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
            (fun (ScheduledConcert c) -> c.TicketsSold),
            (fun v (ScheduledConcert c) ->
                ScheduledConcert { c with TicketsSold = v })

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
    let cities_ =
        (fun (w: World) -> w.Cities),
        (fun v (w: World) -> { w with Cities = v })

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
            connections_
            >-> Map.keyWithDefault_ nodeId Map.empty

    module Place =
        let space_ =
            (fun (p: Place) -> p.SpaceType),
            (fun v (p: Place) -> { p with SpaceType = v })

        let rooms_ =
            (fun (p: Place) -> p.Rooms),
            (fun v (p: Place) -> { p with Rooms = v })

        let exits =
            (fun (p: Place) -> p.Exits),
            (fun v (p: Place) -> { p with Exits = v })

    module City =
        let graph_ =
            (fun (c: City) -> c.Graph),
            (fun v (c: City) -> { c with Graph = v })

        let startingNode_ =
            graph_ >-> Graph.startingNode_

        let connections_ =
            graph_ >-> Graph.connections_

        let nodes_ = graph_ >-> Graph.nodes_

        let nodeConnections_ nodeId =
            graph_ >-> Graph.nodeConnections_ nodeId

module FromState =
    module Albums =
        let unreleasedByBand_ bandId =
            State.bandAlbumRepertoire_
            >-> BandRepertoire.unreleasedAlbums_
            >-> Map.key_ bandId

        let releasedByBand_ bandId =
            State.bandAlbumRepertoire_
            >-> BandRepertoire.releasedAlbums_
            >-> Map.key_ bandId

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

    module World =
        /// Lenses to the cities field in the world.
        let cities_ = State.world_ >-> World.cities_

        /// Lenses to the current city in the world given its ID.
        let city_ cityId =
            State.world_ >-> World.cities_ >-> Map.key_ cityId

        /// Lenses to the city graph given its ID.
        let cityGraph_ cityId = city_ cityId >?> World.City.graph_

        /// Lenses to a specific city node in the world given its city and node IDs.
        let node_ cityId nodeId =
            cityGraph_ cityId
            >?> World.Graph.nodes_
            >?> Map.key_ nodeId
