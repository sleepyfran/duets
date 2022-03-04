module Test.Common.Generators.Concert

open FsCheck
open Test.Common

open Common
open Entities

type ConcertGenOptions =
    { From: Date
      To: Date
      CityId: CityId

      // <-- Generators -->
      VenueGen: Gen<Node<CityNode>> }

let private dateGenerator opts =
    Arb.generate<Date>
    |> Gen.filter (fun date -> date > opts.From && date < opts.To)

let defaultOptions =
    { From = Calendar.gameBeginning
      To = Calendar.gameBeginning.AddYears(10)
      CityId = dummyCity.Id
      VenueGen = City.venueGenerator }

let generator opts =
    gen {
        let venues = opts.VenueGen |> Gen.sample 0 10
        let! concert = Arb.generate<Concert>
        let! date = dateGenerator opts

        return
            { concert with
                  CityId = opts.CityId
                  VenueId = List.sample venues |> fun venue -> venue.Id
                  Date = date }
    }

let pastConcertGenerator (opts: ConcertGenOptions) =
    gen {
        let! concert = generator opts
        let! failed = Arb.generate<bool>
        let! quality = Gen.choose (0, 100) |> Gen.map ((*) 1<quality>)

        if failed then
            return FailedConcert concert
        else
            return PerformedConcert(concert, quality)
    }

let scheduledConcertGenerator (opts: ConcertGenOptions) =
    gen {
        let! concert = generator opts
        return ScheduledConcert concert
    }
