module rec Duets.Data.World.Cities.London.CityOfLondon

open Duets.Data.World.Cities
open Duets.Entities

let private bank (zone: Zone) =
    let street = World.Street.create "Bank" (StreetType.Split(East, 2))

    let bars =
        [ ("The Counting House", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("The Ned", 90<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Bank Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let private stPauls (city: City) (zone: Zone) =
    let street = World.Street.create "St Paul's" (StreetType.Split(North, 2))

    let bookstores =
        [ ("Daunt Books", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Cafe Rouge", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let radioStudios =
        [ ("Jazz FM", 88<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces radioStudios

let private barbican (zone: Zone) =
    let street = World.Street.create "Barbican" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Barbican Hall",
           1943,
           92<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Guildhall Practice Rooms", 80<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces

let private stBartsHospital (zone: Zone) =
    let street =
        World.Street.create "St Bartholomew's Hospital" StreetType.OneWay

    let hospital =
        ("St Bartholomew's Hospital", 100<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    street |> World.Street.addPlace hospital

let createZone (city: City) =
    let zone = World.Zone.create "City of London"
    let bank, metroStation = bank zone
    let stPauls = stPauls city zone
    let barbican = barbican zone
    let stBarts = stBartsHospital zone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = bank.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create bank.Id bank)
    |> World.Zone.addStreet (World.Node.create stPauls.Id stPauls)
    |> World.Zone.addStreet (World.Node.create barbican.Id barbican)
    |> World.Zone.addStreet (World.Node.create stBarts.Id stBarts)
    |> World.Zone.connectStreets bank.Id stPauls.Id North
    |> World.Zone.connectStreets stPauls.Id barbican.Id East
    |> World.Zone.connectStreets bank.Id stBarts.Id South
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addMetroStation metroStation

let zone = createZone
