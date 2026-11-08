module rec Duets.Data.World.Cities.Prague.NovéMěsto

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let václavskéNáměstí (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.václavskéNáměstí
            (StreetType.Split(North, 3))
        |> World.Street.tag "wenceslas-square"

    let carDealers =
        [ ("Praha Premium Motors",
           93<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Viktor Černý"
                   Male
                   (Shorthands.Winter 9<days> 1977<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let casinos =
        [ ("Casino Admiral", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let bars =
        [ ("The Alchemist Bar", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

    let restaurants =
        [ ("Výtopna Railway Restaurant", 88<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hospitals =
        [ ("General University Hospital", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let radioStudios =
        [ ("Radio 1", 92<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("Můstek Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let vodičkova (zone: Zone) =
    let street =
        World.Street.create "Vodičkova" StreetType.OneWay

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

    let merchandiseWorkshops =
        [ ("Tiskárna Praha", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let concertSpaces =
        [ ("National Theatre",
           1000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "seated"
          ("Rock Café", 350, 88<quality>, Layouts.concertSpaceLayout2, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id ]

    let bars =
        [ ("Bukowski's Bar", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Café Louvre", 90<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Perla", 85<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cafes =
        [ ("The Miners", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces cafes

let createZone city =
    let novéMěstoZone = World.Zone.create Ids.Zone.novéMěsto

    let václavskéNáměstí, metroStation = václavskéNáměstí city novéMěstoZone
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
