module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let hollywood = Hollywood.zone

    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> hollywood

    let downtownLA = DowntownLA.createZone city
    let santaMonica = SantaMonica.createZone city

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (hollywood.Id, OnlyNext(downtownLA.Id))
              (downtownLA.Id, NextAndPrevious(hollywood.Id, santaMonica.Id))
              (santaMonica.Id, OnlyPrevious(downtownLA.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone downtownLA
    |> World.City.addMetroLine blueMetroLine
