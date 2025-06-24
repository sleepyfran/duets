module rec Duets.Data.World.Cities.Sydney.Root

open Duets.Entities

let generate () =
    let bondi = Bondi.zone
    let newtown = Newtown.zone
    let northSydney = NorthSydney.zone
    let mascot = Mascot.zone

    let city = World.City.create Sydney 5.2<costOfLiving> 10<utcOffset> bondi

    let cbd = CBD.createZone city

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (cbd.Id, OnlyNext(bondi.Id))
              (bondi.Id, PreviousAndNext(cbd.Id, mascot.Id))
              (mascot.Id, OnlyPrevious(bondi.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (cbd.Id, OnlyNext(newtown.Id))
              (newtown.Id, PreviousAndNext(cbd.Id, northSydney.Id))
              (northSydney.Id, OnlyPrevious(newtown.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    city
    |> World.City.addZone cbd
    |> World.City.addZone bondi
    |> World.City.addZone newtown
    |> World.City.addZone northSydney
    |> World.City.addZone mascot
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
