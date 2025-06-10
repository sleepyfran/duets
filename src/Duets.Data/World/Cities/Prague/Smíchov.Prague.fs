module rec Duets.Data.World.Cities.Prague.Smíchov

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let anděl city (zone: Zone) =
    let street = World.Street.create "Anděl" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Futurum Music Bar",
           650,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Jazz Dock", 150, 95<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Underdogs' Ballroom & Bar",
           200,
           82<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("MeetFactory", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let gyms =
        [ ("Athletic Club Anděl", 85<quality>, zone.Id)
          ("IronWorks Smíchov", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bookstores =
        [ ("Neoluxor", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Mug Harmony", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Anděl Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let nádražní (zone: Zone) =
    let street = World.Street.create "Nádražní" StreetType.OneWay

    let restaurants =
        [ ("El Sabor Mexicano", 89<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let casinos =
        [ ("Royal Flush Resort", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let recordingStudios =
        [ ("Anděl Studio",
           85<quality>,
           200m<dd>,
           (Character.from
               "Jan Novák"
               Male
               (Shorthands.Winter 24<days> 1975<years>)),
           zone.Id)
          ("Smíchovský Zvuk",
           85<quality>,
           200m<dd>,
           (Character.from
               "Alena Horáková"
               Female
               (Shorthands.Summer 15<days> 1977<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Nahrávací Studio Anděl", 92<quality>, 170m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces rehearsalSpaces

let createZone city =
    let smíchovZone = World.Zone.create "Smíchov"

    let anděl, metroStation = anděl city smíchovZone
    let nádražní = nádražní smíchovZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = anděl.Id
          PlaceId = metroStation.Id }

    smíchovZone
    |> World.Zone.addStreet (World.Node.create anděl.Id anděl)
    |> World.Zone.addStreet (World.Node.create nádražní.Id nádražní)
    |> World.Zone.connectStreets anděl.Id nádražní.Id North
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation station
