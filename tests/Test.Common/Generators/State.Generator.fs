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
    { BandFansMin: int
      BandFansMax: int
      FutureConcertsToGenerate: int
      PastConcertsToGenerate: int
      FlightsToGenerate: int

      // <-- Generators -->
      PastConcertGen: Gen<PastConcert>
      ScheduledConcertGen: Gen<ScheduledConcert>
      FlightGen: Gen<Flight>
      PostGen: Gen<SocialNetworkPost> }

let defaultOptions =
    { BandFansMin = 0
      BandFansMax = 25
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
            |> Gen.map (fun (ScheduledConcert (concert, scheduledOn)) ->
                ScheduledConcert(
                    { concert with
                        CityId = city.Id
                        VenueId = venueId },
                    scheduledOn
                ))
            |> Gen.sample 0 opts.FutureConcertsToGenerate

        let pastConcerts =
            opts.PastConcertGen |> Gen.sample 0 opts.PastConcertsToGenerate

        let timeline =
            { PastEvents = Set.ofList pastConcerts
              ScheduledEvents = Set.ofList scheduledConcerts }

        let! flights = Gen.listOfLength opts.FlightsToGenerate opts.FlightGen

        let! initialState = Arb.generate<State>
        let! playableCharacter = Arb.generate<Character>

        let playableCharacter =
            { playableCharacter with Attributes = Character.defaultAttributes }

        let! bandMembers = Gen.listOfLength 4 Arb.generate<CurrentMember>

        let bandCharacters =
            bandMembers
            |> List.map (fun cm ->
                let character =
                    Generators.Character.generator { Id = Some cm.CharacterId }
                    |> Gen.sample 1 1
                    |> List.head

                (cm.CharacterId, character))

        let! bandFame = Gen.choose (opts.BandFansMin, opts.BandFansMax)

        let band =
            { dummyBand with
                Fans = bandFame
                Members = bandMembers }

        return
            { initialState with
                CurrentPosition = (city.Id, venueId)
                Today = Calendar.gameBeginning
                Bands = [ (band.Id, band) ] |> Map.ofList
                CurrentBandId = band.Id
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
