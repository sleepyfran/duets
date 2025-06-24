module rec Duets.Data.World.Cities.Tokyo.Shinjuku

open Duets.Data.World.Cities
open Duets.Entities

let private kabukicho (zone: Zone) =
    let street = World.Street.create "Kabukicho" (StreetType.Split(North, 3))

    let bars =
        [ ("Golden Gai", 90<quality>, zone.Id)
          ("Bar Albatross", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let clubs =
        [ ("Robot Restaurant", 88<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let rehearsalSpaces =
        [ ("Shinjuku Jam", 75<quality>, 160m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("Shinjuku MARZ",
           250,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Shinjuku Loft",
           500,
           83<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Shinjuku Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let home =
        World.Place.create "Home" 100<quality> Home Layouts.homeLayout zone.Id

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces clubs
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation
        |> World.Street.addPlace home

    street, metroStation

let private skyscraperDistrict (city: City) (zone: Zone) =
    let street =
        World.Street.create "Skyscraper District" (StreetType.Split(West, 2))

    let hotels =
        [ ("Park Hyatt Tokyo", 95<quality>, 600m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let gyms =
        [ ("Gold's Gym Shinjuku", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let hospital =
        ("Tokyo Medical Center", 85<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    street
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces gyms
    |> World.Street.addPlace hospital

let createZone (city: City) =
    let shinjukuZone = World.Zone.create "Shinjuku"

    let kabukicho, metroStation = kabukicho shinjukuZone
    let skyscraperDistrict = skyscraperDistrict city shinjukuZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = kabukicho.Id
          PlaceId = metroStation.Id }

    shinjukuZone
    |> World.Zone.addStreet (World.Node.create kabukicho.Id kabukicho)
    |> World.Zone.addStreet (
        World.Node.create skyscraperDistrict.Id skyscraperDistrict
    )
    |> World.Zone.connectStreets kabukicho.Id skyscraperDistrict.Id South
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addMetroStation station
