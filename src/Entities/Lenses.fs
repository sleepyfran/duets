/// Defines all lenses into the types defined in `Types.fs`. These all need
/// to be written by hand since previously we used Myriad, but since it did not
/// support Aether style we switched back to writing them by hand.
module Entities.Lenses

open Aether.Operators
open Common

module State =
    let bands_ =
        (fun (s: State) -> s.Bands), (fun v (s: State) -> { s with Bands = v })

    let bankAccounts_ =
        (fun (s: State) -> s.BankAccounts),
        (fun v (s: State) -> { s with BankAccounts = v })

    let bandRepertoire_ =
        (fun (s: State) -> s.BandRepertoire),
        (fun v (s: State) -> { s with BandRepertoire = v })

    let character_ =
        (fun (s: State) -> s.Character),
        (fun v (s: State) -> { s with Character = v })

    let characterSkills_ =
        (fun (s: State) -> s.CharacterSkills),
        (fun v (s: State) -> { s with CharacterSkills = v })

    let genreMarkets_ =
        (fun (s: State) -> s.GenreMarkets),
        (fun v (s: State) -> { s with GenreMarkets = v })

    let today_ =
        (fun (s: State) -> s.Today), (fun v (s: State) -> { s with Today = v })

module Album =
    let streams_ =
        (fun (a: ReleasedAlbum) -> a.Streams),
        (fun v (a: ReleasedAlbum) -> { a with Streams = v })

module Band =
    let id_ =
        (fun (c: Band) -> c.Id), (fun v (c: Band) -> { c with Id = v })

    let members_ =
        (fun (b: Band) -> b.Members),
        (fun v (b: Band) -> { b with Members = v })

    let pastMembers_ =
        (fun (b: Band) -> b.PastMembers),
        (fun v (b: Band) -> { b with PastMembers = v })

module BankAccount =
    let holder_ =
        (fun (b: BankAccount) -> b.Holder),
        (fun v (b: BankAccount) -> { b with Holder = v })

    let transactions_ =
        (fun (b: BankAccount) -> b.Transactions),
        (fun v (b: BankAccount) -> { b with Transactions = v })

module BandRepertoire =
    let unfinishedSongs_ =
        (fun (br: BandRepertoire) -> br.UnfinishedSongs),
        (fun v (br: BandRepertoire) -> { br with UnfinishedSongs = v })

    let finishedSongs_ =
        (fun (br: BandRepertoire) -> br.FinishedSongs),
        (fun v (br: BandRepertoire) -> { br with FinishedSongs = v })

    let unreleasedAlbums_ =
        (fun (br: BandRepertoire) -> br.UnreleasedAlbums),
        (fun v (br: BandRepertoire) -> { br with UnreleasedAlbums = v })

    let releasedAlbums_ =
        (fun (br: BandRepertoire) -> br.ReleasedAlbums),
        (fun v (br: BandRepertoire) -> { br with ReleasedAlbums = v })

module Character =
    let id_ =
        (fun (c: Character) -> c.Id),
        (fun v (c: Character) -> { c with Id = v })

module FromState =
    module Albums =
        let unreleasedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.unreleasedAlbums_
            >-> Map.key_ bandId

        let releasedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.releasedAlbums_
            >-> Map.key_ bandId

    module BankAccount =
        /// Lens into a specific account.
        let account_ id = State.bankAccounts_ >-> Map.key_ id

        /// Lens into the transactions of a specific bank account given the
        /// state and its id.
        let transactionsOf_ id =
            State.bankAccounts_ >-> Map.key_ id
            >?> BankAccount.transactions_

    module Bands =
        /// Lens into a specific band given the state and its id.
        let band_ id = State.bands_ >-> Map.key_ id

        /// Lens into the members of a specific band given the state and its id.
        let members_ id = band_ id >?> Band.members_

        /// Lens into the past members of a specific band given the state and its id.
        let pastMembers_ id = band_ id >?> Band.pastMembers_

    module GenreMarkets =
        /// Lens into a specific genre market given its genre ID.
        let genreMarket_ id = State.genreMarkets_ >-> Map.key_ id

    module Songs =
        /// Lenses to the unfinished field of a specific band in its repertoire.
        let unfinishedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.unfinishedSongs_
            >-> Map.key_ bandId

        /// Lenses to the finished field of a specific band in its repertoire.
        let finishedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.finishedSongs_
            >-> Map.key_ bandId
