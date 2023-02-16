module Test.Common.Generators.Concert

open FsCheck
open Duets.Simulation

open Duets.Common
open Duets.Entities

type ConcertGenOptions =
    { From: Date
      To: Date
      DayMoment: DayMoment }

let defaultOptions =
    { From = Calendar.gameBeginning
      To = Calendar.gameBeginning.AddYears(10)
      DayMoment = Evening }

let generator opts =
    gen {
        let city = Queries.World.allCities |> List.head

        let venueId =
            Queries.World.placeIdsOf city.Id PlaceTypeIndex.ConcertSpace
            |> List.head

        let! concert = Arb.generate<Concert>
        let! date = Date.dateGenerator opts.From opts.To

        return
            { concert with
                CityId = city.Id
                VenueId = venueId
                Date = date
                DayMoment = opts.DayMoment }
    }

let pastConcertGenerator (opts: ConcertGenOptions) =
    gen {
        let! concert = generator opts
        let! failed = Arb.generate<bool>
        let! quality = Gen.choose (0, 100) |> Gen.map ((*) 1<quality>)

        if failed then
            return FailedConcert(concert, BandDidNotMakeIt)
        else
            return PerformedConcert(concert, quality)
    }

let scheduledConcertGenerator (opts: ConcertGenOptions) =
    gen {
        let! concert = generator opts
        let! scheduledOn = Date.dateGenerator opts.From concert.Date
        return ScheduledConcert(concert, scheduledOn)
    }
