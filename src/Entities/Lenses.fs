/// Defines all lenses into the types defined in `Types.fs`. These all need
/// to be written by hand since previously we used Myriad, but since it did not
/// support Aether style we switched back to writing them by hand.
///
/// TODO: Switch back to Myriad once the following issue is closed.
/// https://github.com/MoiraeSoftware/myriad/issues/103
module Entities.Lenses

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
    (fun (b: Band) -> b.Members), (fun v (b: Band) -> { b with Members = v })

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
