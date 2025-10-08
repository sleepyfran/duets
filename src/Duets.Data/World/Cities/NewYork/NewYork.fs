module rec Duets.Data.World.Cities.NewYork.Root

open Duets.Entities

let generate () =
    let jamaica = Jamaica.zone

    let city =
        World.City.create NewYork 6.0<costOfLiving> -5<utcOffset> jamaica

    let brooklyn = Brooklyn.zone city
    let midtownWest = MidtownWest.createZone city
    let lowerManhattan = LowerManhattan.createZone city

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (midtownWest.Id, OnlyNext(lowerManhattan.Id))
              (lowerManhattan.Id, PreviousAndNext(midtownWest.Id, brooklyn.Id))
              (brooklyn.Id, PreviousAndNext(lowerManhattan.Id, jamaica.Id))
              (jamaica.Id, OnlyPrevious(brooklyn.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone midtownWest
    |> World.City.addZone lowerManhattan
    |> World.City.addZone brooklyn
    |> World.City.addMetroLine blueMetroLine
