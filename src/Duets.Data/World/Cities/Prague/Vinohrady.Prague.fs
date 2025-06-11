module rec Duets.Data.World.Cities.Prague.Vinohrady

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let residentialStreet city (zone: Zone) =
    let street = World.Street.create "Korunní" (StreetType.Split(North, 2))

    let home = PlaceCreators.createHome street.Id zone.Id

    let gyms =
        [ ("Sportovní Centrum Arel", 90<quality>, zone.Id)
          ("Praha Fitness Vinohrady", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bars =
        [ ("PourHouse", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Brew and Foam", 91<quality>, zone.Id)
          ("The Roasted Bean", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces cafes

    street

let náměstíMíru city (zone: Zone) =
    let street = World.Street.create "Náměstí Míru" (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("Retro Music Hall",
           1000,
           80<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Royal Theatre",
           300,
           87<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("La Vita Bella", 90<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bookstores =
        [ ("Big Ben Bookshop", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let hotels =
        [ ("Hotel Don Giovanni", 75<quality>, 80m<dd>, zone.Id)
          ("Hotel Corinthia", 98<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let radioStudios =
        [ ("Evropa 2", 90<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let recordingStudios =
        [ ("Vinohradský Zvuk",
           80<quality>,
           100m<dd>,
           (Character.from
               "Jana Novotná"
               Female
               (Shorthands.Autumn 5<days> 1982<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Zvukový Štěstí", 85<quality>, 100m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let metroStation =
        ("Náměstí Míru Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let hospital =
        ("Královské Vinohrady University Hospital", 75<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlace metroStation
        |> World.Street.addPlace hospital

    street, metroStation

let createZone city =
    let vinohradyZone = World.Zone.create "Vinohrady"

    let residentialStreet = residentialStreet city vinohradyZone
    let náměstíMíru, metroStation = náměstíMíru city vinohradyZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = náměstíMíru.Id
          PlaceId = metroStation.Id }

    vinohradyZone
    |> World.Zone.addStreet (
        World.Node.create residentialStreet.Id residentialStreet
    )
    |> World.Zone.addStreet (World.Node.create náměstíMíru.Id náměstíMíru)
    |> World.Zone.connectStreets residentialStreet.Id náměstíMíru.Id South
    |> World.Zone.addDescriptor Luxurious
    |> World.Zone.addMetroStation station
