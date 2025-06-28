module rec Duets.Data.World.Cities.LosAngeles.Root

open Duets.Entities

let generate () =
    let hollywood = Hollywood.zone
    let downtownLA = DowntownLA.zone

    let redMetroLine =
        { Id = Red
          Stations =
            [ (hollywood.Id, OnlyNext downtownLA.Id)
              (downtownLA.Id, OnlyPrevious hollywood.Id) ]
            |> Map.ofList
          UsualWaitingTime = 9<minute> }

    let city =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset> hollywood
        |> World.City.addZone downtownLA
        |> World.City.addMetroLine redMetroLine

    city
