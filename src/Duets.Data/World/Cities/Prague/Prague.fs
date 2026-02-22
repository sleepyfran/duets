module rec Duets.Data.World.Cities.Prague.Root

open Duets.Entities

let generate () =
    let holešovice = Holešovice.zone

    let city =
        World.City.create Prague 1.6<costOfLiving> 1<utcOffset> holešovice

    let ruzyně = Ruzyně.zone
    let libeň = Libeň.zone
    let novéMěsto = NovéMěsto.createZone city
    let staréMěsto = StaréMěsto.createZone city
    let vršovice = Vršovice.zone
    let smíchov = Smíchov.createZone city
    let vinohrady = Vinohrady.createZone city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (holešovice.Id, OnlyNext novéMěsto.Id)
              (novéMěsto.Id, PreviousAndNext(holešovice.Id, vinohrady.Id))
              (vinohrady.Id, PreviousAndNext(novéMěsto.Id, vršovice.Id))
              (vršovice.Id, OnlyPrevious vinohrady.Id) ]
            |> Map.ofList
          UsualWaitingTime = 3<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (libeň.Id, OnlyNext staréMěsto.Id)
              (staréMěsto.Id, PreviousAndNext(libeň.Id, novéMěsto.Id))
              (novéMěsto.Id, PreviousAndNext(staréMěsto.Id, smíchov.Id))
              (smíchov.Id, PreviousAndNext(novéMěsto.Id, ruzyně.Id))
              (ruzyně.Id, OnlyPrevious smíchov.Id) ]
            |> Map.ofList
          UsualWaitingTime = 2<minute> }

    city
    |> World.City.addZone libeň
    |> World.City.addZone novéMěsto
    |> World.City.addZone ruzyně
    |> World.City.addZone smíchov
    |> World.City.addZone staréMěsto
    |> World.City.addZone vinohrady
    |> World.City.addZone vršovice
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
