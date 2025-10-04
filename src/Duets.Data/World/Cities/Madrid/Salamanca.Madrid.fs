module rec Duets.Data.World.Cities.Madrid.Salamanca

open Duets.Entities
open Duets.Data.World.Cities

let calleSerrano (city: City) (zone: Zone) =
    let street =
        World.Street.create "Calle de Serrano" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        One of Madrid's most prestigious shopping streets in the exclusive Salamanca district.
        The boulevard is lined with luxury boutiques, flagship stores of international designers,
        and elegant residential buildings with ornate facades from the late 19th and early
        20th centuries. Wide sidewalks accommodate well-dressed shoppers, and the street
        exudes sophistication with its polished marble entrances, designer window displays,
        and perfectly manicured trees. Art Nouveau and rationalist architectural details
        adorn many buildings, while luxury cars frequently park along the curbs.
"""

    let hotels =
        [ ("Hotel Wellington", 92<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Ten Con Ten", 90<quality>, French, zone.Id)
          ("El Paraguas", 88<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Gijón", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Teatro Nuevo Alcalá",
           1200,
           87<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("RockFM", 90<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let metroStation =
        ("Serrano Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let calleVelazquez city (zone: Zone) =
    let street =
        World.Street.create "Calle de Velázquez" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        A prominent street in the Salamanca neighborhood named after the famous
        Spanish painter Diego Velázquez. The street features neoclassical and modernist
        architecture with buildings showcasing wrought-iron balconies and decorative stonework.
        The area has an upscale yet relaxed atmosphere with wide pavements, designer
        storefronts, and contemporary galleries. Tall trees provide shade, and the
        well-maintained urban landscape reflects the district's affluent character.
"""

    let restaurants =
        [ ("Amazonico", 89<quality>, Japanese, zone.Id)
          ("Ramses", 87<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let gyms =
        [ ("Metropolitan Abascal", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concertSpaces =
        [ ("Teatro Marquina",
           500,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces concertSpaces

let calleGoya (zone: Zone) =
    let street =
        World.Street.create "Calle de Goya" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        A vibrant commercial street named after painter Francisco de Goya, serving
        as one of Salamanca's main shopping arteries. The street combines traditional
        Spanish architecture with modern retail spaces, featuring a mix of local businesses
        and international chains. Elegant apartment buildings with characteristic Madrid
        balconies line the upper floors, while street-level shops create a bustling
        pedestrian environment. The area attracts a diverse crowd of residents and
        visitors, with sidewalk cafés and historic pastry shops adding charm to the urban landscape.
"""

    let cafes =
        [ ("La Duquesita", 80<quality>, zone.Id)
          ("Pastelería Mallorca", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería Goya", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let createZone city =
    let salamancaZone = World.Zone.create "Salamanca"

    let calleSerrano, metroStation = calleSerrano city salamancaZone
    let calleVelazquez = calleVelazquez city salamancaZone
    let calleGoya = calleGoya salamancaZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = calleSerrano.Id
          PlaceId = metroStation.Id }

    salamancaZone
    |> World.Zone.addStreet (World.Node.create calleSerrano.Id calleSerrano)
    |> World.Zone.addStreet (World.Node.create calleVelazquez.Id calleVelazquez)
    |> World.Zone.addStreet (World.Node.create calleGoya.Id calleGoya)
    |> World.Zone.connectStreets calleSerrano.Id calleVelazquez.Id East
    |> World.Zone.connectStreets calleVelazquez.Id calleGoya.Id South
    |> World.Zone.addMetroStation station
