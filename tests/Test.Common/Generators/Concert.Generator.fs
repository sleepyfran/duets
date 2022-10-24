module Test.Common.Generators.Concert

open FsCheck
open Simulation

open Common
open Entities

type ConcertGenOptions = { From: Date; To: Date }

let private dateGenerator opts =
    Arb.generate<Date>
    |> Gen.filter (fun date -> date > opts.From && date < opts.To)

let defaultOptions = {
    From = Calendar.gameBeginning
    To = Calendar.gameBeginning.AddYears(10)
}

let generator opts = gen {
    let city =
        Queries.World.allCities |> List.head

    let venueId =
        Queries.World.placeIdsOf city.Id PlaceTypeIndex.ConcertSpace
        |> List.head

    let! concert = Arb.generate<Concert>
    let! date = dateGenerator opts

    return
        { concert with
            CityId = city.Id
            VenueId = venueId
            Date = date
        }
}

let pastConcertGenerator (opts: ConcertGenOptions) = gen {
    let! concert = generator opts
    let! failed = Arb.generate<bool>
    let! quality = Gen.choose (0, 100) |> Gen.map ((*) 1<quality>)

    if failed then
        return FailedConcert(concert, BandDidNotMakeIt)
    else
        return PerformedConcert(concert, quality)
}

let scheduledConcertGenerator (opts: ConcertGenOptions) = gen {
    let! concert = generator opts
    let! scheduledOn = dateGenerator { opts with To = concert.Date }
    return ScheduledConcert(concert, scheduledOn)
}
