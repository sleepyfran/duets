module State.Lenses

open Aether.Operators
open Common
open Entities

module Bands =
  /// Lens into a specific band given the state and its id.
  let band_ id = Lenses.State.bands_ >-> Map.key_ id

  /// Lens into the members of a specific band given the state and its id.
  let members_ id = band_ id >?> Lenses.Band.members_

  /// Lens into the past members of a specific band given the state and its id.
  let pastMembers_ id = band_ id >?> Lenses.Band.pastMembers_

module Songs =
  /// Lenses to the unfinished field of a specific band in its repertoire.
  let unfinishedByBand_ bandId =
    Lenses.State.bandRepertoire_
    >-> Lenses.BandRepertoire.unfinished_
    >-> Map.key_ bandId

  /// Lenses to the finished field of a specific band in its repertoire.
  let finishedByBand_ bandId =
    Lenses.State.bandRepertoire_
    >-> Lenses.BandRepertoire.finished_
    >-> Map.key_ bandId
