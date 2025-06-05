module rec Duets.Data.World.Cities.NewYork

open Duets.Entities
open Duets.Entities.Calendar

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
    let createNewYork =
        World.City.create NewYork 6.0<costOfLiving> -5<utcOffset>

    createHome
    |> createNewYork
    |> addAirport
    |> addBars
    |> addBookstores
    |> addCafes
    |> addCasinos
    |> addConcertSpaces
    |> addGyms
    |> addHospital
    |> addHotels
    |> addMerchandiseWorkshops
    |> addRehearsalSpaces
    |> addRestaurants
    |> addStudios
    |> addRadioStudios

(* -------- Airport --------- *)
let addAirport city =
    let place =
        World.Place.create
            "John F. Kennedy International Airport"
            85<quality>
            Airport
            Layouts.airportLayout
            astoria

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("McSorley's Old Ale House", 90<quality>, eastVillage)
      ("Brooklyn Brewery", 92<quality>, brooklynHeights)
      ("Bohemian Hall and Beer Garden", 88<quality>, astoria)
      ("Bronx Alehouse", 86<quality>, riverdale)
      ("Flagship Brewery", 94<quality>, stGeorge) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("Strand Book Store", 95<quality>, eastVillage)
      ("Books Are Magic", 92<quality>, brooklynHeights)
      ("Housing Works Bookstore Cafe", 90<quality>, soho)
      ("McNally Jackson Books", 89<quality>, lowerEastSide)
      ("The Mysterious Bookshop", 87<quality>, tribeca) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("The Roasted Bean", 92<quality>, soho)
      ("Cup of Joy", 90<quality>, brooklynHeights)
      ("Java Palace", 88<quality>, astoria)
      ("Brew and Foam", 91<quality>, riverdale)
      ("Mug Harmony", 89<quality>, stGeorge) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------*)
let private addCasinos city =
    [ ("Big Apple Casino", 92<quality>, midtown)
      ("Empire Fortune Casino", 90<quality>, tribeca)
      ("Statue of Luck Casino", 88<quality>, astoria)
      ("Broadway Gamblers' Haven", 91<quality>, upperEastSide)
      ("Harbor Lights Resort", 89<quality>, stGeorge) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Madison Square Garden",
       20000,
       midtown,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Radio City Music Hall",
       6000,
       midtown,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("Barclays Center",
       19000,
       brooklynHeights,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Carnegie Hall", 2804, midtown, 89<quality>, Layouts.concertSpaceLayout4)
      ("Beacon Theatre",
       2894,
       upperWestSide,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("Apollo Theater", 1506, harlem, 80<quality>, Layouts.concertSpaceLayout1)
      ("The Bowery Ballroom",
       575,
       lowerEastSide,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("Irving Plaza",
       1025,
       eastVillage,
       84<quality>,
       Layouts.concertSpaceLayout1)
      ("Terminal 5", 3000, midtown, 82<quality>, Layouts.concertSpaceLayout2)
      ("Webster Hall",
       1500,
       eastVillage,
       90<quality>,
       Layouts.concertSpaceLayout3)
      ("Rough Trade NYC",
       250,
       brooklynHeights,
       80<quality>,
       Layouts.concertSpaceLayout4)
      ("Forest Hills Stadium",
       14000,
       astoria,
       83<quality>,
       Layouts.concertSpaceLayout1)
      ("Kings Theatre",
       3000,
       brooklynHeights,
       87<quality>,
       Layouts.concertSpaceLayout4)
      ("The Town Hall", 1500, midtown, 88<quality>, Layouts.concertSpaceLayout2)
      ("The Hammerstein Ballroom",
       2200,
       midtown,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("The Metropolitan Opera House",
       3800,
       upperWestSide,
       91<quality>,
       Layouts.concertSpaceLayout1)
      ("The New York Philharmonic",
       2750,
       upperWestSide,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("The Brooklyn Academy of Music",
       2100,
       brooklynHeights,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("The Joyce Theater",
       472,
       midtown,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("The New Victory Theater",
       499,
       midtown,
       90<quality>,
       Layouts.concertSpaceLayout1) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout soho

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
    |> List.map (PlaceCreators.createGym city)
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
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("NY Merch", midtown)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

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
    |> List.map PlaceCreators.createRehearsalSpace
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
      ("Los Tacos No.1", 88<quality>, Mexican, eastVillage)
      ("Istanbul Kebab House", 67<quality>, Turkish, astoria)
      ("Turkish Delight", 86<quality>, Turkish, soho) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("SoHo Records",
       85<quality>,
       200m<dd>,
       soho,
       (Character.from
           "John Smith"
           Male
           (Shorthands.Winter 24<days> 1975<years>)))
      ("Brooklyn Studio",
       90<quality>,
       300m<dd>,
       brooklynHeights,
       (Character.from
           "Eva Johnson"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("Astoria Sound",
       92<quality>,
       340m<dd>,
       astoria,
       (Character.from "Tom Davis" Male (Shorthands.Summer 10<days> 1978<years>)))
      ("Riverdale Studio",
       80<quality>,
       100m<dd>,
       riverdale,
       (Character.from
           "Jane Wilson"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("St. George Sound",
       88<quality>,
       260m<dd>,
       stGeorge,
       (Character.from
           "Peter Brown"
           Male
           (Shorthands.Summer 20<days> 1981<years>)))
      ("Harlem Harmony Studios",
       86<quality>,
       220m<dd>,
       harlem,
       (Character.from
           "Elisa Miller"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("Tribeca Studios",
       85<quality>,
       200m<dd>,
       tribeca,
       (Character.from
           "Alice Davis"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("Midtown Studios",
       89<quality>,
       280m<dd>,
       midtown,
       (Character.from
           "Martin Thompson"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Upper East Side Sound",
       90<quality>,
       300m<dd>,
       upperEastSide,
       (Character.from
           "Margaret Taylor"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Radio Studios --------- *)
let private addRadioStudios city =
    [ ("Z100", 94<quality>, "Pop", midtown)
      ("Q104.3", 92<quality>, "Rock", midtown)
      ("WBGO 88.3 FM", 90<quality>, "Jazz", harlem) ]
    |> List.map (PlaceCreators.createRadioStudio city)
    |> List.fold (fun city place -> World.City.addPlace place city) city
