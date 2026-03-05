module rec Duets.Data.World.Cities.Toronto.Root

open Duets.Entities

let generate () =
    let queenWest = QueenWest.zone
    let pearson = Pearson.zone

    let city =
        World.City.create Toronto 4.8<costOfLiving> -5<utcOffset> queenWest

    let downtownCore = DowntownCore.createZone city
    let eastEnd = EastEnd.createZone city
    let midtown = Midtown.createZone city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (pearson.Id, OnlyNext(midtown.Id))
              (midtown.Id, PreviousAndNext(pearson.Id, downtownCore.Id))
              (downtownCore.Id, OnlyPrevious(midtown.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (queenWest.Id, OnlyNext(downtownCore.Id))
              (downtownCore.Id, PreviousAndNext(queenWest.Id, eastEnd.Id))
              (eastEnd.Id, OnlyPrevious(downtownCore.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone downtownCore
    |> World.City.addZone eastEnd
    |> World.City.addZone midtown
    |> World.City.addZone pearson
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
