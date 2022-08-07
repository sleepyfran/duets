module Test.Common.Generators.State

open FsCheck

open Common
open Entities
open Test.Common
open Simulation

let dateGenerator =
    Arb.generate<Date>
    |> Gen.filter (fun date ->
        date > Calendar.gameBeginning
        && date < Calendar.gameBeginning.AddYears(2))

type StateGenOptions =
    { BandFame: int
      FutureConcertsToGenerate: int
      PastConcertsToGenerate: int

      // <-- Generators -->
      PastConcertGen: Gen<PastConcert>
      ScheduledConcertGen: Gen<ScheduledConcert> }

let defaultOptions =
    { BandFame = 25
      FutureConcertsToGenerate = 0
      PastConcertsToGenerate = 0
      PastConcertGen =
        (Concert.pastConcertGenerator
            { Concert.defaultOptions with
                From = dummyToday.AddYears(-2)
                To = dummyToday })
      ScheduledConcertGen =
        (Concert.scheduledConcertGenerator
            { Concert.defaultOptions with
                From = dummyToday.AddDays(1)
                To = dummyToday.AddYears(2) }) }

let generator (opts: StateGenOptions) =
    gen {
        let city =
            Queries.World.Common.allCities |> List.head

        let venueId, _, _ =
            Queries.World.ConcertSpace.allInCity city.Id
            |> List.head

        let scheduledConcerts =
            opts.ScheduledConcertGen
            |> Gen.map (fun (ScheduledConcert concert) ->
                ScheduledConcert
                    { concert with
                        CityId = city.Id
                        VenueId = venueId })
            |> Gen.sample 0 opts.FutureConcertsToGenerate

        let pastConcerts =
            opts.PastConcertGen
            |> Gen.sample 0 opts.PastConcertsToGenerate

        let timeline =
            { PastEvents = Set.ofList pastConcerts
              ScheduledEvents = Set.ofList scheduledConcerts }

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

        let band =
            { dummyBand with
                Fame = opts.BandFame
                Members = bandMembers }

        return
            { initialState with
                CurrentPosition = (city.Id, Node venueId)
                Today = Calendar.gameBeginning
                Bands = [ (band.Id, band) ] |> Map.ofList
                CurrentBandId = band.Id
                Concerts = [ (band.Id, timeline) ] |> Map.ofList
                Characters =
                    bandCharacters
                    @ [ (playableCharacter.Id, playableCharacter) ]
                    |> Map.ofList
                PlayableCharacterId = playableCharacter.Id }
    }

let generateOne opts =
    generator opts |> Gen.sample 0 1 |> List.head

let generateN opts n = generator opts |> Gen.sample 0 n
