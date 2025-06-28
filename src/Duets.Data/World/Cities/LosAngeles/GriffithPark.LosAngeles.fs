module Duets.Data.World.Cities.LosAngeles.GriffithPark

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let nVermontAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.nVermontAvenue
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("The Greek Theatre",
           5870,
           91<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Vermont/Sunset Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let griffithParkZone = World.Zone.create Ids.Zone.griffithPark

    let nVermontAvenue, vermontStation = nVermontAvenue griffithParkZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = nVermontAvenue.Id
          PlaceId = vermontStation.Id }

    griffithParkZone
    |> World.Zone.addStreet (World.Node.create nVermontAvenue.Id nVermontAvenue)
    |> World.Zone.addMetroStation station
