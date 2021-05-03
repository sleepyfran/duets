//------------------------------------------------------------------------------
//        This code was generated by myriad.
//        Changes to this file will be lost when the code is regenerated.
//------------------------------------------------------------------------------
namespace rec Entities

module CharacterLenses =
    open Entities.Types
    let Id =
        Lenses.Lens((fun (x: Character) -> x.Id), (fun (x: Character) (value: CharacterId) -> { x with Id = value }))

    let Name =
        Lenses.Lens((fun (x: Character) -> x.Name), (fun (x: Character) (value: string) -> { x with Name = value }))

    let Age =
        Lenses.Lens((fun (x: Character) -> x.Age), (fun (x: Character) (value: int) -> { x with Age = value }))

    let Gender =
        Lenses.Lens((fun (x: Character) -> x.Gender), (fun (x: Character) (value: Gender) -> { x with Gender = value }))
namespace rec Entities

module MemberForHireLenses =
    open Entities.Types
    let Character =
        Lenses.Lens(
            (fun (x: MemberForHire) -> x.Character),
            (fun (x: MemberForHire) (value: Character) -> { x with Character = value })
        )

    let Role =
        Lenses.Lens(
            (fun (x: MemberForHire) -> x.Role),
            (fun (x: MemberForHire) (value: InstrumentType) -> { x with Role = value })
        )

    let Skills =
        Lenses.Lens(
            (fun (x: MemberForHire) -> x.Skills),
            (fun (x: MemberForHire) (value: SkillWithLevel list) -> { x with Skills = value })
        )
namespace rec Entities

module CurrentMemberLenses =
    open Entities.Types
    let Character =
        Lenses.Lens(
            (fun (x: CurrentMember) -> x.Character),
            (fun (x: CurrentMember) (value: Character) -> { x with Character = value })
        )

    let Role =
        Lenses.Lens(
            (fun (x: CurrentMember) -> x.Role),
            (fun (x: CurrentMember) (value: InstrumentType) -> { x with Role = value })
        )

    let Since =
        Lenses.Lens(
            (fun (x: CurrentMember) -> x.Since),
            (fun (x: CurrentMember) (value: Date) -> { x with Since = value })
        )
namespace rec Entities

module PastMemberLenses =
    open Entities.Types
    let Character =
        Lenses.Lens(
            (fun (x: PastMember) -> x.Character),
            (fun (x: PastMember) (value: Character) -> { x with Character = value })
        )

    let Role =
        Lenses.Lens(
            (fun (x: PastMember) -> x.Role),
            (fun (x: PastMember) (value: InstrumentType) -> { x with Role = value })
        )

    let Period =
        Lenses.Lens(
            (fun (x: PastMember) -> x.Period),
            (fun (x: PastMember) (value: Period) -> { x with Period = value })
        )
namespace rec Entities

module BandLenses =
    open Entities.Types
    let Id =
        Lenses.Lens((fun (x: Band) -> x.Id), (fun (x: Band) (value: BandId) -> { x with Id = value }))

    let Name =
        Lenses.Lens((fun (x: Band) -> x.Name), (fun (x: Band) (value: string) -> { x with Name = value }))

    let Genre =
        Lenses.Lens((fun (x: Band) -> x.Genre), (fun (x: Band) (value: Genre) -> { x with Genre = value }))

    let Members =
        Lenses.Lens(
            (fun (x: Band) -> x.Members),
            (fun (x: Band) (value: CurrentMember list) -> { x with Members = value })
        )

    let PastMembers =
        Lenses.Lens(
            (fun (x: Band) -> x.PastMembers),
            (fun (x: Band) (value: PastMember list) -> { x with PastMembers = value })
        )
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
        Lenses.Lens(
            (fun (x: State) -> x.Bands),
            (fun (x: State) (value: Map<BandId, Band>) -> { x with Bands = value })
        )

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

    let CurrentBandId =
        Lenses.Lens(
            (fun (x: State) -> x.CurrentBandId),
            (fun (x: State) (value: BandId) -> { x with CurrentBandId = value })
        )

    let BandRepertoire =
        Lenses.Lens(
            (fun (x: State) -> x.BandRepertoire),
            (fun (x: State) (value: BandRepertoire) -> { x with BandRepertoire = value })
        )

    let Today =
        Lenses.Lens((fun (x: State) -> x.Today), (fun (x: State) (value: Date) -> { x with Today = value }))
