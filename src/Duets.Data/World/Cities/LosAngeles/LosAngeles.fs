module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let hollywood = Hollywood.zone

    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> hollywood

    let downtownLA = DowntownLA.createZone city
    let santaMonica = SantaMonica.createZone city
    let beverlyHills = BeverlyHills.createZone city
    let venice = Venice.createZone city
    let lax = LAX.zone

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (hollywood.Id, OnlyNext(downtownLA.Id))
              (downtownLA.Id, PreviousAndNext(hollywood.Id, santaMonica.Id))
              (santaMonica.Id, PreviousAndNext(downtownLA.Id, venice.Id))
              (venice.Id, OnlyPrevious(santaMonica.Id))
              (downtownLA.Id, PreviousAndNext(santaMonica.Id, lax.Id))
              (lax.Id, OnlyPrevious(downtownLA.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 9<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (downtownLA.Id, OnlyNext(beverlyHills.Id))
              (beverlyHills.Id, OnlyPrevious(downtownLA.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    city
    |> World.City.addZone downtownLA
    |> World.City.addZone santaMonica
    |> World.City.addZone beverlyHills
    |> World.City.addZone venice
    |> World.City.addZone lax
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
