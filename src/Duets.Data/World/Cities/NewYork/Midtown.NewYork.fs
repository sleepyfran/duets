module rec Duets.Data.World.Cities.NewYork.Midtown

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private timesSquare (zone: Zone) =
    let street = World.Street.create "Times Square" (StreetType.Split(North, 4))

    let casinos =
        [ ("Big Apple Casino", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let concertSpaces =
        [ ("Madison Square Garden",
           20000,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Radio City Music Hall",
           6000,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("The Joyce Theater",
           472,
           95<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The New Victory Theater",
           499,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hospital =
        ("Mount Sinai West Hospital", 85<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    let metroStation =
        ("Times Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace hospital
        |> World.Street.addPlace metroStation

    street, metroStation

let private fifthAvenue city (zone: Zone) =
    let street = World.Street.create "5th Avenue" (StreetType.Split(North, 3))

    let gyms =
        [ ("24 Hour Fitness Midtown", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let hotels =
        [ ("The Plaza", 98<quality>, 240m<dd>, zone.Id)
          ("The Peninsula New York", 92<quality>, 210m<dd>, zone.Id)
          ("The Roosevelt Hotel", 70<quality>, 120m<dd>, zone.Id)
          ("The New Yorker", 65<quality>, 100m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let merchandiseWorkshops =
        [ ("NY Merch", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let restaurants =
        [ ("Le Bernardin", 91<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces restaurants

let private broadway (zone: Zone) =
    let street = World.Street.create "Broadway" (StreetType.Split(North, 4))

    let concertSpaces =
        [ ("Carnegie Hall",
           2804,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Terminal 5", 3000, 82<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("The Town Hall",
           1500,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Hammerstein Ballroom",
           2200,
           86<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Midtown Melodies", 89<quality>, 140m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("Midtown Studios",
           89<quality>,
           280m<dd>,
           (Character.from
               "Martin Thompson"
               Male
               (Shorthands.Winter 30<days> 1976<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces studios

let createZone (city: City) =
    let midtownZone = World.Zone.create "Midtown"

    let timesSquare, metroStation = timesSquare midtownZone
    let fifthAvenue = fifthAvenue city midtownZone
    let broadway = broadway midtownZone

    let radioStudios =
        [ ("Z100", 94<quality>, "Pop", midtownZone.Id)
          ("Q104.3", 92<quality>, "Rock", midtownZone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let fifthAvenue = fifthAvenue |> World.Street.addPlaces radioStudios

    let metroStation =
        { Lines = [ Blue; Red ]
          LeavesToStreet = timesSquare.Id
          PlaceId = metroStation.Id }

    midtownZone
    |> World.Zone.addStreet (World.Node.create timesSquare.Id timesSquare)
    |> World.Zone.addStreet (World.Node.create fifthAvenue.Id fifthAvenue)
    |> World.Zone.addStreet (World.Node.create broadway.Id broadway)
    |> World.Zone.connectStreets timesSquare.Id fifthAvenue.Id East
    |> World.Zone.connectStreets fifthAvenue.Id broadway.Id East
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation metroStation
