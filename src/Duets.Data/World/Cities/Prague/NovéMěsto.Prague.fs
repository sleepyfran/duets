module rec Duets.Data.World.Cities.Prague.NovéMěsto

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let václavskéNáměstí city (zone: Zone) =
    let street =
        World.Street.create "Václavské náměstí" (StreetType.Split(North, 3))

    let concertSpaces =
        [ ("Lucerna Music Bar",
           800,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Rock Café", 350, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("Hotel Majestic Plaza", 80<quality>, 90m<dd>, zone.Id)
          ("Hotel Kings Court", 92<quality>, 130m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Sushi Maki", 87<quality>, Japanese, zone.Id)
          ("Pho Bo", 86<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bookstores =
        [ ("Palác Knih Luxor", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Java Palace", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Můstek Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let naPříkopě city (zone: Zone) =
    let street = World.Street.create "Na Příkopě" StreetType.OneWay

    let gyms =
        [ ("PowerGym Hlavní nádraží", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let casinos =
        [ ("High Stakes Haven", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let recordingStudios =
        [ ("Pražské Záznamy",
           90<quality>,
           300m<dd>,
           (Character.from
               "Eva Svobodová"
               Female
               (Shorthands.Spring 15<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let radioStudios =
        [ ("Český rozhlas Jazz", 85<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let bars =
        [ ("Bistro Prostřeno", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("Pokoje Prostor", 90<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("Café v lese", 250, 80<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Klub 007 Strahov",
           250,
           83<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Storm Club", 500, 84<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Palác Akropolis",
           500,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let street =
        street
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces concertSpaces

    street

let createZone city =
    let novéMěstoZone = World.Zone.create "Nové Město"

    let václavskéNáměstí, metroStation = václavskéNáměstí city novéMěstoZone
    let naPříkopě = naPříkopě city novéMěstoZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = václavskéNáměstí.Id
          PlaceId = metroStation.Id }

    novéMěstoZone
    |> World.Zone.addStreet (
        World.Node.create václavskéNáměstí.Id václavskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create naPříkopě.Id naPříkopě)
    |> World.Zone.connectStreets václavskéNáměstí.Id naPříkopě.Id West
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation station
