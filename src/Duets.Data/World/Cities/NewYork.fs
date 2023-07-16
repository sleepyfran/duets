module rec Duets.Data.World.Cities.NewYork

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let private manhattan = World.Zone.create "Manhattan"
let private brooklyn = World.Zone.create "Brooklyn"
let private queens = World.Zone.create "Queens"
let private bronx = World.Zone.create "Bronx"
let private statenIsland = World.Zone.create "Staten Island"
let private harlem = World.Zone.create "Harlem"
let private soho = World.Zone.create "SoHo"
let private tribeca = World.Zone.create "Tribeca"
let private midtown = World.Zone.create "Midtown"
let private upperEastSide = World.Zone.create "Upper East Side"
let private upperWestSide = World.Zone.create "Upper West Side"
let private lowerEastSide = World.Zone.create "Lower East Side"
let private eastVillage = World.Zone.create "East Village"
let private westVillage = World.Zone.create "West Village"

/// Generates the city of New York.
let generate () =
    let createNewYork = World.City.create NewYork 8.4

    createHome
    |> createNewYork
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
            "John F. Kennedy International Airport"
            85<quality>
            Airport
            Everywhere.Common.airportLayout
            queens

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("McSorley's Old Ale House", 90<quality>, manhattan)
      ("Brooklyn Brewery", 92<quality>, brooklyn)
      ("Bohemian Hall and Beer Garden", 88<quality>, queens)
      ("Bronx Alehouse", 86<quality>, bronx)
      ("Flagship Brewery", 94<quality>, statenIsland) ]
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
    [ ("The Roasted Bean", 92<quality>, manhattan)
      ("Cup of Joy", 90<quality>, brooklyn)
      ("Java Palace", 88<quality>, queens)
      ("Brew and Foam", 91<quality>, bronx)
      ("Mug Harmony", 89<quality>, statenIsland) ]
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
    [ ("Madison Square Garden",
       20000,
       manhattan,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Radio City Music Hall",
       6000,
       manhattan,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Barclays Center",
       19000,
       brooklyn,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Carnegie Hall",
       2804,
       manhattan,
       89<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Beacon Theatre",
       2894,
       manhattan,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Apollo Theater",
       1506,
       harlem,
       80<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("The Bowery Ballroom",
       575,
       lowerEastSide,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Irving Plaza",
       1025,
       eastVillage,
       84<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Terminal 5",
       3000,
       midtown,
       82<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Webster Hall",
       1500,
       eastVillage,
       90<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Rough Trade NYC",
       250,
       brooklyn,
       80<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Forest Hills Stadium",
       14000,
       queens,
       83<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Kings Theatre",
       3000,
       brooklyn,
       87<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Town Hall",
       1500,
       manhattan,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The Hammerstein Ballroom",
       2200,
       manhattan,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The Metropolitan Opera House",
       3800,
       manhattan,
       91<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("The New York Philharmonic",
       2750,
       manhattan,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The Brooklyn Academy of Music",
       2100,
       brooklyn,
       88<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Joyce Theater",
       472,
       manhattan,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The New Victory Theater",
       499,
       manhattan,
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
        manhattan

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = World.Node.create 0 RoomType.Lobby

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "New York-Presbyterian Hospital"
            65<quality>
            Hospital
            roomGraph
            manhattan

    World.City.addPlace place city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Music Cellar", 85<quality>, 100m<dd>, manhattan)
      ("Sound Space", 90<quality>, 150m<dd>, brooklyn)
      ("Queens Studio", 92<quality>, 170m<dd>, queens)
      ("Bronx Beats", 80<quality>, 50m<dd>, bronx)
      ("Island Tunes", 88<quality>, 120m<dd>, statenIsland)
      ("Harlem Harmony", 86<quality>, 110m<dd>, harlem)
      ("SoHo Sound", 87<quality>, 120m<dd>, soho)
      ("Tribeca Tones", 85<quality>, 100m<dd>, tribeca)
      ("Midtown Melodies", 89<quality>, 140m<dd>, midtown)
      ("East Side Echoes", 90<quality>, 150m<dd>, upperEastSide) ]
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
    [ ("Carbone", 90<quality>, Italian, manhattan)
      ("Juliana's Pizza", 88<quality>, Italian, brooklyn)
      ("Peter Luger Steak House", 85<quality>, American, queens)
      ("Casa Enrique", 89<quality>, Mexican, bronx)
      ("Nobu", 87<quality>, Japanese, manhattan)
      ("Pho Bang", 86<quality>, Vietnamese, brooklyn)
      ("Lombardi's Pizza", 92<quality>, Italian, manhattan)
      ("Le Bernardin", 91<quality>, French, manhattan)
      ("Shake Shack", 84<quality>, American, manhattan)
      ("Los Tacos No.1", 88<quality>, Mexican, manhattan) ]
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
    [ ("Manhattan Records",
       85<quality>,
       200m<dd>,
       manhattan,
       (Character.from "John Smith" Male (December 24 1975)))
      ("Brooklyn Beats",
       90<quality>,
       300m<dd>,
       brooklyn,
       (Character.from "Eva Johnson" Female (March 15 1980)))
      ("Queens Quavers",
       92<quality>,
       340m<dd>,
       queens,
       (Character.from "Tom Davis" Male (July 10 1978)))
      ("Bronx Bass",
       80<quality>,
       100m<dd>,
       bronx,
       (Character.from "Jane Wilson" Female (September 5 1982)))
      ("Staten Sound",
       88<quality>,
       260m<dd>,
       statenIsland,
       (Character.from "Peter Brown" Male (June 20 1981)))
      ("Harlem Studios",
       86<quality>,
       220m<dd>,
       harlem,
       (Character.from "Elisa Miller" Female (April 1 1990)))
      ("SoHo Symphony",
       87<quality>,
       240m<dd>,
       soho,
       (Character.from "Martin Thompson" Male (January 30 1976)))
      ("Tribeca Tunes",
       85<quality>,
       200m<dd>,
       tribeca,
       (Character.from "Alice Davis" Female (August 15 1977)))
      ("Midtown Melodies",
       89<quality>,
       280m<dd>,
       midtown,
       (Character.from "Andrew Johnson" Male (February 2 1960)))
      ("East Side Echoes",
       90<quality>,
       300m<dd>,
       upperEastSide,
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
