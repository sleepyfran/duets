module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let hollywood = Hollywood.zone
    let downtownLA = DowntownLA.zone
    let lax = Lax.zone

    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> hollywood

    let koreatown = Koreatown.createZone city
    let santaMonica = SantaMonica.createZone city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (hollywood.Id, OnlyNext(downtownLA.Id))
              (downtownLA.Id, OnlyPrevious(hollywood.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (downtownLA.Id, OnlyNext(koreatown.Id))
              (koreatown.Id, PreviousAndNext(downtownLA.Id, santaMonica.Id))
              (santaMonica.Id, PreviousAndNext(koreatown.Id, lax.Id))
              (lax.Id, OnlyPrevious(santaMonica.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone koreatown
    |> World.City.addZone downtownLA
    |> World.City.addZone santaMonica
    |> World.City.addZone lax
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
