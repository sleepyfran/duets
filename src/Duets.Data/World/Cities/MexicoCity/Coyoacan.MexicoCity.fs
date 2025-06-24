module rec Duets.Data.World.Cities.MexicoCity.Coyoacan

open Duets.Data.World.Cities
open Duets.Entities

let private jardinCentenario (zone: Zone) =
    let street =
        World.Street.create "Jardín Centenario" (StreetType.Split(South, 2))

    let cafes =
        [ ("Café El Jarocho", 80<quality>, zone.Id)
          ("La Coyoacana", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería Rosario Castellanos", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Coyoacán Station", zone.Id) |> PlaceCreators.createMetro street.Id

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlace metroStation,
    metroStation

let private franciscoSosa (zone: Zone) =
    let street =
        World.Street.create "Francisco Sosa" (StreetType.Split(East, 2))

    let rehearsalSpaces =
        [ ("Casa de Cultura Jesús Reyes Heroles", 75<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("Foro Cultural Coyoacán",
           400,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("El Hijo del Cuervo",
           120,
           78<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces

let private calleCarrilloPuerto (zone: Zone) =
    let street =
        World.Street.create "Calle Carrillo Puerto" (StreetType.Split(West, 2))

    let home = PlaceCreators.createHome street.Id zone.Id

    let cafes =
        [ ("Café Ruta de la Seda", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería El Sótano", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Centro Cultural El Hormiguero",
           60,
           75<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces

let zone =
    let zone = World.Zone.create "Coyoacán"
    let jardinCentenario, metroStation = jardinCentenario zone
    let franciscoSosa = franciscoSosa zone
    let calleCarrilloPuerto = calleCarrilloPuerto zone

    let zoneMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = jardinCentenario.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create jardinCentenario.Id jardinCentenario
    )
    |> World.Zone.addStreet (World.Node.create franciscoSosa.Id franciscoSosa)
    |> World.Zone.addStreet (
        World.Node.create calleCarrilloPuerto.Id calleCarrilloPuerto
    )
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addDescriptor Bohemian
    |> World.Zone.addMetroStation zoneMetroStation
