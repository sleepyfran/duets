[<AutoOpen>]
module Test.Common.Root

open Aether
open Aether.Operators
open Common
open Fugit.Months
open Entities
open Simulation.Concerts.Live
open Simulation.Setup

let dummyCharacter =
    Character.from "Test" Other 24

let dummyCharacter2 =
    Character.from "Test 2" Female 35

let dummyCharacter3 =
    Character.from "Test 3" Male 28

let dummyBand =
    { Band.empty with
        Members =
            [ Band.Member.from dummyCharacter.Id Guitar (Calendar.gameBeginning) ] }

let dummyBandWithMultipleMembers =
    { Band.empty with
        Members =
            [ Band.Member.from dummyCharacter.Id Guitar (Calendar.gameBeginning)
              Band.Member.from dummyCharacter2.Id Bass (Calendar.gameBeginning)
              Band.Member.from dummyCharacter3.Id Drums (Calendar.gameBeginning) ] }

let dummySong = Song.empty

let dummyFinishedSong =
    (FinishedSong dummySong, 50<quality>)

let dummyRecordedSong =
    RecordedSong dummyFinishedSong

let dummyRecordedSongWithLength length =
    RecordedSong(FinishedSong { dummySong with Length = length }, 50<quality>)

let dummyToday = Calendar.gameBeginning

let dummyTodayMiddleOfYear =
    June 20 2021
    |> Calendar.Transform.changeDayMoment Dawn

let dummyCharacterBankAccount =
    BankAccount.forCharacter dummyCharacter.Id

let dummyBandBankAccount =
    BankAccount.forBand dummyBand.Id

let dummyTargetBankAccount =
    BankAccount.forCharacter (CharacterId(Identity.create ()))

let dummyAlbum =
    Album.from "Test Album" [ dummyRecordedSong ]

let dummyUnreleasedAlbum =
    UnreleasedAlbum dummyAlbum

let dummyReleasedAlbum =
    { Album = dummyAlbum
      ReleaseDate = dummyToday
      Streams = 0
      MaxDailyStreams = 1000
      Hype = 1.0 }

let dummyStudio =
    { Name = "Test Studio"
      Producer = dummyCharacter
      PricePerSong = 200<dd> }

let dummyConcertSpace =
    { Name = "Test Venue"
      Quality = 80<quality>
      Capacity = 1500 }

let dummyVenue =
    let lobby =
        ConcertSpaceRoom.Lobby |> World.Node.create

    World.Place.create dummyConcertSpace lobby
    |> ConcertPlace
    |> World.Node.create

let dummyCity =
    let testBoulevard =
        OutsideNode
            { Name = "Test Boulevard"
              Descriptors = []
              Type = Boulevard }
        |> World.Node.create

    World.City.create "Test City" testBoulevard
    |> World.City.addNode dummyVenue

let dummyConcert =
    { Id = Identity.create ()
      CityId = dummyCity.Id
      VenueId = dummyVenue.Id
      Date = dummyToday.AddDays(30)
      DayMoment = Night
      TicketPrice = 20<dd>
      TicketsSold = 0 }

let dummyState =
    startGame dummyCharacter dummyBand
    |> fun (GameCreated state) -> state

let dummyStateWithMultipleMembers =
    startGame dummyCharacter dummyBandWithMultipleMembers
    |> fun (GameCreated state) -> state

let dummyOngoingConcert =
    { Events = []
      Points = 0<quality>
      Concert = dummyConcert }

/// Adds a given member to the given band.
let addMember (band: Band) bandMember =
    let memberLens =
        Lenses.FromState.Bands.members_ band.Id

    Optic.map memberLens (List.append [ bandMember ])

/// Adds a given skill to the given character.
let addSkillTo (character: Character) (skillWithLevel: SkillWithLevel) =
    let (skill, _) = skillWithLevel

    let skillLens =
        Lenses.State.characterSkills_
        >-> Map.keyWithDefault_ character.Id Map.empty

    let addSkill map = Map.add skill.Id skillWithLevel map

    Optic.map skillLens addSkill

/// Applies addSkillTo multiple times for each skill given.
let addSkillsTo
    (character: Character)
    (skillsWithLevel: SkillWithLevel list)
    state
    =
    skillsWithLevel
    |> List.fold (fun state skill -> addSkillTo character skill state) state

/// Adds an unfinished song to the given state.
let addUnfinishedSong (band: Band) unfinishedSong =
    let (UnfinishedSong (song), _, _) =
        unfinishedSong

    let unfinishedSongLenses =
        Lenses.FromState.Songs.unfinishedByBand_ band.Id

    Optic.map unfinishedSongLenses (Map.add song.Id unfinishedSong)

/// Returns the last unfinished song that was created.
let lastUnfinishedSong (band: Band) state =
    state.BandSongRepertoire.UnfinishedSongs
    |> Map.find band.Id
    |> Map.head

/// Returns the last finished song that was added.
let lastFinishedSong (band: Band) state =
    state.BandSongRepertoire.FinishedSongs
    |> Map.find band.Id
    |> Map.head

/// Adds a finished song to the given state.
let addFinishedSong (band: Band) finishedSong =
    let (FinishedSong (song), _) = finishedSong

    let finishedSongLenses =
        Lenses.FromState.Songs.finishedByBand_ band.Id

    Optic.map finishedSongLenses (Map.add song.Id finishedSong)

/// Adds the specified funds to the given account.
let addFunds account amount =
    Optic.map (Lenses.FromState.BankAccount.balanceOf_ account) (fun balance ->
        balance + amount)

/// Adds the specified album to the band's released albums.
let addReleasedAlbum (band: Band) album =
    let releasedLenses =
        Lenses.FromState.Albums.releasedByBand_ band.Id

    Optic.map releasedLenses (Map.add album.Album.Id album)

let ongoingConcertFromResponse response = response.OngoingConcert

let resultFromResponse response = response.Result

let pointsFromResponse response = response.Points
