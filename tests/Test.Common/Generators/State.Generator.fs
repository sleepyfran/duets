module Test.Common.Generators.State

open FsCheck

open Common
open Entities
open Test.Common

let dateGenerator =
    Arb.generate<Date>
    |> Gen.filter
        (fun date ->
            date > Calendar.gameBeginning
            && date < Calendar.gameBeginning.AddYears(2))

type StateGenOptions =
    { BandFame: int
      FutureConcertsToGenerate: int
      PastConcertsToGenerate: int

      // <-- Generators -->
      PastConcertGen: Gen<Concert>
      FutureConcertGen: Gen<Concert>
      VenueGen: Gen<Node<CityNode>> }

let defaultOptions =
    { BandFame = 25
      FutureConcertsToGenerate = 0
      PastConcertsToGenerate = 0
      PastConcertGen =
          (Concert.generator
              { Concert.defaultOptions with
                    From = dummyToday.AddYears(-2)
                    To = dummyToday })
      FutureConcertGen =
          (Concert.generator
              { Concert.defaultOptions with
                    From = dummyToday.AddDays(1)
                    To = dummyToday.AddYears(2) })
      VenueGen = City.venueGenerator }

let generator (opts: StateGenOptions) =
    gen {
        let venues = opts.VenueGen |> Gen.sample 0 10
        let firstVenue = venues |> List.head

        let city =
            { dummyCity with
                  Graph = World.Graph.from firstVenue }

        let cityWithVenues =
            venues
            |> List.tail
            |> List.fold (fun city venue -> World.City.addNode venue city) city

        let futureConcerts =
            opts.FutureConcertGen
            |> Gen.map (fun concert -> { concert with VenueId = firstVenue.Id })
            |> Gen.sample 0 opts.FutureConcertsToGenerate

        let pastConcerts =
            opts.PastConcertGen
            |> Gen.sample 0 opts.PastConcertsToGenerate

        let timeline =
            { FutureEvents = Set.ofList futureConcerts
              PastEvents = Set.ofList pastConcerts }

        let! initialState = Arb.generate<State>

        let band = { dummyBand with Fame = opts.BandFame }

        return
            { initialState with
                  Today = Calendar.gameBeginning
                  World = World.create [ cityWithVenues ]
                  Bands = [ (band.Id, band) ] |> Map.ofList
                  CurrentBandId = band.Id
                  Concerts = [ (band.Id, timeline) ] |> Map.ofList }
    }

let generateOne opts =
    generator opts |> Gen.sample 0 1 |> List.head

let generateN opts n = generator opts |> Gen.sample 0 n
