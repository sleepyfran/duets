module rec Duets.Data.World.Cities.Tokyo.Root

open Duets.Entities

let generate () =
    let shibuya = Shibuya.zone

    // Cost of living and timezone roughly matching Tokyo (UTC+9)
    let city =
        World.City.create Tokyo 5.3<costOfLiving> 9<utcOffset> shibuya

    let shinjuku = Shinjuku.createZone city
    let ginza = Ginza.createZone city
    let asakusa = Asakusa.createZone city
    let haneda = Haneda.zone

    // Blue line connects the airport with the city centre and historic area
    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (haneda.Id, OnlyNext(shibuya.Id))
              (shibuya.Id, PreviousAndNext(haneda.Id, asakusa.Id))
              (asakusa.Id, OnlyPrevious(shibuya.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 5<minute> }

    // Red line connects the upscale shopping and business districts with Shibuya
    let redMetroLine =
        { Id = Red
          Stations =
            [ (ginza.Id, OnlyNext(shinjuku.Id))
              (shinjuku.Id, PreviousAndNext(ginza.Id, shibuya.Id))
              (shibuya.Id, OnlyPrevious(shinjuku.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 6<minute> }

    city
    |> World.City.addZone shinjuku
    |> World.City.addZone ginza
    |> World.City.addZone asakusa
    |> World.City.addZone haneda
    |> World.City.addMetroLine blueMetroLine
    |> World.City.addMetroLine redMetroLine
