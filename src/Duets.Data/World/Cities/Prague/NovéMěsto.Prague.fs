module rec Duets.Data.World.Cities.Prague.NovéMěsto

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let václavskéNáměstí (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.václavskéNáměstí
            (StreetType.Split(North, 3))
        |> World.Street.attachContext
            """
        Wenceslas Square is actually a broad boulevard rather than a traditional square,
        serving as Prague's commercial and cultural heart. The upper end features
        the monumental National Museum building and the iconic equestrian statue of Saint Wenceslas.
        The boulevard is lined with hotels, restaurants, shops, and Art Nouveau
        buildings including the famous Hotel Evropa. Historic significance marks every corner
        this was the site of major political demonstrations including the 1989 Velvet Revolution.
        The atmosphere is cosmopolitan and bustling, with street vendors, tourists,
        and locals mixing in equal measure. At night, neon signs illuminate the wide promenade.
"""

    let bars =
        [ ("The Alchemist Bar", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Výtopna Railway Restaurant", 88<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hospitals =
        [ ("General University Hospital", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let metroStation =
        ("Můstek Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlace metroStation

    street, metroStation

let vodičkova (zone: Zone) =
    let street =
        World.Street.create "Vodičkova" StreetType.OneWay
        |> World.Street.attachContext
            """
        Vodičkova is a busy commercial street connecting Wenceslas Square to the
        Vltava River area, characterized by its shopping arcades and historic passages.
        The famous Lucerna Palace complex, built by Václav Havel's grandfather,
        dominates the street with its Art Nouveau architecture and the iconic upside-down horse statue.
        Pedestrian traffic is constant, with shoppers moving between department stores and boutiques.
        The street features a mix of architectural styles from different eras, with ornate
        early 20th-century facades alongside more utilitarian communist-era buildings.
        Underground passages connect to the metro and various shopping arcades.
"""

    let concertSpaces =
        [ ("Lucerna Music Bar",
           800,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let národní (zone: Zone) =
    let street =
        World.Street.create Ids.Street.národní StreetType.OneWay
        |> World.Street.attachContext
            """
        Národní třída (National Avenue) runs from Wenceslas Square to the Vltava River,
        passing through the heart of cultural Prague. The street is anchored by the
        majestic National Theatre with its golden roof, a symbol of Czech national revival.
        Along the route, numerous cultural institutions, cafes, and historic buildings
        line the wide sidewalks. The street witnessed pivotal moments in Czech history,
        particularly during the 1989 demonstrations. Art Nouveau and neo-Renaissance
        architecture predominates, with the famous Café Louvre maintaining its historic
        interiors. The atmosphere is intellectual and cultural, with theater-goers,
        students, and artists frequenting the area.
"""

    let concertSpaces =
        [ ("National Theatre",
           1000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Rock Café", 350, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bukowski's Bar", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Café Louvre", 90<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Perla", 85<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels

let zone =
    let novéMěstoZone = World.Zone.create Ids.Zone.novéMěsto

    let václavskéNáměstí, metroStation = václavskéNáměstí novéMěstoZone
    let národní = národní novéMěstoZone
    let vodičkova = vodičkova novéMěstoZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = václavskéNáměstí.Id
          PlaceId = metroStation.Id }

    novéMěstoZone
    |> World.Zone.addStreet (
        World.Node.create václavskéNáměstí.Id václavskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create národní.Id národní)
    |> World.Zone.addStreet (World.Node.create vodičkova.Id vodičkova)
    |> World.Zone.connectStreets václavskéNáměstí.Id národní.Id West
    |> World.Zone.connectStreets václavskéNáměstí.Id vodičkova.Id South

    |> World.Zone.addMetroStation station
