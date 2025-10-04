module Duets.Data.World.Cities.LosAngeles.Koreatown

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let wilshireBoulevard (zone: Zone) (city: City) =
    let street =
        World.Street.create
            Ids.Street.wilshireBoulevardKoreatown
            (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Wilshire Boulevard is the main east-west artery of Koreatown, defined by
        high-density urban development, historic architecture, and major transportation hubs.
        It is the location of significant landmarks like the iconic jade-green
        Wiltern Theatre and its attached Pellissier Building, a masterpiece of
        Art Deco architecture that serves as a major concert venue. The boulevard
        features modern high-rises, mixed with preserved historic buildings and
        plazas like Beneficial Plaza and Liberty Park.
"""

    let concertSpaces =
        [ ("The Wiltern Theatre",
           2300,
           90<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let gyms =
        [ ("Equinox K-Town", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let hospitals =
        [ ("Kaiser Permanente K-Town", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let radioStudios =
        [ ("K-Town Broadcast Tower", 87<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let restaurants =
        [ ("BCD Tofu House", 86<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("The Wilshire Residency", 75<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Wilshire/Western Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let westernAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.westernAvenue StreetType.OneWay
        |> World.Street.attachContext
            """
        Western Avenue is a major north-south street forming the western boundary
        of Koreatown. As one of the longest major north-south avenues in LA County,
        its Koreatown stretch is characterized by its high concentration of businesses,
        including shopping centers like Koreatown Galleria and smaller, busy commercial
        strips. It acts as a bustling, vibrant, and multi-cultural corridor,
        intersecting with many of the neighborhood's side streets that lead to the
        residential core. The intersection with Wilshire Boulevard marks a key,
        dense nexus of the community.
"""

    let concertSpaces =
        [ ("The Catch One",
           350,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("The Plaza Practice Rooms", 85<quality>, 300m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("Indie Lab",
           83<quality>,
           450m<dd>,
           (Character.from
               "Marcus Kim"
               Male
               (Shorthands.Summer 20<days> 1985<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    // TODO: Add karaoke venue support.
    let bars =
        [ ("Noraebang City", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("The K-Town Bean", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let koreatownZone = World.Zone.create Ids.Zone.koreatown

    let wilshireBoulevard, wilshireStation =
        wilshireBoulevard koreatownZone city

    let westernAvenue = westernAvenue koreatownZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = wilshireBoulevard.Id
          PlaceId = wilshireStation.Id }

    koreatownZone
    |> World.Zone.addStreet (
        World.Node.create wilshireBoulevard.Id wilshireBoulevard
    )
    |> World.Zone.addStreet (World.Node.create westernAvenue.Id westernAvenue)
    |> World.Zone.connectStreets wilshireBoulevard.Id westernAvenue.Id North
    |> World.Zone.addMetroStation station
