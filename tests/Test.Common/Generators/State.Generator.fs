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
      ConcertsToGenerate: int

      // <-- Generators -->
      VenueGen: Gen<Node<CityNode>> }

let defaultOptions =
    { BandFame = 25
      ConcertsToGenerate = 10
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

        let concerts =
            Gen.map2
                (fun date concert ->
                    { concert with
                          CityId = city.Id
                          VenueId = List.sample venues |> fun venue -> venue.Id
                          Date = date })
                dateGenerator
                Arb.generate<Concert>
            |> Gen.sample 0 opts.ConcertsToGenerate

        let timeline =
            { FutureEvents = Set.ofList concerts
              PastEvents = Set.empty }

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
