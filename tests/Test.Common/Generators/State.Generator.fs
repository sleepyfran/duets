module Test.Common.Generators.State

open FsCheck
open Test.Common.Generators

open Duets.Data.World
open Duets.Entities
open Duets.Simulation

let dateGenerator =
    Arb.generate<Date>
    |> Gen.filter (fun date ->
        date > Calendar.gameBeginning
        && date < (Calendar.gameBeginning |> Calendar.Ops.addYears 2<years>))

type StateGenOptions =
    { InitialCityId: CityId
      BandFansMin: int<fans>
      BandFansMax: int<fans>
      Career: Job option
      CharacterMoodMin: int
      CharacterMoodMax: int
      FutureConcertsToGenerate: int
      PastConcertsToGenerate: int
      FlightsToGenerate: int
      Today: Date

      // <-- Generators -->
      PastConcertGen: Gen<PastConcert>
      ScheduledConcertGen: Gen<ScheduledConcert>
      FlightGen: Gen<Flight>
      PostGen: Gen<SocialNetworkPost> }

let defaultOptions =
    let defaultDate = Calendar.gameBeginning

    { InitialCityId = Queries.World.allCities |> List.head |> _.Id
      BandFansMin = 0<fans>
      BandFansMax = 25<fans>
      Career = None
      CharacterMoodMin = 100
      CharacterMoodMax = 100
      FutureConcertsToGenerate = 0
      PastConcertsToGenerate = 0
      FlightsToGenerate = 0
      Today = defaultDate

      PastConcertGen =
        (Concert.pastConcertGenerator
            { Concert.defaultOptions with
                From = defaultDate |> Calendar.Ops.addYears -2<years>
                To = defaultDate })
      ScheduledConcertGen =
        (Concert.scheduledConcertGenerator
            { Concert.defaultOptions with
                From = defaultDate |> Calendar.Ops.addDays 1<days>
                To = defaultDate |> Calendar.Ops.addYears 2<years> })
      FlightGen = Arb.generate<Flight>
      PostGen = Arb.generate<SocialNetworkPost> }

let generator (opts: StateGenOptions) =
    gen {
        let city = Queries.World.cityById opts.InitialCityId

        let venueId =
            Queries.World.placeIdsByTypeInCity
                city.Id
                PlaceTypeIndex.ConcertSpace
            |> List.head

        let scheduledConcerts =
            opts.ScheduledConcertGen
            |> Gen.map (fun (ScheduledConcert(concert, scheduledOn)) ->
                ScheduledConcert(concert, scheduledOn))
            |> Gen.sample 0 opts.FutureConcertsToGenerate

        let pastConcerts =
            opts.PastConcertGen |> Gen.sample 0 opts.PastConcertsToGenerate

        let timeline =
            { PastEvents = pastConcerts
              ScheduledEvents = scheduledConcerts }

        let! flights = Gen.listOfLength opts.FlightsToGenerate opts.FlightGen

        let! initialState = Arb.generate<State>
        let! playableCharacter = Arb.generate<Character>

        let! characterMood =
            Gen.choose (opts.CharacterMoodMin, opts.CharacterMoodMax)

        let playableCharacter =
            { playableCharacter with
                Attributes =
                    [ (CharacterAttribute.Energy, 100)
                      (CharacterAttribute.Health, 100)
                      (CharacterAttribute.Hunger, 100)
                      (CharacterAttribute.Mood, characterMood) ]
                    |> Map.ofList }

        let! bandMembers = Gen.listOfLength 4 Arb.generate<CurrentMember>

        let bandCharacters =
            bandMembers
            |> List.map (fun cm ->
                let character =
                    Character.generator { Id = Some cm.CharacterId }
                    |> Gen.sample 1 1
                    |> List.head

                (cm.CharacterId, character))

        let! generatedFans =
            Gen.choose (opts.BandFansMin / 1<fans>, opts.BandFansMax / 1<fans>)

        let bandFans = [ Prague, generatedFans * 1<fans> ] |> Map.ofList

        let band =
            { Band.empty with
                Name = "Test Band"
                Genre = "Jazz"
                Fans = bandFans
                Members = bandMembers }

        return
            { initialState with
                CurrentPosition = (city.Id, venueId, Ids.Common.lobby)
                Today = opts.Today
                Career = opts.Career
                Bands =
                    { Current = band.Id
                      Character = [ (band.Id, band) ] |> Map.ofList
                      Simulated = Map.empty }
                BankAccounts =
                    [ Band band.Id,
                      { Holder = Band band.Id
                        Balance = 0m<dd> }
                      Character playableCharacter.Id,
                      { Holder = Character playableCharacter.Id
                        Balance = 0m<dd> } ]
                    |> Map.ofList
                Concerts = [ (band.Id, timeline) ] |> Map.ofList
                Characters =
                    bandCharacters
                    @ [ (playableCharacter.Id, playableCharacter) ]
                    |> Map.ofList
                PlayableCharacterId = playableCharacter.Id
                GenreMarkets =
                    [ "Jazz", { MarketPoint = 2.9; Fluctuation = 1.0 } ]
                    |> Map.ofList
                Flights = flights }
    }

let generateOne opts =
    generator opts |> Gen.sample 0 1 |> List.head

let generateN opts n = generator opts |> Gen.sample 0 n
