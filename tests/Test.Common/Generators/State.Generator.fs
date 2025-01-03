module Test.Common.Generators.State

open FsCheck

open Duets.Common
open Duets.Entities
open Test.Common
open Duets.Simulation

let dateGenerator =
    Arb.generate<Date>
    |> Gen.filter (fun date ->
        date > Calendar.gameBeginning
        && date < Calendar.gameBeginning.AddYears(2))

type StateGenOptions =
    { BandFansMin: int<fans>
      BandFansMax: int<fans>
      CharacterMoodMin: int
      CharacterMoodMax: int
      FutureConcertsToGenerate: int
      PastConcertsToGenerate: int
      FlightsToGenerate: int

      // <-- Generators -->
      PastConcertGen: Gen<PastConcert>
      ScheduledConcertGen: Gen<ScheduledConcert>
      FlightGen: Gen<Flight>
      PostGen: Gen<SocialNetworkPost> }

let defaultOptions =
    { BandFansMin = 0<fans>
      BandFansMax = 25<fans>
      CharacterMoodMin = 100
      CharacterMoodMax = 100
      FutureConcertsToGenerate = 0
      PastConcertsToGenerate = 0
      FlightsToGenerate = 0
      PastConcertGen =
        (Concert.pastConcertGenerator
            { Concert.defaultOptions with
                From = dummyToday.AddYears(-2)
                To = dummyToday })
      ScheduledConcertGen =
        (Concert.scheduledConcertGenerator
            { Concert.defaultOptions with
                From = dummyToday.AddDays(1)
                To = dummyToday.AddYears(2) })
      FlightGen = Arb.generate<Flight>
      PostGen = Arb.generate<SocialNetworkPost> }

let generator (opts: StateGenOptions) =
    gen {
        let city = Queries.World.allCities |> List.head

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
                    Generators.Character.generator { Id = Some cm.CharacterId }
                    |> Gen.sample 1 1
                    |> List.head

                (cm.CharacterId, character))

        let! generatedFans =
            Gen.choose (opts.BandFansMin / 1<fans>, opts.BandFansMax / 1<fans>)

        let bandFans = [ Prague, generatedFans * 1<fans> ] |> Map.ofList

        let band =
            { dummyBand with
                Fans = bandFans
                Members = bandMembers }

        return
            { initialState with
                CurrentPosition = (city.Id, venueId, 0)
                Today = Calendar.gameBeginning
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
