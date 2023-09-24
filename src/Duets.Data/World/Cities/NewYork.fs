module rec Duets.Data.World.Cities.NewYork

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let private soho = World.Zone.create "SoHo"
let private tribeca = World.Zone.create "Tribeca"
let private midtown = World.Zone.create "Midtown"
let private upperEastSide = World.Zone.create "Upper East Side"
let private upperWestSide = World.Zone.create "Upper West Side"
let private lowerEastSide = World.Zone.create "Lower East Side"
let private eastVillage = World.Zone.create "East Village"
let private westVillage = World.Zone.create "West Village"
let private harlem = World.Zone.create "Harlem"
let private brooklynHeights = World.Zone.create "Brooklyn Heights"
let private astoria = World.Zone.create "Astoria"
let private riverdale = World.Zone.create "Riverdale"
let private stGeorge = World.Zone.create "St. George"

/// Generates the city of New York.
let generate () =
    let createNewYork = World.City.create NewYork 6.0

    createHome
    |> createNewYork
    |> addAirport
    |> addBars
    |> addCafes
    |> addCasinos
    |> addConcertSpaces
    |> addGyms
    |> addHospital
    |> addHotels
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
            astoria

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("McSorley's Old Ale House", 90<quality>, eastVillage)
      ("Brooklyn Brewery", 92<quality>, brooklynHeights)
      ("Bohemian Hall and Beer Garden", 88<quality>, astoria)
      ("Bronx Alehouse", 86<quality>, riverdale)
      ("Flagship Brewery", 94<quality>, stGeorge) ]
    |> List.map Common.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("The Roasted Bean", 92<quality>, soho)
      ("Cup of Joy", 90<quality>, brooklynHeights)
      ("Java Palace", 88<quality>, astoria)
      ("Brew and Foam", 91<quality>, riverdale)
      ("Mug Harmony", 89<quality>, stGeorge) ]
    |> List.map Common.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------*)
