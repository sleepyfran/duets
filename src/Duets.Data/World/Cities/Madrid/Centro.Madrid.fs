module rec Duets.Data.World.Cities.Madrid.Centro

open Duets.Entities
open Duets.Entities.Calendar
open Duets.Data.World.Cities

let granVia (zone: Zone) =
    let street = World.Street.create "Gran Vía" (StreetType.Split(East, 3))

    let hotels =
        [ ("Hotel Emperador", 90<quality>, 320m<dd>, zone.Id)
          ("Hotel Vincci", 85<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Museo del Jamón", 80<quality>, Spanish, zone.Id)
          ("StreetXO", 92<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café de la Luz", 78<quality>, zone.Id)
          ("La Rollerie", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Wurlitzer Ballroom",
           120,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Teatro Kapital",
           500,
           85<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("WiZink Center",
           17000,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Gran Vía Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let puertaDelSol (zone: Zone) =
    let street =
        World.Street.create "Puerta del Sol" (StreetType.Split(South, 2))

    let restaurants =
        [ ("Casa Labra", 84<quality>, Spanish, zone.Id)
          ("La Mallorquina", 88<quality>, Spanish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("La Venencia", 80<quality>, zone.Id)
          ("El Tigre", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Librería San Ginés", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Teatro Real",
           1700,
           98<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Sala El Sol", 400, 86<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces

let barrioDeLasLetras (zone: Zone) =
    let street =
        World.Street.create "Barrio de las Letras" (StreetType.Split(West, 2))

    let cafes =
        [ ("Café Central", 86<quality>, zone.Id)
          ("La Fídula", 79<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let studios =
        [ ("Estudio Uno",
           88<quality>,
           260m<dd>,
           (Character.from
               "Paco de Lucía"
               Male
               (Shorthands.Spring 21<days> 1947<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Sala Clamores", 75<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces

let calleArenal (zone: Zone) =
    let street =
        World.Street.create "Calle del Arenal" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Joy Eslava", 800, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Costello Club",
           300,
           82<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let calleBarquillo (zone: Zone) =
    let street =
        World.Street.create "Calle del Barquillo" (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Teatro Lara", 400, 89<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Café Berlín", 400, 83<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let callePrincesa (zone: Zone) =
    let street =
        World.Street.create "Calle de la Princesa" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Teatro Barceló",
           900,
           92<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Café La Palma",
           300,
           80<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let calleAlcala (zone: Zone) =
    let street =
        World.Street.create "Calle de Alcalá" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Teatro de la Zarzuela",
           1300,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Teatro Nuevo Apolo",
           1200,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let centroZone = World.Zone.create "Centro"

    let granVia, metroStation = granVia centroZone
    let puertaDelSol = puertaDelSol centroZone
    let barrioDeLasLetras = barrioDeLasLetras centroZone
    let calleArenal = calleArenal centroZone
    let calleBarquillo = calleBarquillo centroZone
    let callePrincesa = callePrincesa centroZone
    let calleAlcala = calleAlcala centroZone

    let station =
        { Lines = [ Blue; Red ]
          LeavesToStreet = granVia.Id
          PlaceId = metroStation.Id }

    centroZone
    |> World.Zone.addStreet (World.Node.create granVia.Id granVia)
    |> World.Zone.addStreet (World.Node.create puertaDelSol.Id puertaDelSol)
    |> World.Zone.addStreet (
        World.Node.create barrioDeLasLetras.Id barrioDeLasLetras
    )
    |> World.Zone.addStreet (World.Node.create calleArenal.Id calleArenal)
    |> World.Zone.addStreet (World.Node.create calleBarquillo.Id calleBarquillo)
    |> World.Zone.addStreet (World.Node.create callePrincesa.Id callePrincesa)
    |> World.Zone.addStreet (World.Node.create calleAlcala.Id calleAlcala)
    |> World.Zone.connectStreets granVia.Id puertaDelSol.Id South
    |> World.Zone.connectStreets puertaDelSol.Id barrioDeLasLetras.Id West
    |> World.Zone.connectStreets barrioDeLasLetras.Id calleArenal.Id North
    |> World.Zone.connectStreets calleArenal.Id calleBarquillo.Id East
    |> World.Zone.connectStreets calleBarquillo.Id callePrincesa.Id South
    |> World.Zone.connectStreets callePrincesa.Id calleAlcala.Id East
    |> World.Zone.addMetroStation station
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addDescriptor EntertainmentHeart
