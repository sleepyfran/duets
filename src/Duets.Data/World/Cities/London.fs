module rec Duets.Data.World.Cities.London

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let private cityOfLondon = World.Zone.create "City of London"
let private camden = World.Zone.create "Camden"
let private hackney = World.Zone.create "Hackney"
let private islington = World.Zone.create "Islington"
let private kensington = World.Zone.create "Kensington"
let private lambeth = World.Zone.create "Lambeth"
let private southwark = World.Zone.create "Southwark"
let private westminster = World.Zone.create "Westminster"
let private greenwich = World.Zone.create "Greenwich"
let private towerHamlets = World.Zone.create "Tower Hamlets"
let private hammersmith = World.Zone.create "Hammersmith"
let private fulham = World.Zone.create "Fulham"
let private wandsworth = World.Zone.create "Wandsworth"
let private newham = World.Zone.create "Newham"
let private tottenham = World.Zone.create "Tottenham"
let private morden = World.Zone.create "Morden"
let private deptford = World.Zone.create "Deptford"

/// Generates the city of London.
let generate () =
    let createLondon = World.City.create London 8.9

    createHome
    |> createLondon
    |> addAirport
    |> addBars
    |> addCafes
    |> addConcertSpaces
    |> addHospital
    |> addRehearsalSpaces
    |> addRestaurants
    |> addStudios

