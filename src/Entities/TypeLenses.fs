//------------------------------------------------------------------------------
//        This code was generated by myriad.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace rec Entities

module BandRepertoireLenses =
    open Entities.Types
    let Unfinished =
        Lenses.Lens(
            (fun (x: BandRepertoire) -> x.Unfinished),
            (fun (x: BandRepertoire) (value: SongsByBand<UnfinishedSongWithQualities>) -> { x with Unfinished = value })
        )

    let Finished =
        Lenses.Lens(
            (fun (x: BandRepertoire) -> x.Finished),
            (fun (x: BandRepertoire) (value: SongsByBand<FinishedSongWithQuality>) -> { x with Finished = value })
        )
namespace rec Entities

module StateLenses =
    open Entities.Types
    let Bands =
        Lenses.Lens((fun (x: State) -> x.Bands), (fun (x: State) (value: Band list) -> { x with Bands = value }))

    let Character =
        Lenses.Lens(
            (fun (x: State) -> x.Character),
            (fun (x: State) (value: Character) -> { x with Character = value })
        )

    let CharacterSkills =
        Lenses.Lens(
            (fun (x: State) -> x.CharacterSkills),
            (fun (x: State) (value: CharacterSkills) -> { x with CharacterSkills = value })
        )

    let BandRepertoire =
        Lenses.Lens(
            (fun (x: State) -> x.BandRepertoire),
            (fun (x: State) (value: BandRepertoire) -> { x with BandRepertoire = value })
        )

    let Today =
        Lenses.Lens((fun (x: State) -> x.Today), (fun (x: State) (value: Date) -> { x with Today = value }))