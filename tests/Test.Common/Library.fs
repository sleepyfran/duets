[<AutoOpen>]
module Test.Common.Root

open Aether
open Aether.Operators
open Duets.Common
open Duets.Data.World
open Fugit.Months
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts.Live
open Duets.Simulation.Setup

let staticRandom value =
    { new System.Random() with
        override this.Next() = value
        override this.Next(_, _) = value }

let dummyCharacter =
    Character.from
        "Test"
        Other
        (Calendar.gameBeginning |> Calendar.Ops.addYears -24)

let dummyCharacter2 =
    Character.from
        "Test 2"
        Female
        (Calendar.gameBeginning |> Calendar.Ops.addYears -35)

let dummyCharacter3 =
    Character.from
        "Test 3"
        Male
        (Calendar.gameBeginning |> Calendar.Ops.addYears -28)

let dummyBand =
    { Band.empty with
        Name = "Dummy Band"
        Genre = "Jazz"
        Members =
            [ Band.Member.from dummyCharacter.Id Guitar (Calendar.gameBeginning) ] }

let dummyBandWithMultipleMembers =
    { dummyBand with
        Members =
            [ Band.Member.from dummyCharacter.Id Guitar (Calendar.gameBeginning)
              Band.Member.from dummyCharacter2.Id Bass (Calendar.gameBeginning)
              Band.Member.from dummyCharacter3.Id Drums (Calendar.gameBeginning) ] }

let dummySong = { Song.empty with Genre = "Jazz" }

let dummyUnfinishedSong = (UnfinishedSong dummySong, 50<quality>, 50<quality>)

let dummyFinishedSong = (FinishedSong dummySong, 50<quality>)

let dummyRecordedSong = RecordedSong dummyFinishedSong

let dummyRecordedSongWithLength length =
    RecordedSong(FinishedSong { dummySong with Length = length }, 50<quality>)

let dummyToday = Calendar.gameBeginning

let dummyTodayOneDayMomentAfter =
    dummyToday |> Calendar.Transform.changeDayMoment Morning

let dummyTodayMiddleOfYear =
    June 20 2021 |> Calendar.Transform.changeDayMoment EarlyMorning

let dummyCharacterBankAccount = BankAccount.forCharacter dummyCharacter.Id

let dummyBandBankAccount = BankAccount.forBand dummyBand.Id

let dummyTargetBankAccount =
    BankAccount.forCharacter (CharacterId(Identity.create ()))

let dummyAlbum = Album.from "Test Album" dummyRecordedSong

let dummyUnreleasedAlbum = UnreleasedAlbum dummyAlbum

let dummyReleasedAlbum =
    { Album = dummyAlbum
      ReleaseDate = dummyToday
      Streams = 0
      Hype = 1.0 }

let dummyStudio =
    { Producer = dummyCharacter
      PricePerSong = 200m<dd> }

let dummyCity =
    let world = World.get ()
    world.Cities |> Map.head

let dummyPlace =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.Home
    |> List.head

let dummyConcert =
    { Id = Identity.create ()
      CityId = Prague
      VenueId =
        Queries.World.placeIdsByTypeInCity Prague PlaceTypeIndex.ConcertSpace
        |> List.head
      Date = dummyToday.AddDays(30)
      DayMoment = Night
      TicketPrice = 20m<dd>
      TicketsSold = 0 }

let dummyPastConcert = PastConcert.PerformedConcert(dummyConcert, 100<quality>)

let dummyState =
    startGame dummyCharacter dummyBand [] dummyCity
    |> fun (GameCreated state) ->
        { state with
            GenreMarkets =
                [ "Jazz", { MarketPoint = 2.5; Fluctuation = 1.0 } ]
                |> Map.ofList }

let dummyStateWithMultipleMembers =
    startGame dummyCharacter dummyBandWithMultipleMembers [] dummyCity
    |> fun (GameCreated state) -> state

let dummyOngoingConcert =
    { Events = []
      Points = 0<quality>
      Concert = dummyConcert }

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
    let (UnfinishedSong (song), _, _) = unfinishedSong

    let unfinishedSongLenses = Lenses.FromState.Songs.unfinishedByBand_ band.Id

    Optic.map unfinishedSongLenses (Map.add song.Id unfinishedSong)

/// Returns the last unfinished song that was created.
let lastUnfinishedSong (band: Band) state =
    state.BandSongRepertoire.UnfinishedSongs |> Map.find band.Id |> Map.head

/// Returns the last finished song that was added.
let lastFinishedSong (band: Band) state =
    state.BandSongRepertoire.FinishedSongs |> Map.find band.Id |> Map.head

/// Adds a finished song to the given state.
let addFinishedSong (band: Band) finishedSong =
    let (FinishedSong (song), _) = finishedSong

    let finishedSongLenses = Lenses.FromState.Songs.finishedByBand_ band.Id

    Optic.map finishedSongLenses (Map.add song.Id finishedSong)

/// Adds the specified funds to the given account.
let addFunds account amount =
    Optic.map (Lenses.FromState.BankAccount.balanceOf_ account) (fun balance ->
        balance + amount)

/// Adds the specified album to the band's released albums.
let addReleasedAlbum (bandId: BandId) album =
    let releasedLenses = Lenses.FromState.Albums.releasedByBand_ bandId

    Optic.map releasedLenses (Map.add album.Album.Id album)

let ongoingConcertFromResponse response = response.OngoingConcert

let effectsFromResponse response = response.Effects

let resultFromResponse response = response.Result

let pointsFromResponse response = response.Points