(* -------- Airport --------- *)
let addAirport city =
    let place =
        World.Place.create
            "Heathrow Airport"
            85<quality>
            Airport
            Everywhere.Common.airportLayout
            hammersmith

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("The Mayflower", 90<quality>, southwark)
      ("Ye Olde Cheshire Cheese", 92<quality>, cityOfLondon)
      ("The Churchill Arms", 88<quality>, kensington)
      ("The Spaniards Inn", 86<quality>, camden)
      ("The Dove", 94<quality>, hammersmith) ]
    |> List.map (fun (name, quality, zone) ->
        World.Place.create
            name
            quality
            Bar
            Everywhere.Common.barRoomLayout
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("Monmouth Coffee Company", 92<quality>, southwark)
      ("The Attendant", 90<quality>, hackney)
      ("Kaffeine", 88<quality>, westminster)
      ("The Coffee Jar", 91<quality>, camden)
      ("TAP Coffee", 89<quality>, cityOfLondon) ]
    |> List.map (fun (name, quality, zone) ->
        World.Place.create
            name
            quality
            Cafe
            Everywhere.Common.cafeRoomLayout
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("The O2 Arena",
       20000,
       greenwich,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Royal Albert Hall",
       5272,
       kensington,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Brixton Academy",
       4921,
       lambeth,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Roundhouse",
       1700,
       camden,
       89<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Hammersmith Apollo",
       5000,
       hammersmith,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Barbican Centre",
       1943,
       cityOfLondon,
       80<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Union Chapel",
       900,
       islington,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Scala", 1145, camden, 84<quality>, Everywhere.Common.concertSpaceLayout1)
      ("Bush Hall",
       350,
       hammersmith,
       82<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The Forum",
       2300,
       camden,
       90<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Electric Ballroom",
       1100,
       camden,
       80<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Underworld",
       500,
       camden,
       83<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("The Jazz Cafe",
       420,
       camden,
       87<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Garage",
       600,
       islington,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The Borderline",
       300,
       westminster,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The 100 Club",
       350,
       westminster,
       91<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("The Lexington",
       200,
       islington,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The Dublin Castle",
       200,
       camden,
       88<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Windmill Brixton",
       150,
       lambeth,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The Half Moon",
       220,
       wandsworth,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1) ]
    |> List.map (fun (name, capacity, zone, quality, layout) ->
        World.Place.create
            name
            quality
            (ConcertSpace { Capacity = capacity })
            layout
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create
        "Home"
        100<quality>
        Home
        Everywhere.Common.homeLayout
        cityOfLondon

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = World.Node.create 0 RoomType.Lobby

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "St Thomas' Hospital"
            65<quality>
            Hospital
            roomGraph
            lambeth

    World.City.addPlace place city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Premises Studios", 85<quality>, 100m<dd>, hackney)
      ("Pirate Studios", 90<quality>, 150m<dd>, greenwich)
      ("The Joint", 92<quality>, 170m<dd>, lambeth)
      ("Bally Studios", 80<quality>, 50m<dd>, tottenham)
      ("Crown Lane Studio", 88<quality>, 120m<dd>, morden)
      ("The Music Complex", 86<quality>, 110m<dd>, deptford)
      ("Bush Studios", 87<quality>, 120m<dd>, southwark)
      ("New Rose Studios", 85<quality>, 100m<dd>, islington)
      ("John Henry's", 89<quality>, 140m<dd>, islington)
      ("Resident Studios", 90<quality>, 150m<dd>, newham) ]
    |> List.map (fun (name, quality, price, zone) ->
        World.Place.create
            name
            quality
            (RehearsalSpace { Price = price })
            Everywhere.Common.rehearsalSpaceLayout
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Dabbous", 90<quality>, French, westminster)
      ("The Ledbury", 88<quality>, French, kensington)
      ("La Burratta", 85<quality>, Italian, camden)
      ("Barrafina", 89<quality>, Italian, westminster)
      ("Yauatcha", 87<quality>, Japanese, cityOfLondon)
      ("The Wolseley", 86<quality>, French, westminster)
      ("Duck & Waffle", 92<quality>, American, cityOfLondon)
      ("Sketch", 91<quality>, French, westminster)
      ("Burger & Lobster", 84<quality>, American, camden)
      ("Hawksmoor", 88<quality>, American, cityOfLondon) ]
    |> List.map (fun (name, quality, cuisine, zone) ->
        World.Place.create
            name
            quality
            Restaurant
            (Everywhere.Common.restaurantRoomLayout cuisine)
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Studios --------- *)
let private addStudios city =
    [ ("Abbey Road Studios",
       85<quality>,
       200m<dd>,
       camden,
       (Character.from "George Martin" Male (January 3 1926)))
      ("AIR Studios",
       90<quality>,
       300m<dd>,
       camden,
       (Character.from "Eva Johnson" Female (March 15 1980)))
      ("Metropolis Studios",
       92<quality>,
       340m<dd>,
       hammersmith,
       (Character.from "Tom Davis" Male (July 10 1978)))
      ("Sarm West Studios",
       80<quality>,
       100m<dd>,
       westminster,
       (Character.from "Jane Wilson" Female (September 5 1982)))
      ("Strongroom",
       88<quality>,
       260m<dd>,
       hackney,
       (Character.from "Peter Brown" Male (June 20 1981)))
      ("The Church Studios",
       86<quality>,
       220m<dd>,
       camden,
       (Character.from "Elisa Miller" Female (April 1 1990)))
      ("Trident Studios",
       87<quality>,
       240m<dd>,
       towerHamlets,
       (Character.from "Martin Thompson" Male (January 30 1976)))
      ("Olympic Studios",
       85<quality>,
       200m<dd>,
       hammersmith,
       (Character.from "Alice Davis" Female (August 15 1977)))
      ("The Pool",
       89<quality>,
       280m<dd>,
       lambeth,
       (Character.from "Andrew Johnson" Male (February 2 1960)))
      ("Rak Studios",
       90<quality>,
       300m<dd>,
       westminster,
       (Character.from "Margaret Taylor" Female (May 3 1988))) ]
    |> List.map (fun (name, quality, pricePerSong, zone, producer) ->
        let studio =
            { Producer = producer
              PricePerSong = pricePerSong }

        World.Place.create
            name
            quality
            (Studio studio)
            Everywhere.Common.studioLayout
            zone)
    |> List.fold (fun city place -> World.City.addPlace place city) city
