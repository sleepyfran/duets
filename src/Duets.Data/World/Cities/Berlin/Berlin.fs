module rec Duets.Data.World.Cities.Berlin.Root

open Duets.Entities

let generate () =
    let mitte = Mitte.createZone Berlin
    let schoenefeld = Schoenefeld.zone

    let city =
        World.City.create Berlin 4.5<costOfLiving> 1<utcOffset> mitte

    let kreuzberg = Kreuzberg.createZone city
    let friedrichshain = Friedrichshain.createZone
    let charlottenburg = Charlottenburg.createZone Berlin city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (charlottenburg.Id, OnlyNext(mitte.Id))
              (mitte.Id, PreviousAndNext(charlottenburg.Id, friedrichshain.Id))
              (friedrichshain.Id, OnlyPrevious(mitte.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 7<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (kreuzberg.Id, OnlyNext(mitte.Id))
              (mitte.Id, PreviousAndNext(kreuzberg.Id, schoenefeld.Id))
              (schoenefeld.Id, OnlyPrevious(mitte.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 10<minute> }

    city
    |> World.City.addZone kreuzberg
    |> World.City.addZone friedrichshain
    |> World.City.addZone charlottenburg
    |> World.City.addZone schoenefeld
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
