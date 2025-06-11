module rec Duets.Data.World.Cities.NewYork.Root

open Duets.Entities

let generate () =
    let astoria = Astoria.zone

    let city = World.City.create NewYork 6.0<costOfLiving> -5<utcOffset> astoria

    let soho = Soho.zone
    let midtown = Midtown.createZone city
    let brooklynHeights = BrooklynHeights.createZone city
    let harlem = Harlem.createZone city

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (astoria.Id, OnlyNext(midtown.Id))
              (midtown.Id, PreviousAndNext(astoria.Id, soho.Id))
              (soho.Id, PreviousAndNext(midtown.Id, brooklynHeights.Id))
              (brooklynHeights.Id, OnlyPrevious(soho.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (harlem.Id, OnlyNext(midtown.Id))
              (midtown.Id, OnlyPrevious(harlem.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    city
    |> World.City.addZone soho
    |> World.City.addZone midtown
    |> World.City.addZone brooklynHeights
    |> World.City.addZone harlem
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
