/// Defines all lenses into the types defined in `Types.fs`. These all need
/// to be written by hand since previously we used Myriad, but since it did not
/// support Aether style we switched back to writing them by hand.
module Entities.Lenses

open Aether.Operators
open Common

module State =
    let bands_ =
        (fun (s: State) -> s.Bands), (fun v (s: State) -> { s with Bands = v })

    let bandRepertoire_ =
        (fun (s: State) -> s.BandRepertoire),
        (fun v (s: State) -> { s with BandRepertoire = v })

    let character_ =
        (fun (s: State) -> s.Character),
        (fun v (s: State) -> { s with Character = v })

    let characterSkills_ =
        (fun (s: State) -> s.CharacterSkills),
        (fun v (s: State) -> { s with CharacterSkills = v })

    let today_ =
        (fun (s: State) -> s.Today), (fun v (s: State) -> { s with Today = v })

module Band =
    let members_ =
        (fun (b: Band) -> b.Members),
        (fun v (b: Band) -> { b with Members = v })

    let pastMembers_ =
        (fun (b: Band) -> b.PastMembers),
        (fun v (b: Band) -> { b with PastMembers = v })

module BandRepertoire =
    let unfinished_ =
        (fun (br: BandRepertoire) -> br.Unfinished),
        (fun v (br: BandRepertoire) -> { br with Unfinished = v })

    let finished_ =
        (fun (br: BandRepertoire) -> br.Finished),
        (fun v (br: BandRepertoire) -> { br with Finished = v })

module FromState =
    module Bands =
        /// Lens into a specific band given the state and its id.
        let band_ id = State.bands_ >-> Map.key_ id

        /// Lens into the members of a specific band given the state and its id.
        let members_ id = band_ id >?> Band.members_

        /// Lens into the past members of a specific band given the state and its id.
        let pastMembers_ id = band_ id >?> Band.pastMembers_

    module Songs =
        /// Lenses to the unfinished field of a specific band in its repertoire.
        let unfinishedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.unfinished_
            >-> Map.key_ bandId

        /// Lenses to the finished field of a specific band in its repertoire.
        let finishedByBand_ bandId =
            State.bandRepertoire_
            >-> BandRepertoire.finished_
            >-> Map.key_ bandId