let private addCasinos city =
    [ ("Big Apple Casino", 92<quality>, midtown)
      ("Empire Fortune Casino", 90<quality>, tribeca)
      ("Statue of Luck Casino", 88<quality>, astoria)
      ("Broadway Gamblers' Haven", 91<quality>, upperEastSide)
      ("Harbor Lights Resort", 89<quality>, stGeorge) ]
    |> List.map Common.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Madison Square Garden",
       20000,
       midtown,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Radio City Music Hall",
       6000,
       midtown,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Barclays Center",
       19000,
       brooklynHeights,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Carnegie Hall",
       2804,
       midtown,
       89<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Beacon Theatre",
       2894,
       upperWestSide,
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
       brooklynHeights,
       80<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Forest Hills Stadium",
       14000,
       astoria,
       83<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Kings Theatre",
       3000,
       brooklynHeights,
       87<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Town Hall",
       1500,
       midtown,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The Hammerstein Ballroom",
       2200,
       midtown,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The Metropolitan Opera House",
       3800,
       upperWestSide,
       91<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("The New York Philharmonic",
       2750,
       upperWestSide,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("The Brooklyn Academy of Music",
       2100,
       brooklynHeights,
       88<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("The Joyce Theater",
       472,
       midtown,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("The New Victory Theater",
       499,
       midtown,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1) ]
    |> List.map Common.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create
        "Home"
        100<quality>
        Home
        Everywhere.Common.homeLayout
        soho

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Equinox SoHo", 90<quality>, soho)
      ("Brooklyn Boulders Gowanus", 88<quality>, brooklynHeights)
      ("YMCA McBurney", 85<quality>, westVillage)
      ("Planet Fitness Harlem", 89<quality>, harlem)
      ("Blink Fitness Bronx", 87<quality>, riverdale)
      ("Crunch Fitness", 86<quality>, upperEastSide)
      ("24 Hour Fitness Midtown", 92<quality>, midtown)
      ("NYSC Astoria", 85<quality>, astoria)
      ("The Edge Gym", 88<quality>, harlem)
      ("Gold's Gym Upper East Side", 84<quality>, upperEastSide) ]
    |> List.map (Common.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "New York-Presbyterian Hospital"
            65<quality>
            Hospital
            roomGraph
            upperEastSide

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("The Plaza", 98<quality>, 240m<dd>, midtown)
      ("The St. Regis New York", 95<quality>, 220m<dd>, upperEastSide)
      ("The Peninsula New York", 92<quality>, 210m<dd>, midtown)
      ("The Ritz-Carlton New York", 90<quality>, 200m<dd>, upperWestSide)
      ("The Lowell Hotel", 88<quality>, 190m<dd>, upperEastSide)
      ("The Pierre", 85<quality>, 180m<dd>, upperEastSide)
      ("The Carlyle", 80<quality>, 160m<dd>, upperEastSide)
      ("The Mark New York", 75<quality>, 140m<dd>, upperEastSide)
      ("The Roosevelt Hotel", 70<quality>, 120m<dd>, midtown)
      ("The New Yorker", 65<quality>, 100m<dd>, midtown) ]
    |> List.map Common.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Music Cellar", 85<quality>, 100m<dd>, soho)
      ("Sound Space", 90<quality>, 150m<dd>, brooklynHeights)
      ("Queens Quavers", 92<quality>, 170m<dd>, astoria)
      ("Bronx Bass", 80<quality>, 50m<dd>, riverdale)
      ("Island Tunes", 88<quality>, 120m<dd>, stGeorge)
      ("Harlem Harmony", 86<quality>, 110m<dd>, harlem)
      ("SoHo Sound", 87<quality>, 120m<dd>, soho)
      ("Tribeca Tones", 85<quality>, 100m<dd>, tribeca)
      ("Midtown Melodies", 89<quality>, 140m<dd>, midtown)
      ("East Side Echoes", 90<quality>, 150m<dd>, upperEastSide) ]
    |> List.map Common.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Carbone", 90<quality>, Italian, soho)
      ("Juliana's Pizza", 88<quality>, Italian, brooklynHeights)
      ("Peter Luger Steak House", 85<quality>, American, brooklynHeights)
      ("Casa Enrique", 89<quality>, Mexican, astoria)
      ("Nobu", 87<quality>, Japanese, tribeca)
      ("Pho Bang", 86<quality>, Vietnamese, eastVillage)
      ("Lombardi's Pizza", 92<quality>, Italian, lowerEastSide)
      ("Le Bernardin", 91<quality>, French, midtown)
      ("Shake Shack", 84<quality>, American, upperWestSide)
      ("Los Tacos No.1", 88<quality>, Mexican, eastVillage) ]
    |> List.map Common.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("SoHo Records",
       85<quality>,
       200m<dd>,
       soho,
       (Character.from "John Smith" Male (December 24 1975)))
      ("Brooklyn Studio",
       90<quality>,
       300m<dd>,
       brooklynHeights,
       (Character.from "Eva Johnson" Female (March 15 1980)))
      ("Astoria Sound",
       92<quality>,
       340m<dd>,
       astoria,
       (Character.from "Tom Davis" Male (July 10 1978)))
      ("Riverdale Studio",
       80<quality>,
       100m<dd>,
       riverdale,
       (Character.from "Jane Wilson" Female (September 5 1982)))
      ("St. George Sound",
       88<quality>,
       260m<dd>,
       stGeorge,
       (Character.from "Peter Brown" Male (June 20 1981)))
      ("Harlem Harmony Studios",
       86<quality>,
       220m<dd>,
       harlem,
       (Character.from "Elisa Miller" Female (April 1 1990)))
      ("Tribeca Studios",
       85<quality>,
       200m<dd>,
       tribeca,
       (Character.from "Alice Davis" Female (August 15 1977)))
      ("Midtown Studios",
       89<quality>,
       280m<dd>,
       midtown,
       (Character.from "Martin Thompson" Male (January 30 1976)))
      ("Upper East Side Sound",
       90<quality>,
       300m<dd>,
       upperEastSide,
       (Character.from "Margaret Taylor" Female (May 3 1988))) ]
    |> List.map Common.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city
