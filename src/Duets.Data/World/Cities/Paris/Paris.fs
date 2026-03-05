module rec Duets.Data.World.Cities.Paris.Root

open Duets.Entities

let generate () =
    let montmartre = Montmartre.createZone Paris
    let roissy = Roissy.zone

    let city =
        World.City.create Paris 5.5<costOfLiving> 1<utcOffset> montmartre

    let leMarais = LeMarais.createZone city
    let saintGermain = SaintGermain.createZone Paris city
    let laDefense = LaDefense.createZone city

    let redMetroLine =
        { Id = Red
          Stations =
            [ (montmartre.Id, OnlyNext(leMarais.Id))
              (leMarais.Id, PreviousAndNext(montmartre.Id, saintGermain.Id))
              (saintGermain.Id, OnlyPrevious(leMarais.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 6<minute> }

    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (saintGermain.Id, OnlyNext(laDefense.Id))
              (laDefense.Id, PreviousAndNext(saintGermain.Id, roissy.Id))
              (roissy.Id, OnlyPrevious(laDefense.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 12<minute> }

    city
    |> World.City.addZone leMarais
    |> World.City.addZone saintGermain
    |> World.City.addZone laDefense
    |> World.City.addZone roissy
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
