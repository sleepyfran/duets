module rec Duets.Data.World.Cities.Toronto.DowntownCore

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private kingStreetWest (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.kingStreetWest
            (StreetType.Split(East, 2))

    let concerts =
        [ ("Scotiabank Arena",
           19800,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Roy Thomson Hall",
           2630,
           94<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Four Seasons Centre",
           2000,
           93<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("TIFF Bell Lightbox", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema city.Id street.Id)

    let bars =
        [ ("The Fifth Social Club", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Union Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street, metroStation

let private bayStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bayStreet StreetType.OneWay

    let casinos =
        [ ("Great Canadian Casino", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Bay Street Premium Motors",
           94<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "James Thornton"
                   Male
                   (Shorthands.Autumn 15<days> 1976<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let hotels =
        [ ("Fairmont Royal York", 95<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Canoe", 94<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Balzac's Coffee", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let private frontStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.frontStreet StreetType.OneWay
        |> World.Street.tag "gooderham-building"

    let hotels =
        [ ("Delta Hotels Toronto", 80<quality>, 220m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let concerts =
        [ ("Meridian Hall",
           3000,
           90<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Keg", 82<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants

let createZone (city: City) =
    let downtownCoreZone = World.Zone.create Ids.Zone.downtownCore

    let kingStreetWest, unionMetro = kingStreetWest city downtownCoreZone
    let bayStreet = bayStreet downtownCoreZone
    let frontStreet = frontStreet downtownCoreZone

    let unionMetroStation =
        { Lines = [ Red; Blue ]
          LeavesToStreet = kingStreetWest.Id
          PlaceId = unionMetro.Id }

    downtownCoreZone
    |> World.Zone.addStreet (
        World.Node.create kingStreetWest.Id kingStreetWest
    )
    |> World.Zone.addStreet (World.Node.create bayStreet.Id bayStreet)
    |> World.Zone.addStreet (World.Node.create frontStreet.Id frontStreet)
    |> World.Zone.connectStreets kingStreetWest.Id bayStreet.Id North
    |> World.Zone.connectStreets kingStreetWest.Id frontStreet.Id South
    |> World.Zone.addMetroStation unionMetroStation
