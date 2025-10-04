module rec Duets.Data.World.Cities.London.Root

open Duets.Entities

let generate () =
    let greenwich = Greenwich.zone

    let city = World.City.create London 6.2<costOfLiving> 0<utcOffset> greenwich

    let westEnd = WestEnd.createZone city
    let cityOfLondon = CityOfLondon.createZone city
    let camden = Camden.createZone city
    let heathrow = Heathrow.zone

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (westEnd.Id, OnlyNext(cityOfLondon.Id))
              (cityOfLondon.Id, PreviousAndNext(westEnd.Id, greenwich.Id))
              (greenwich.Id, OnlyPrevious(cityOfLondon.Id))
              (cityOfLondon.Id, PreviousAndNext(camden.Id, heathrow.Id))
              (camden.Id, OnlyNext(cityOfLondon.Id))
              (heathrow.Id, OnlyPrevious(cityOfLondon.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (westEnd.Id, OnlyNext(camden.Id))
              (camden.Id, OnlyPrevious(westEnd.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    city
    |> World.City.addZone westEnd
    |> World.City.addZone cityOfLondon
    |> World.City.addZone camden
    |> World.City.addZone greenwich
    |> World.City.addZone heathrow
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
