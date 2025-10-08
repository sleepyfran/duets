module rec Duets.Data.World.Cities.Prague.Libeň

open Duets.Data.World.Cities
open Duets.Entities

let ocelářská (zone: Zone) =
    let street =
        World.Street.create "Ocelářská" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """Ocelářská street is dominated by the modern 
        O2 Arena complex, one of Central Europe's largest entertainment venues with
        its distinctive dome visible across the district. The street features contemporary
        commercial architecture, including the smaller O2 Universum hall and various
        retail spaces. The area is characterized by wide pedestrian zones and modern
        urban planning, with integrated public art installations. During major events,
        the street transforms into a bustling hub with crowds flowing to and from concerts
        and sporting events. The nearby Českomoravská metro station provides direct
        access to this entertainment district.
"""

    let concertSpaces =
        [ ("O2 Arena", 18000, 95<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("O2 Universum",
           4500,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Pivovar U Korunky", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Restaurace U Korunky", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Českomoravská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let voctářova (zone: Zone) =
    let street =
        World.Street.create "Voctářova" (StreetType.Split(West, 1))
        |> World.Street.attachContext
            """
        Voctářova is a quieter residential and local commercial street in
        Libeň, maintaining a more traditional Prague neighborhood character. The street
        features early 20th-century apartment buildings with period facades and small
        local businesses. Traditional Czech pubs like U Hubatků serve as community
        gathering points, preserving the authentic local atmosphere. Tree-lined sidewalks
        and smaller scale development create a neighborhood feel distinct from the
        modern entertainment district nearby. The street reflects Libeň's working-class
        roots while slowly gentrifying.
"""

    let bars =
        [ ("U Hubatků", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Restaurace U Hubatků", 81<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street |> World.Street.addPlaces bars |> World.Street.addPlaces restaurants

let zone =
    let libeňZone = World.Zone.create Ids.Zone.libeň

    let ocelářská, metroStation = ocelářská libeňZone
    let voctářova = voctářova libeňZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = ocelářská.Id
          PlaceId = metroStation.Id }

    libeňZone
    |> World.Zone.addStreet (World.Node.create ocelářská.Id ocelářská)
    |> World.Zone.addStreet (World.Node.create voctářova.Id voctářova)
    |> World.Zone.connectStreets ocelářská.Id voctářova.Id South
    |> World.Zone.addMetroStation station
