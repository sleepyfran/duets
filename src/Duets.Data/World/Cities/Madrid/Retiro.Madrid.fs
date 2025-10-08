module rec Duets.Data.World.Cities.Madrid.Retiro

open Duets.Entities
open Duets.Data.World.Cities

let parqueRetiro (zone: Zone) =
    let street =
        World.Street.create "Parque del Retiro" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        A picturesque area bordering Madrid's most famous park, the Retiro. Tree-lined
        paths with chestnuts and plane trees provide shade over elegant wrought-iron benches.
        The street offers views of the park's ornamental gates and the beautiful Palacio
        de Cristal visible through the foliage. Street musicians often perform near the park entrances,
        and the air carries the scent of flowers from nearby gardens. Historic lampposts and
        cobblestone sections add to the romantic atmosphere.
"""

    let cafes =
        [ ("Florida Retiro", 86<quality>, zone.Id)
          ("Café del Palacio de Cristal", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería del Retiro", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Teatro Monumental",
           500,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("La Riviera", 2500, 90<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Sala But", 900, 88<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Sala Caracol",
           500,
           85<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Retiro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let home =
        World.Place.create "Home" 100<quality> Home Layouts.homeLayout zone.Id street.Id

    let street =
        street
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation
        |> World.Street.addPlace home

    street, metroStation

let doctorEsquerdo city (zone: Zone) =
    let street =
        World.Street.create "Doctor Esquerdo" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        A busy urban thoroughfare in southeastern Madrid named after a prominent
        19th-century physician. The street is characterized by wide sidewalks,
        modern apartment buildings from the 1960s-80s, and significant medical infrastructure.
        Tree-lined sections with London plane trees provide greenery, while the constant
        flow of ambulances and medical professionals reflects the area's healthcare focus.
        The architecture mixes functional modernist blocks with some Art Deco details.
"""

    let hospital =
        ("Hospital General Universitario Gregorio Marañón", 88<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    let gyms =
        [ ("Retiro Fitness", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let cafes =
        [ ("Café Esquerdo", 81<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlace hospital
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces cafes

let avenidaMenendezPelayo (zone: Zone) =
    let street =
        World.Street.create
            "Avenida de Menéndez Pelayo"
            (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        A grand avenue running parallel to the Retiro Park's eastern edge, named
        after the Spanish scholar. The street features wide lanes separated by a
        central median with gardens and fountains. Elegant early 20th-century buildings
        with balconies overlook the tree-lined avenue. The proximity to the park brings
        joggers and cyclists, while outdoor terrace seating creates a lively social atmosphere.
        Ornamental streetlights and well-maintained green spaces contribute to the
        area's upscale character.
"""

    let restaurants =
        [ ("Taberna La Dolores", 84<quality>, Spanish, zone.Id)
          ("La Castela", 87<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("Bar Menéndez", 79<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street |> World.Street.addPlaces restaurants |> World.Street.addPlaces bars

let createZone city =
    let retiroZone = World.Zone.create "Retiro"

    let parqueRetiro, metroStation = parqueRetiro retiroZone
    let doctorEsquerdo = doctorEsquerdo city retiroZone
    let avenidaMenendezPelayo = avenidaMenendezPelayo retiroZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = parqueRetiro.Id
          PlaceId = metroStation.Id }

    retiroZone
    |> World.Zone.addStreet (World.Node.create parqueRetiro.Id parqueRetiro)
    |> World.Zone.addStreet (World.Node.create doctorEsquerdo.Id doctorEsquerdo)
    |> World.Zone.addStreet (
        World.Node.create avenidaMenendezPelayo.Id avenidaMenendezPelayo
    )
    |> World.Zone.connectStreets parqueRetiro.Id doctorEsquerdo.Id South
    |> World.Zone.connectStreets
        doctorEsquerdo.Id
        avenidaMenendezPelayo.Id
        North
    |> World.Zone.addMetroStation station
