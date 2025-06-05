module Test.Common.Generators.Concert

open FsCheck
open Test.Common

open Duets.Entities

type ConcertGenOptions =
    { From: Date
      To: Date
      City: CityId
      Venue: PlaceId
      DayMoment: DayMoment }

let defaultOptions =
    { From = Calendar.gameBeginning
      To = Calendar.gameBeginning |> Calendar.Ops.addYears 10<years>
      City = Prague
      Venue = dummyVenue.Id
      DayMoment = Evening }

let generator opts =
    gen {
        let cityId = opts.City
        let venueId = opts.Venue

        let! concert = Arb.generate<Concert>
        let! date = Date.dateGenerator opts.From opts.To

        return
            { concert with
                CityId = cityId
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
