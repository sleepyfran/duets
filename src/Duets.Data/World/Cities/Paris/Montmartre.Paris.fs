module Duets.Data.World.Cities.Paris.Montmartre

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let rueDesAbbesses (zone: Zone) (cityId: CityId) =
    let street =
        World.Street.create
            Ids.Street.rueDesAbbesses
            (StreetType.Split(East, 2))
        |> World.Street.tag "abbesses"

    let concertSpaces =
        [ ("Le Divan du Monde",
           300,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("La Cigale",
           1400,
           90<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Le Rendez-Vous des Amis", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Café des Deux Moulins", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Abbesses Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let rueLepic (zone: Zone) =
    let street =
        World.Street.create Ids.Street.rueLepic StreetType.OneWay

    let restaurants =
        [ ("Chez Plumeau", 86<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bookstores =
        [ ("Librairie des Abbesses", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let merchandiseWorkshops =
        [ ("La Maison de la Bonne Chanson", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces merchandiseWorkshops

let boulevardDeClichy (zone: Zone) (cityId: CityId) =
    let street =
        World.Street.create
            Ids.Street.boulevardDeClichy
            (StreetType.Split(East, 2))
        |> World.Street.tag "moulin-rouge"

    let home = PlaceCreators.createHome street.Id zone.Id

    let concertSpaces =
        [ ("Moulin Rouge",
           850,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("Hôtel Particulier Montmartre", 93<quality>, 420m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cinemas =
        [ ("Le Studio 28", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces cinemas

let createZone (cityId: CityId) =
    let montmartreZone = World.Zone.create Ids.Zone.montmartre

    let rueDesAbbesses, abbessesStation = rueDesAbbesses montmartreZone cityId
    let rueLepic = rueLepic montmartreZone
    let boulevardDeClichy = boulevardDeClichy montmartreZone cityId

    let station =
        { Lines = [ Red ]
          LeavesToStreet = rueDesAbbesses.Id
          PlaceId = abbessesStation.Id }

    montmartreZone
    |> World.Zone.addStreet (
        World.Node.create rueDesAbbesses.Id rueDesAbbesses
    )
    |> World.Zone.addStreet (World.Node.create rueLepic.Id rueLepic)
    |> World.Zone.addStreet (
        World.Node.create boulevardDeClichy.Id boulevardDeClichy
    )
    |> World.Zone.connectStreets rueDesAbbesses.Id rueLepic.Id North
    |> World.Zone.connectStreets rueLepic.Id boulevardDeClichy.Id East
    |> World.Zone.addMetroStation station
