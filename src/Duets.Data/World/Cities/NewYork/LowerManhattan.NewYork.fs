module rec Duets.Data.World.Cities.NewYork.LowerManhattan

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private bleeckerStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.bleeckerStreet
            (StreetType.Split(East, 3))

    let bookstores =
        [ ("McNally Jackson Books", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concerts =
        [ ("The Bitter End",
           400,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("(Le) Poisson Rouge",
           700,
           89<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("The Lynn Redgrave Theater",
           350,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "seated" ]

    let restaurants =
        [ ("John's of Bleecker Street", 88<quality>, Italian, zone.Id)
          ("Kesté Pizza & Vino", 86<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Joe Coffee", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let private bowery (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bowery (StreetType.Split(North, 3))

    let bookstores =
        [ ("Strand Bookstore", 94<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

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

    let cafes =
        [ ("Café Integral", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces shops
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces cafes

let private irvingPlace city (zone: Zone) =
    let street =
        World.Street.create Ids.Street.irvingPlace (StreetType.Split(North, 3))

    let recordingStudios =
        [ ("Electric Lady Studios",
           96<quality>,
           350m<dd>,
           (Character.from
               "Steve Rosenthal"
               Male
               (Shorthands.Winter 15<days> 1965<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let concerts =
        [ ("Irving Plaza",
           1025,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Pete's Tavern", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "pub")

    let hotels =
        [ ("The W New York – Union Square", 90<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let hospitals =
        [ ("NewYork-Presbyterian Lower Manhattan Hospital", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let cafes =
        [ ("Irving Farm Coffee Roasters", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    // TODO: Add Union Square Park once we support parks

    let metroStation =
        ("Union Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let private lowerEastSide (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.lowerEastSide (StreetType.Split(East, 2))

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
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let restaurants =
        [ ("Katz's Delicatessen", 90<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Indigo Lower East Side", 86<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces radioStudios
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels

let createZone (city: City) =
    let lowerManhattanZone = World.Zone.create Ids.Zone.lowerManhattan

    let bleeckerStreet = bleeckerStreet lowerManhattanZone
    let bowery = bowery lowerManhattanZone
    let irvingPlace, unionSquareMetro = irvingPlace city lowerManhattanZone
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
