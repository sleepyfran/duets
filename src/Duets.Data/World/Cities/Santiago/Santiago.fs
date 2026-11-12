module rec Duets.Data.World.Cities.Santiago.Root

open Duets.Entities

let generate () =
    let pudahuel = Pudahuel.zone

    let city = World.City.create Santiago 3.8<costOfLiving> -4<utcOffset> pudahuel

    let providencia = Providencia.createZone city
    let santiagoCentro = SantiagoCentro.createZone city
    let bellavista = Bellavista.createZone city
    let lasCondes = LasCondes.createZone city
    let ñuñoa = Ñuñoa.createZone city

    // Red Line: Ñuñoa <-> Providencia <-> Santiago Centro <-> Bellavista
    let redMetroLine =
        { Id = Red
          Stations =
            [ (ñuñoa.Id, OnlyNext(providencia.Id))
              (providencia.Id, PreviousAndNext(ñuñoa.Id, santiagoCentro.Id))
              (santiagoCentro.Id, PreviousAndNext(providencia.Id, bellavista.Id))
              (bellavista.Id, OnlyPrevious(santiagoCentro.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 6<minute> }

    // Blue Line: Las Condes <-> Santiago Centro <-> Pudahuel
    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (lasCondes.Id, OnlyNext(santiagoCentro.Id))
              (santiagoCentro.Id, PreviousAndNext(lasCondes.Id, pudahuel.Id))
              (pudahuel.Id, OnlyPrevious(santiagoCentro.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone providencia
    |> World.City.addZone santiagoCentro
    |> World.City.addZone bellavista
    |> World.City.addZone lasCondes
    |> World.City.addZone ñuñoa
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
