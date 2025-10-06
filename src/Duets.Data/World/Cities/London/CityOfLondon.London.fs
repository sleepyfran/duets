module rec Duets.Data.World.Cities.London.CityOfLondon

open Duets.Data.World.Cities
open Duets.Entities

let private bank (zone: Zone) =
    let street =
        World.Street.create "Bank" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Bank is the historic financial heart of London, named after the Bank of England
        which dominates the area with its imposing neoclassical facade. The junction where
        seven streets converge creates a bustling intersection surrounded by grand Victorian
        and Edwardian banking halls, now repurposed as bars and restaurants. The Royal Exchange
        with its impressive columned portico stands as a testament to centuries of commerce.
        Narrow medieval alleyways wind between towering modern office blocks, creating dramatic
        contrasts between old and new. During weekdays, the area swarms with suited professionals,
        while weekends see it quieter and more atmospheric.
"""

    let bars =
        [ ("The Counting House", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("The Ned", 90<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Bank Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let private stPauls (city: City) (zone: Zone) =
    let street =
        World.Street.create "St Paul's" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        St Paul's is dominated by Christopher Wren's magnificent cathedral, its iconic dome
        visible from across the city skyline. The cathedral steps and surrounding pedestrianized
        areas provide open spaces rare in the densely built City. Paternoster Square to the
        north features modern architecture mixed with restored medieval street patterns. The
        area retains a contemplative atmosphere despite the modern office workers, with the
        cathedral bells marking the hours. Millennium Bridge to the south offers views across
        the Thames to Tate Modern.
"""

    let bookstores =
        [ ("Daunt Books", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Cafe Rouge", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let radioStudios =
        [ ("Jazz FM", 88<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces radioStudios

let private barbican (zone: Zone) =
    let street =
        World.Street.create "Barbican" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        The Barbican is a striking example of Brutalist architecture, a vast residential and
        cultural complex built in the 1960s-80s on WWII bomb sites. Elevated walkways connect
        fortress-like concrete towers and terraced housing blocks arranged around a central
        lake and gardens. The arts centre hosts world-class concerts, theatre, and exhibitions.
        Despite its harsh exterior, hidden gardens and conservatories provide unexpected green
        spaces. The area has a unique, futuristic atmosphere that divides opinion but remains
        an iconic London landmark.
"""

    let concertSpaces =
        [ ("Barbican Hall",
           1943,
           92<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Guildhall Practice Rooms", 80<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces

let private stBartsHospital (zone: Zone) =
    let street =
        World.Street.create "St Bartholomew's Hospital" StreetType.OneWay
        |> World.Street.attachContext
            """
        St Bartholomew's Hospital, founded in 1123, is Britain's oldest hospital still
        occupying its original site. The main square features an elegant 18th-century
        gateway leading to buildings that blend medieval, Georgian, and modern architecture.
        The atmosphere is purposeful and calm, with medical staff and visitors moving through
        historic courtyards. The nearby St Bartholomew-the-Great church, London's oldest
        parish church, adds to the area's medieval character. Despite surrounding modern
        development, this enclave preserves centuries of healing tradition.
"""

    let hospital =
        ("St Bartholomew's Hospital", 100<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    street |> World.Street.addPlace hospital

let createZone (city: City) =
    let zone = World.Zone.create "City of London"
    let bank, metroStation = bank zone
    let stPauls = stPauls city zone
    let barbican = barbican zone
    let stBarts = stBartsHospital zone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = bank.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create bank.Id bank)
    |> World.Zone.addStreet (World.Node.create stPauls.Id stPauls)
    |> World.Zone.addStreet (World.Node.create barbican.Id barbican)
    |> World.Zone.addStreet (World.Node.create stBarts.Id stBarts)
    |> World.Zone.connectStreets bank.Id stPauls.Id North
    |> World.Zone.connectStreets stPauls.Id barbican.Id East
    |> World.Zone.connectStreets bank.Id stBarts.Id South
    |> World.Zone.addMetroStation metroStation

let zone = createZone
