module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let downtownLA = DowntownLA.zone
    let westside = Westside.zone
    let koreatown = Koreatown.zone
    let griffithPark = GriffithPark.zone
    let echoParkSilverLake = EchoParkSilverLake.zone

    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> downtownLA

    let hollywood = Hollywood.createZone city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (hollywood.Id, OnlyNext downtownLA.Id)
              (downtownLA.Id, PreviousAndNext(hollywood.Id, westside.Id))
              (westside.Id, PreviousAndNext(downtownLA.Id, koreatown.Id))
              (koreatown.Id, OnlyPrevious westside.Id) ]
            |> Map.ofList
          UsualWaitingTime = 9<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (griffithPark.Id, OnlyNext downtownLA.Id)
              (downtownLA.Id,
               PreviousAndNext(griffithPark.Id, echoParkSilverLake.Id))
              (echoParkSilverLake.Id, OnlyPrevious downtownLA.Id) ]
            |> Map.ofList
          UsualWaitingTime = 12<minute> }

    city
    |> World.City.addZone hollywood
    |> World.City.addZone westside
    |> World.City.addZone koreatown
    |> World.City.addZone griffithPark
    |> World.City.addZone echoParkSilverLake
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
