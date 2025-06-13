module rec Duets.Data.World.Cities.Madrid.Root

open Duets.Entities

let generate () =
    let centro = Centro.zone

    let city = World.City.create Madrid 5.3<costOfLiving> 1<utcOffset> centro

    let salamanca = Salamanca.createZone city
    let chamberi = Chamberi.createZone city
    let chamartin = Chamartin.createZone city
    let retiro = Retiro.createZone city
    let barajas = Barajas.zone

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (barajas.Id, OnlyNext(chamartin.Id))
              (chamartin.Id, PreviousAndNext(barajas.Id, chamberi.Id))
              (chamberi.Id, PreviousAndNext(chamartin.Id, centro.Id))
              (centro.Id, PreviousAndNext(chamberi.Id, retiro.Id))
              (retiro.Id, OnlyPrevious(centro.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (salamanca.Id, OnlyNext(centro.Id))
              (centro.Id, PreviousAndNext(salamanca.Id, chamartin.Id))
              (chamartin.Id, OnlyPrevious(centro.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    city
    |> World.City.addZone centro
    |> World.City.addZone salamanca
    |> World.City.addZone chamberi
    |> World.City.addZone chamartin
    |> World.City.addZone retiro
    |> World.City.addZone barajas
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
