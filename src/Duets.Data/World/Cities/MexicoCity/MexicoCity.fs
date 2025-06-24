module rec Duets.Data.World.Cities.MexicoCity.Root

open Duets.Entities

let generate () =
    let centro = Centro.zone
    let polanco = Polanco.zone
    let coyoacan = Coyoacan.zone
    let aeropuerto = Aeropuerto.zone

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (centro.Id, OnlyNext(polanco.Id))
              (polanco.Id, PreviousAndNext(centro.Id, coyoacan.Id))
              (coyoacan.Id, OnlyPrevious(polanco.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    let redMetroLine =
        { Id = Red
          Stations =
            [ (centro.Id, OnlyNext(aeropuerto.Id))
              (aeropuerto.Id, OnlyPrevious(centro.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    World.City.create MexicoCity 3.2<costOfLiving> -6<utcOffset> centro
    |> World.City.addZone centro
    |> World.City.addZone polanco
    |> World.City.addZone coyoacan
    |> World.City.addZone aeropuerto
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
