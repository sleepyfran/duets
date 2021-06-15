module Test.Common

open Aether
open Aether.Operators
open Common
open Entities
open Simulation.Setup.StartGame

let dummyCharacter =
    Character.from "Test" 24 Other |> Result.unwrap

let dummyBand =
    { Band.empty with
          Members =
              [ Band.Member.from dummyCharacter Guitar (Calendar.gameBeginning) ] }

let dummySong = Song.empty
let dummyFinishedSong = (FinishedSong dummySong, 50<quality>)
let dummyRecordedSong = RecordedSong dummyFinishedSong

let dummyRecordedSongWithLength length =
    RecordedSong(FinishedSong { dummySong with Length = length }, 50<quality>)

let dummyToday = Calendar.gameBeginning

let dummyCharacterBankAccount =
    BankAccount.forCharacter dummyCharacter.Id

let dummyBandBankAccount = BankAccount.forBand dummyBand.Id

let dummyTargetBankAccount =
    BankAccount.forCharacter (CharacterId(Identity.create ()))

let dummyAlbum =
    Album.from "Test Album" [ dummyRecordedSong ]
    |> Result.unwrap

let dummyUnreleasedAlbum = UnreleasedAlbum dummyAlbum

let dummyReleasedAlbum = ReleasedAlbum(dummyAlbum, dummyToday)

let dummyStudio =
    { Id = Identity.create ()
      Name = "Test Studio"
      Producer = Producer <| dummyCharacter
      PricePerSong = 200<dd> }

let dummyState =
    startGame dummyCharacter dummyBand
    |> fun (GameCreated state) -> state

/// Adds a given member to the given band.
let addMember (band: Band) bandMember =
    let memberLens = Lenses.FromState.Bands.members_ band.Id

    Optic.map memberLens (List.append [ bandMember ])

/// Adds a given skill to the given character.
let addSkillTo (character: Character) (skillWithLevel: SkillWithLevel) =
    let (skill, _) = skillWithLevel

    let skillLens =
        Lenses.State.characterSkills_
        >-> Map.key_ character.Id

    let addSkill map = Map.add skill.Id skillWithLevel map

    Optic.map skillLens addSkill

/// Adds an unfinished song to the given state.
let addUnfinishedSong (band: Band) unfinishedSong =
    let (UnfinishedSong (song), _, _) = unfinishedSong

    let unfinishedSongLenses =
        Lenses.FromState.Songs.unfinishedByBand_ band.Id

    Optic.map unfinishedSongLenses (Map.add song.Id unfinishedSong)

/// Returns the last unfinished song that was created.
let lastUnfinishedSong (band: Band) state =
    state.BandRepertoire.UnfinishedSongs
    |> Map.find band.Id
    |> Map.head

/// Returns the last finished song that was added.
let lastFinishedSong (band: Band) state =
    state.BandRepertoire.FinishedSongs
    |> Map.find band.Id
    |> Map.head

/// Adds a finished song to the given state.
let addFinishedSong (band: Band) finishedSong =
    let (FinishedSong (song), _) = finishedSong

    let finishedSongLenses =
        Lenses.FromState.Songs.finishedByBand_ band.Id

    Optic.map finishedSongLenses (Map.add song.Id finishedSong)

/// Adds the specified funds to the given account.
let addFunds account amount state =
    let add transactions =
        List.append transactions [ Incoming amount ]

    Optic.map (Lenses.FromState.BankAccount.transactionsOf_ account) add state
