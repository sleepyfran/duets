module Duets.Data.World.Cities.LosAngeles.Hollywood

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let hollywoodBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.hollywoodBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Fonda Theatre",
           1200,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Dolby Theatre",
           3400,
           92<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Musso & Frank Grill", 90<quality>, zone.Id)
          ("The Frolic Room", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Larry Edmunds Bookshop", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Rosy Café", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let hotels =
        [ ("The Hollywood Roosevelt", 92<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let merchandiseWorkshops =
        [ ("Amoeba Music", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let restaurants =
        [ ("Hard Rock Cafe", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces restaurants

let highlandAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.highlandAvenue
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Hollywood Bowl",
           17500,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Petit Trois L'Original", 89<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Hollywood/Highland Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let sunsetBoulevardHollywood (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.sunsetBoulevardHollywood
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Hollywood Palladium",
           3700,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("The Waffle", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let hotels =
        [ ("Chateau Marmont", 95<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let rehearsalSpaces =
        [ ("SIR Rehearsal Studios", 88<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("Gwen", 92<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let studios =
        [ ("EastWest Studios",
           94<quality>,
           800m<dd>,
           (Character.from
               "Rick Rubin"
               Male
               (Shorthands.Spring 21<days> 1963<years>)),
           zone.Id)
          ("Sunset Sound",
           92<quality>,
           750m<dd>,
           (Character.from
               "Fabrice Dupont"
               Male
               (Shorthands.Summer 15<days> 1970<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces studios

let vineStreet (zone: Zone) city =
    let street =
        World.Street.create Ids.Street.vineStreet (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Avalon Hollywood",
           1500,
           87<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("W Hollywood", 88<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let gyms =
        [ ("Equinox Hollywood", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let studios =
        [ ("Capitol Studios",
           96<quality>,
           900m<dd>,
           (Character.from
               "Steve Albini"
               Male
               (Shorthands.Summer 31<days> 1962<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces studios

let cahuengaBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.cahuengaBoulevard
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Hotel Cafe", 215, 85<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let santaMonicaBoulevardHollywood (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.santaMonicaBoulevardHollywood
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Hollywood Forever Cemetery",
           3000,
           86<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("LA Rehearsal", 85<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces

let nCherokeeAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.nCherokeeAvenue StreetType.OneWay

    let bars =
        [ ("Boardner's by La Belle", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street |> World.Street.addPlaces bars

let nLaBreaAvenue (zone: Zone) =
    let street = World.Street.create Ids.Street.nLaBreaAvenue StreetType.OneWay

    let bars =
        [ ("The Woods", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street |> World.Street.addPlaces bars

let franklinAvenue (zone: Zone) =
    let street = World.Street.create Ids.Street.franklinAvenue StreetType.OneWay

    let bookstores =
        [ ("Counterpoint Records & Books", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let hotels =
        [ ("Magic Castle Hotel", 86<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street |> World.Street.addPlaces bookstores |> World.Street.addPlaces hotels

let beachwoodDrive (zone: Zone) =
    let street = World.Street.create Ids.Street.beachwoodDrive StreetType.OneWay

    let cafes =
        [ ("Beachwood Cafe", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street |> World.Street.addPlaces cafes

let hollowayDrive (zone: Zone) =
    let street = World.Street.create Ids.Street.hollowayDrive StreetType.OneWay

    let cafes =
        [ ("Dialog Cafe", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street |> World.Street.addPlaces cafes

let nColeAvenue (zone: Zone) city =
    let street = World.Street.create Ids.Street.nColeAvenue StreetType.OneWay

    let gyms =
        [ ("Gold's Gym", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street |> World.Street.addPlaces gyms

let deLongpreAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.deLongpreAvenue StreetType.OneWay

    let hospitals =
        [ ("Southern California Hospital at Hollywood", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    street |> World.Street.addPlaces hospitals

let wAlamedaAvenue (zone: Zone) city =
    let street = World.Street.create Ids.Street.wAlamedaAvenue StreetType.OneWay

    let radioStudios =
        [ ("iHeartRadio Theater", 91<quality>, "Alternative", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    street |> World.Street.addPlaces radioStudios

let sewardStreet (zone: Zone) =
    let street = World.Street.create Ids.Street.sewardStreet StreetType.OneWay

    let restaurants =
        [ ("Rao's", 90<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street |> World.Street.addPlaces restaurants

let nSycamoreAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.nSycamoreAvenue StreetType.OneWay

    let studios =
        [ ("Record Plant",
           93<quality>,
           850m<dd>,
           (Character.from
               "Chris Lord-Alge"
               Male
               (Shorthands.Winter 9<days> 1956<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street |> World.Street.addPlaces studios

let createZone (city: City) =
    let hollywoodZone = World.Zone.create Ids.Zone.hollywood

    let hollywoodBoulevard = hollywoodBoulevard hollywoodZone
    let highlandAvenue, highlandAvenueStation = highlandAvenue hollywoodZone
    let sunsetBoulevard = sunsetBoulevardHollywood hollywoodZone
    let vineStreet = vineStreet hollywoodZone city
    let cahuengaBoulevard = cahuengaBoulevard hollywoodZone
    let santaMonicaBoulevard = santaMonicaBoulevardHollywood hollywoodZone
    let nCherokeeAvenue = nCherokeeAvenue hollywoodZone
    let nLaBreaAvenue = nLaBreaAvenue hollywoodZone
    let franklinAvenue = franklinAvenue hollywoodZone
    let beachwoodDrive = beachwoodDrive hollywoodZone
    let hollowayDrive = hollowayDrive hollywoodZone
    let nColeAvenue = nColeAvenue hollywoodZone city
    let deLongpreAvenue = deLongpreAvenue hollywoodZone
    let wAlamedaAvenue = wAlamedaAvenue hollywoodZone city
    let sewardStreet = sewardStreet hollywoodZone
    let nSycamoreAvenue = nSycamoreAvenue hollywoodZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = highlandAvenue.Id
          PlaceId = highlandAvenueStation.Id }

    hollywoodZone
    |> World.Zone.addStreet (
        World.Node.create hollywoodBoulevard.Id hollywoodBoulevard
    )
    |> World.Zone.addStreet (World.Node.create highlandAvenue.Id highlandAvenue)
    |> World.Zone.addStreet (
        World.Node.create sunsetBoulevard.Id sunsetBoulevard
    )
    |> World.Zone.addStreet (World.Node.create vineStreet.Id vineStreet)
    |> World.Zone.addStreet (
        World.Node.create cahuengaBoulevard.Id cahuengaBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create santaMonicaBoulevard.Id santaMonicaBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create nCherokeeAvenue.Id nCherokeeAvenue
    )
    |> World.Zone.addStreet (World.Node.create nLaBreaAvenue.Id nLaBreaAvenue)
    |> World.Zone.addStreet (World.Node.create franklinAvenue.Id franklinAvenue)
    |> World.Zone.addStreet (World.Node.create beachwoodDrive.Id beachwoodDrive)
    |> World.Zone.addStreet (World.Node.create hollowayDrive.Id hollowayDrive)
    |> World.Zone.addStreet (World.Node.create nColeAvenue.Id nColeAvenue)
    |> World.Zone.addStreet (
        World.Node.create deLongpreAvenue.Id deLongpreAvenue
    )
    |> World.Zone.addStreet (World.Node.create wAlamedaAvenue.Id wAlamedaAvenue)
    |> World.Zone.addStreet (World.Node.create sewardStreet.Id sewardStreet)
    |> World.Zone.addStreet (
        World.Node.create nSycamoreAvenue.Id nSycamoreAvenue
    )
    |> World.Zone.connectStreets cahuengaBoulevard.Id hollywoodBoulevard.Id East
    |> World.Zone.connectStreets hollywoodBoulevard.Id vineStreet.Id East
    |> World.Zone.connectStreets vineStreet.Id highlandAvenue.Id East
    |> World.Zone.connectStreets sunsetBoulevard.Id vineStreet.Id North
    |> World.Zone.connectStreets vineStreet.Id santaMonicaBoulevard.Id North
    |> World.Zone.connectStreets nLaBreaAvenue.Id hollywoodBoulevard.Id North
    |> World.Zone.connectStreets hollywoodBoulevard.Id franklinAvenue.Id North
    |> World.Zone.connectStreets franklinAvenue.Id beachwoodDrive.Id North
    |> World.Zone.connectStreets nCherokeeAvenue.Id hollywoodBoulevard.Id North
    |> World.Zone.connectStreets hollywoodBoulevard.Id deLongpreAvenue.Id North
    |> World.Zone.connectStreets vineStreet.Id nColeAvenue.Id North
    |> World.Zone.connectStreets highlandAvenue.Id hollowayDrive.Id North
    |> World.Zone.connectStreets sunsetBoulevard.Id sewardStreet.Id North
    |> World.Zone.connectStreets sunsetBoulevard.Id nSycamoreAvenue.Id North
    |> World.Zone.connectStreets franklinAvenue.Id wAlamedaAvenue.Id North
    |> World.Zone.addMetroStation station
