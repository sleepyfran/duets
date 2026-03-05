module rec Duets.Data.World.Cities.Seoul.Root

open Duets.Entities

let generate () =
    let incheon = Incheon.zone

    let city = World.City.create Seoul 4.8<costOfLiving> 9<utcOffset> incheon

    let gangnam = Gangnam.createZone city
    let hongdae = Hongdae.createZone city
    let myeongdong = Myeongdong.createZone city
    let itaewon = Itaewon.createZone city

    // Red Line: Gangnam <-> Myeongdong <-> Hongdae
    let redMetroLine =
        { Id = Red
          Stations =
            [ (gangnam.Id, OnlyNext(myeongdong.Id))
              (myeongdong.Id, PreviousAndNext(gangnam.Id, hongdae.Id))
              (hongdae.Id, OnlyPrevious(myeongdong.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 5<minute> }

    // Blue Line: Itaewon <-> Myeongdong <-> Incheon
    let blueMetroLine =
        { Id = Blue
          Stations =
            [ (itaewon.Id, OnlyNext(myeongdong.Id))
              (myeongdong.Id, PreviousAndNext(itaewon.Id, incheon.Id))
              (incheon.Id, OnlyPrevious(myeongdong.Id)) ]
            |> Map.ofList
          UsualWaitingTime = 8<minute> }

    city
    |> World.City.addZone gangnam
    |> World.City.addZone hongdae
    |> World.City.addZone myeongdong
    |> World.City.addZone itaewon
    |> World.City.addMetroLine redMetroLine
    |> World.City.addMetroLine blueMetroLine
