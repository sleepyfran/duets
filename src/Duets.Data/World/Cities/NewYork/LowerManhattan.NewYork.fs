module rec Duets.Data.World.Cities.NewYork.LowerManhattan

open Duets.Data.World.Cities
open Duets.Entities

let private bleeckerStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.bleeckerStreet
            (StreetType.Split(East, 4))

    let concerts =
        [ ("The Bitter End",
           400,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("(Le) Poisson Rouge",
           700,
           89<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Lynn Redgrave Theater",
           350,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("John's of Bleecker Street", 88<quality>, Italian, zone.Id)
          ("Kesté Pizza & Vino", 86<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants

let private bowery (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bowery (StreetType.Split(North, 4))

    let concerts =
        [ ("Bowery Ballroom",
           575,
           90<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    // TODO: Add New Museum once we support art museums

    let bars =
        [ ("Capitale", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("John Varvatos Store", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("The Bowery Hotel", 92<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces shops
    |> World.Street.addPlaces hotels

let private irvingPlace (zone: Zone) =
    let street =
        World.Street.create Ids.Street.irvingPlace (StreetType.Split(North, 3))

    let concerts =
        [ ("Irving Plaza",
           1025,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Pete's Tavern", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("The W New York – Union Square", 90<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let hospitals =
        [ ("NewYork-Presbyterian Lower Manhattan Hospital", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    // TODO: Add Union Square Park once we support parks

    let metroStation =
        ("Union Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlace metroStation

    street, metroStation

let private lowerEastSide (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.lowerEastSide (StreetType.Split(East, 3))

    let concerts =
        [ ("Mercury Lounge",
           250,
           88<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("The Red Room at KGB Bar",
           80,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("WBAI (Pacifica)", 86<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces radioStudios

let createZone (city: City) =
    let lowerManhattanZone = World.Zone.create Ids.Zone.lowerManhattan

    let bleeckerStreet = bleeckerStreet lowerManhattanZone
    let bowery = bowery lowerManhattanZone
    let irvingPlace, unionSquareMetro = irvingPlace lowerManhattanZone
    let lowerEastSide = lowerEastSide city lowerManhattanZone

    let unionSquareMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = irvingPlace.Id
          PlaceId = unionSquareMetro.Id }

    lowerManhattanZone
    |> World.Zone.addStreet (World.Node.create bleeckerStreet.Id bleeckerStreet)
    |> World.Zone.addStreet (World.Node.create bowery.Id bowery)
    |> World.Zone.addStreet (World.Node.create irvingPlace.Id irvingPlace)
    |> World.Zone.addStreet (World.Node.create lowerEastSide.Id lowerEastSide)
    |> World.Zone.connectStreets bleeckerStreet.Id bowery.Id North
    |> World.Zone.connectStreets bowery.Id irvingPlace.Id North
    |> World.Zone.connectStreets bowery.Id lowerEastSide.Id East
    |> World.Zone.addMetroStation unionSquareMetroStation
