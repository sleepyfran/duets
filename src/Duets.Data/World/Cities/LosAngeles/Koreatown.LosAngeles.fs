module Duets.Data.World.Cities.LosAngeles.Koreatown

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let wilshireBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.wilshireBoulevardKoreatown
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("El Rey Theatre",
           771,
           86<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("The Wiltern",
           1850,
           90<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Wilshire/Vermont Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let koreatownZone = World.Zone.create Ids.Zone.koreatown

    let wilshireBoulevard, wilshireStation = wilshireBoulevard koreatownZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = wilshireBoulevard.Id
          PlaceId = wilshireStation.Id }

    koreatownZone
    |> World.Zone.addStreet (
        World.Node.create wilshireBoulevard.Id wilshireBoulevard
    )
    |> World.Zone.addMetroStation station
