[<AutoOpen>]
module Test.Common.Root

open System
open Aether
open Aether.Operators
open Duets.Common
open Duets.Data.World
open Fugit.Months
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts.Live
open Duets.Simulation.Setup

type RandomGenDisposable() =
    interface IDisposable with
        member this.Dispose() = RandomGen.reset ()

let private randomImpl impl =
    { new Random() with
        override this.Next() = impl ()
        override this.Next(_, _) = impl () }
    |> RandomGen.change

    new RandomGenDisposable()

/// Changes the implementation of the `RandomGen` to always return the given value.
/// Automatically resets back to the default implementation when the returned
/// disposable is disposed.
let changeToStaticRandom value = randomImpl (fun _ -> value)

/// Changes the implementation of the `RandomGen` to return the given values in order.
/// Automatically resets back to the default implementation when the returned
/// disposable is disposed.
let changeToOrderedRandom (values: int list) =
    let possibleValues = ResizeArray(values |> List.ofSeq)

    let shift () =
        if possibleValues.Count = 0 then
            failwith
                $"Ran out of values after {values.Length} calls. Add more values to the initial list."
        else
            let value = possibleValues[0]
            possibleValues.RemoveAt(0)
            value

    randomImpl shift

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

let dummyHeadlinerBand =
    { dummyBand with
        Id = Identity.create () |> BandId
        Name = "The Headliners"
        Fans = [ Prague, 12000<fans> ] |> Map.ofList }

let dummySong = Song.empty

let dummyUnfinishedSong = Unfinished(dummySong, 50<quality>, 50<quality>)

let dummyFinishedSong = Finished(dummySong, 50<quality>)

let dummyRecordedSong = Recorded(dummySong, 50<quality>)
let dummyRecordedSongRef = Recorded(dummySong.Id, 50<quality>)

let dummyRecordedSongWithLength length =
    Recorded({ dummySong with Length = length }, 50<quality>)

let dummyToday = Calendar.gameBeginning

let dummyTodayOneDayMomentAfter =
    dummyToday |> Calendar.Transform.changeDayMoment Morning

let dummyTodayMiddleOfYear =
    June 20 2021 |> Calendar.Transform.changeDayMoment EarlyMorning

let dummyCharacterBankAccount = BankAccount.forCharacter dummyCharacter.Id

let dummyBandBankAccount = BankAccount.forBand dummyBand.Id

let dummyTargetBankAccount =
    BankAccount.forCharacter (CharacterId(Identity.create ()))

let dummyAlbum = Album.from dummyBand "Test Album" dummyRecordedSongRef

let dummyUnreleasedAlbum =
    { Album = dummyAlbum
      SelectedProducer = SelectedProducer.StudioProducer }

let dummyReleasedAlbum =
    { Album = dummyAlbum
      ReleaseDate = dummyToday
      Streams = 0
      Hype = 1.0
      Reviews = [] }

let dummyStudio =
    { Producer = dummyCharacter
      PricePerSong = 200m<dd> }

let dummyCity =
    let world = World.get
    world.Cities |> Map.find Prague

let dummyAirport =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.Airport
    |> List.head

let dummyPlace =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.Home
    |> List.head

let dummyHotel1 =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.Hotel
    |> List.head

let dummyHotel2 =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.Hotel
    |> List.item 1

let dummyVenue =
    Queries.World.placesByTypeInCity dummyCity.Id PlaceTypeIndex.ConcertSpace
    |> List.head

let dummyConcert =
    { Id = Identity.create ()
      CityId = Prague
      VenueId = dummyVenue.Id
      Date = dummyToday.AddDays(30)
      DayMoment = Night
      TicketPrice = 20m<dd>
      TicketsSold = 0
      ParticipationType = Headliner }

let dummyPastConcert = PastConcert.PerformedConcert(dummyConcert, 100<quality>)

let dummyState =
    RandomGen.reset ()

    let effect = startGame dummyCharacter dummyBand [] dummyCity

    match effect with
    | GameCreated state ->
        { state with
            GenreMarkets =
                [ "Jazz", { MarketPoint = 2.5; Fluctuation = 1.0 } ]
                |> Map.ofList }
    | _ -> failwith "Unexpected effect while creating game."

let dummyStateWithMultipleMembers =
    let effect =
        startGame dummyCharacter dummyBandWithMultipleMembers [] dummyCity

    match effect with
    | GameCreated state -> state
    | _ -> failwith "Unexpected effect while creating game."

let dummyOngoingConcert =
    { Events = []
      Points = 0<quality>
      Checklist =
        { MerchStandSetup = false
          SoundcheckDone = false }
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
let addUnfinishedSong (band: Band) (unfinishedSong: Unfinished<Song>) =
    let (Unfinished(song, _, _)) = unfinishedSong

    let unfinishedSongLenses = Lenses.FromState.Songs.unfinishedByBand_ band.Id

    Optic.map unfinishedSongLenses (Map.add song.Id unfinishedSong)

/// Returns the last unfinished song that was created.
let lastUnfinishedSong (band: Band) state =
    state.BandSongRepertoire.UnfinishedSongs |> Map.find band.Id |> Map.head

/// Returns the last finished song that was added.
let lastFinishedSong (band: Band) state =
    state.BandSongRepertoire.FinishedSongs |> Map.find band.Id |> Map.head

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
