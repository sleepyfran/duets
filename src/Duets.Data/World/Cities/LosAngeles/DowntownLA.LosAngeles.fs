module rec Duets.Data.World.Cities.LosAngeles.DowntownLA

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let financialCorridor city (zone: Zone) =
    let street =
        World.Street.create "Financial Corridor" (StreetType.Split(North, 3))

    let hotels =
        [ ("The Biltmore Hotel", 88<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Grand Central Market", 85<quality>, American, zone.Id)
          ("Perch", 90<quality>, Japanese, zone.Id)
          ("Patina", 92<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let gyms =
        [ ("Crunch Fitness", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let recordingStudios =
        [ ("United Recording",
           85<quality>,
           280m<dd>,
           (Character.from
               "David Foster"
               Male
               (Shorthands.Autumn 1<days> 1949<years>)),
           zone.Id)
          ("Capitol Studios",
           90<quality>,
           300m<dd>,
           (Character.from
               "Al Schmitt"
               Male
               (Shorthands.Spring 17<days> 1930<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let cafes =
        [ ("Starbucks", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Downtown LA Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let grandStreet (zone: Zone) =
    let street = World.Street.create "Grand Street" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Novo", 2300, 88<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Broken Spanish", 89<quality>, Mexican, zone.Id)
          ("Bestia", 91<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let casinos =
        [ ("The Bicycle Casino", 80<quality>, zone.Id)
          ("Hollywood Park Casino", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let recordingStudios =
        [ ("EastWest Studios",
           92<quality>,
           350m<dd>,
           (Character.from
               "Greg Kurstin"
               Male
               (Shorthands.Spring 14<days> 1969<years>)),
           zone.Id)
          ("Village Studios",
           95<quality>,
           380m<dd>,
           (Character.from
               "Rick Rubin"
               Male
               (Shorthands.Spring 10<days> 1963<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let hospital =
        ("Good Samaritan Hospital", 80<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlace hospital

let backstreets (zone: Zone) =
    let street = World.Street.create "Backstreets" (StreetType.Split(West, 2))

    let recordingStudios =
        [ ("Conway Recording Studios",
           83<quality>,
           290m<dd>,
           (Character.from
               "Bruce Swedien"
               Male
               (Shorthands.Spring 19<days> 1934<years>)),
           zone.Id)
          ("Paramount Recording Studio",
           79<quality>,
           250m<dd>,
           (Character.from
               "Rodney Jenkins"
               Male
               (Shorthands.Autumn 24<days> 1969<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let bars =
        [ ("The Redwood Bar", 72<quality>, zone.Id)
          ("Seven Grand", 76<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("The Beat Lab", 62<quality>, 140m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let bookstores =
        [ ("The Last Bookstore", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Groundwork Coffee", 74<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes

let createZone city =
    let downtownLAZone = World.Zone.create "Downtown LA"

    let financialCorridor, metroStation = financialCorridor city downtownLAZone
    let grandStreet = grandStreet downtownLAZone
    let backstreets = backstreets downtownLAZone

    let station =
        { Lines = [ Blue; Red ]
          LeavesToStreet = financialCorridor.Id
          PlaceId = metroStation.Id }

    downtownLAZone
    |> World.Zone.addStreet (
        World.Node.create financialCorridor.Id financialCorridor
    )
    |> World.Zone.addStreet (World.Node.create grandStreet.Id grandStreet)
    |> World.Zone.addStreet (World.Node.create backstreets.Id backstreets)
    |> World.Zone.connectStreets financialCorridor.Id grandStreet.Id East
    |> World.Zone.connectStreets grandStreet.Id backstreets.Id North
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation station
