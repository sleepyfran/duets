module Duets.Data.World.Cities.Paris.SaintGermain

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let boulevardSaintGermain (zone: Zone) (cityId: CityId) (city: City) =
    let street =
        World.Street.create
            Ids.Street.boulevardSaintGermain
            (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Boulevard Saint-Germain is the grand east-west spine of the Left Bank,
        running from the Pont de Sully near the Île Saint-Louis all the way to the
        Pont de la Concorde. Its western end around Saint-Germain-des-Prés church—
        the oldest in Paris—is legendary for the literary cafés where Sartre, de Beauvoir,
        Camus and Hemingway once debated existentialism: Les Deux Magots and Café de Flore
        still attract crowds for their intellectual heritage as much as their coffee.
        The boulevard is flanked by premier bookshops, fashion houses, antique dealers
        and art galleries, making it the cultural high street of intellectual Paris.
"""

    let concertSpaces =
        [ ("L'Olympia",
           2000,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Théâtre du Châtelet",
           2500,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("Radio France Studios", 92<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let restaurants =
        [ ("Brasserie Lipp", 91<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Saint-Germain-des-Prés Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let rueDeSeine (zone: Zone) =
    let street =
        World.Street.create Ids.Street.rueDeSeine StreetType.OneWay
        |> World.Street.attachContext
            """
        Rue de Seine runs from the Boulevard Saint-Germain down to the Seine river,
        and is one of the most densely gallery-packed streets in Paris. Its short
        length contains an extraordinary concentration of contemporary and modern art
        galleries—dealers of painting, sculpture, photography and works on paper rub
        shoulders with antique shops and specialist print sellers. The street spills
        into the adjoining Rue Mazarine and Rue Guénégaud to form Paris's main gallery
        district, known simply as "Saint-Germain galleries." Early on weekday mornings,
        before the tourists arrive, it is almost eerily quiet, the pale light catching
        the art in the windows.
"""

    let cafes =
        [ ("Café de la Mairie", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("Prescription Cocktail Club", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let merchandiseWorkshops =
        [ ("FNAC Saint-Germain", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("Hôtel d'Aubusson", 93<quality>, 380m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces hotels

let rueDuBac (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.rueDuBac StreetType.OneWay
        |> World.Street.attachContext
            """
        Rue du Bac is one of the most elegant streets on the Left Bank, running south
        from the Seine through the heart of the 7th arrondissement. It is particularly
        famous for its upscale food shops, including the legendary Hédiard and Poilâne
        bakery. The street also passes the Bon Marché, Paris's oldest department store,
        and a cluster of prestigious antique dealers whose combined galleries form the
        Carré Rive Gauche. The Chapelle Notre-Dame de la Médaille Miraculeuse, a site
        of major Catholic pilgrimage, stands partway along the street, regularly drawing
        quiet queues of devotees among the fashionable shoppers.
"""

    let hospitals =
        [ ("Hôpital Laennec", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let rehearsalSpaces =
        [ ("La Fabrique Sonore", 86<quality>, 340m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let carDealers =
        [ ("Véhicules du Bac",
           70<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Pierre Morel"
                   Male
                   (Shorthands.Summer 5<days> 1975<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Club Med Gym Rive Gauche", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces gyms

let createZone (cityId: CityId) (city: City) =
    let saintGermainZone = World.Zone.create Ids.Zone.saintGermain

    let boulevardSaintGermain, saintGermainStation =
        boulevardSaintGermain saintGermainZone cityId city

    let rueDeSeine = rueDeSeine saintGermainZone
    let rueDuBac = rueDuBac saintGermainZone city

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = boulevardSaintGermain.Id
          PlaceId = saintGermainStation.Id }

    saintGermainZone
    |> World.Zone.addStreet (
        World.Node.create boulevardSaintGermain.Id boulevardSaintGermain
    )
    |> World.Zone.addStreet (World.Node.create rueDeSeine.Id rueDeSeine)
    |> World.Zone.addStreet (World.Node.create rueDuBac.Id rueDuBac)
    |> World.Zone.connectStreets boulevardSaintGermain.Id rueDeSeine.Id North
    |> World.Zone.connectStreets rueDeSeine.Id rueDuBac.Id East
    |> World.Zone.addMetroStation station
