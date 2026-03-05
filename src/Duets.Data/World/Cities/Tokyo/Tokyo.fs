module rec Duets.Data.World.Cities.Tokyo.Root

open Duets.Entities

let generate () =
    let narita = Narita.zone

    let city = World.City.create Tokyo 6.0<costOfLiving> 9<utcOffset> narita

    let shibuya = Shibuya.createZone city
    let shinjuku = Shinjuku.createZone city
    let roppongi = Roppongi.createZone city
    let akihabara = Akihabara.createZone city

    // Red Line: Shibuya <-> Akihabara <-> Shinjuku
    let redMetroLine =
        { Id = Red
          Stations =
            [ (shibuya.Id, OnlyNext(akihabara.Id))
              (akihabara.Id, PreviousAndNext(shibuya.Id, shinjuku.Id))
              (shinjuku.Id, OnlyPrevious(akihabara.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 6<minute> }

    // Blue Line: Roppongi <-> Akihabara <-> Narita
    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (roppongi.Id, OnlyNext(akihabara.Id))
              (akihabara.Id, PreviousAndNext(roppongi.Id, narita.Id))
              (narita.Id, OnlyPrevious(akihabara.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 12<minute> }

    city
    |> World.City.addZone shibuya
    |> World.City.addZone shinjuku
    |> World.City.addZone roppongi
    |> World.City.addZone akihabara
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
