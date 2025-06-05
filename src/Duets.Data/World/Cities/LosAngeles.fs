module rec Duets.Data.World.Cities.LosAngeles

open Duets.Entities
open Duets.Entities.Calendar

let private downtownLA = World.Zone.create "Downtown LA"
let private hollywood = World.Zone.create "Hollywood"
let private santaMonica = World.Zone.create "Santa Monica"
let private veniceBeach = World.Zone.create "Venice Beach"
let private beverlyHills = World.Zone.create "Beverly Hills"
let private silverLake = World.Zone.create "Silver Lake"
let private echoPark = World.Zone.create "Echo Park"
let private westHollywood = World.Zone.create "West Hollywood"
let private losFeliz = World.Zone.create "Los Feliz"
let private pasadena = World.Zone.create "Pasadena"
let private longBeach = World.Zone.create "Long Beach"
let private malibu = World.Zone.create "Malibu"
let private studioCity = World.Zone.create "Studio City"
let private westchester = World.Zone.create "Westchester"
let private inglewood = World.Zone.create "Inglewood"
let private koreaTown = World.Zone.create "Koreatown"
let private midWilshire = World.Zone.create "Mid-Wilshire"
let private westLosAngeles = World.Zone.create "West Los Angeles"
let private westlake = World.Zone.create "Westlake"
let private highlandPark = World.Zone.create "Highland Park"

/// Generates the city of Los Angeles.
let generate () =
    let createNewYork =
        World.City.create LosAngeles 5.7<costOfLiving> -8<utcOffset>

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
            "Los Angeles International Airport"
            81<quality>
            Airport
            Layouts.airportLayout
            westchester

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("The Varnish", 90<quality>, downtownLA)
      ("Seven Grand", 92<quality>, downtownLA)
      ("Harvard & Stone", 88<quality>, hollywood)
      ("The Spare Room", 90<quality>, hollywood)
      ("La Cuevita", 85<quality>, silverLake) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("The Last Bookstore", 92<quality>, downtownLA)
      ("Skylight Books", 90<quality>, losFeliz)
      ("Vroman's Bookstore", 88<quality>, pasadena)
      ("Book Soup", 91<quality>, westHollywood)
      ("Eso Won Books", 89<quality>, silverLake) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("The Bean Roastery", 92<quality>, downtownLA)
      ("Sunshine Cafe", 90<quality>, hollywood)
      ("LA Perk", 88<quality>, santaMonica)
      ("Venice Brew House", 91<quality>, veniceBeach)
      ("Beverly Beans", 89<quality>, beverlyHills)
      ("Silver Brew", 87<quality>, silverLake)
      ("Echo Espresso", 90<quality>, echoPark)
      ("West Hollywood Roasters", 93<quality>, westHollywood)
      ("Los Feliz Latte", 88<quality>, losFeliz)
      ("Pasadena Percolate", 91<quality>, pasadena)
      ("Beachfront Brews", 89<quality>, longBeach)
      ("Malibu Morning", 90<quality>, malibu)
      ("Studio City Sips", 88<quality>, studioCity) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("City of Angels Casino", 92<quality>, downtownLA)
      ("LA Gold Rush Casino", 90<quality>, hollywood)
      ("Pacific Paradise Casino", 88<quality>, veniceBeach)
      ("Sunset Strip Casino", 91<quality>, westHollywood)
      ("Hollywood Hills Casino", 89<quality>, beverlyHills)
      ("Silver Star Casino", 87<quality>, silverLake)
      ("Echo Beach Casino", 90<quality>, echoPark) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Staples Center",
       21000,
       downtownLA,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Hollywood Bowl",
       17000,
       hollywood,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("The Forum", 17000, inglewood, 88<quality>, Layouts.concertSpaceLayout2)
      ("Walt Disney Concert Hall",
       2265,
       downtownLA,
       89<quality>,
       Layouts.concertSpaceLayout4)
      ("Greek Theatre", 5700, losFeliz, 95<quality>, Layouts.concertSpaceLayout2)
      ("The Wiltern", 1850, koreaTown, 80<quality>, Layouts.concertSpaceLayout1)
      ("The Palladium",
       3500,
       hollywood,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("The Fonda Theatre",
       1200,
       hollywood,
       84<quality>,
       Layouts.concertSpaceLayout1)
      ("The Novo", 2000, downtownLA, 82<quality>, Layouts.concertSpaceLayout2)
      ("El Rey Theatre",
       775,
       midWilshire,
       90<quality>,
       Layouts.concertSpaceLayout3)
      ("The Echo", 350, echoPark, 80<quality>, Layouts.concertSpaceLayout4)
      ("The Troubadour",
       450,
       westHollywood,
       87<quality>,
       Layouts.concertSpaceLayout4)
      ("The Regent Theater",
       900,
       downtownLA,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("The Orpheum Theatre",
       2000,
       downtownLA,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("The Dorothy Chandler Pavilion",
       3197,
       downtownLA,
       91<quality>,
       Layouts.concertSpaceLayout1)
      ("The Theatre at Ace Hotel",
       1600,
       downtownLA,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("The Teragram Ballroom",
       600,
       downtownLA,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("The Bootleg Theater",
       200,
       westlake,
       67<quality>,
       Layouts.concertSpaceLayout2)
      ("The Moroccan Lounge",
       275,
       downtownLA,
       78<quality>,
       Layouts.concertSpaceLayout1)
      ("The Hi Hat", 150, highlandPark, 67<quality>, Layouts.concertSpaceLayout2) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let private createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout beverlyHills

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("LA Fitness Downtown LA", 90<quality>, downtownLA)
      ("Hollywood Gym", 88<quality>, hollywood)
      ("Santa Monica Family YMCA", 85<quality>, santaMonica)
      ("Venice Beach Fitness", 89<quality>, veniceBeach)
      ("Beverly Hills Athletic Club", 87<quality>, beverlyHills)
      ("Silver Lake Fitness", 86<quality>, silverLake)
      ("Echo Park Gym", 92<quality>, echoPark)
      ("West Hollywood Health Club", 85<quality>, westHollywood)
      ("Los Feliz Fitness Center", 88<quality>, losFeliz)
      ("Pasadena Powerhouse Gym", 84<quality>, pasadena)
      ("Long Beach Fitness Studio", 86<quality>, longBeach)
      ("Malibu Beach Body Gym", 87<quality>, malibu)
      ("Studio City Strength", 90<quality>, studioCity) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "Los Angeles Medical Center"
            65<quality>
            Hospital
            roomGraph
            downtownLA

    World.City.addPlace place city

(* -------- Hotels --------- *)
let addHotels city =
    [ ("Los Angeles Grand Hotel", 98<quality>, 240m<dd>, downtownLA)
      ("Hollywood Luxury Suites", 95<quality>, 220m<dd>, hollywood)
      ("Santa Monica Beachfront Resort", 92<quality>, 210m<dd>, santaMonica)
      ("Beverly Hills Grand Residence", 90<quality>, 200m<dd>, beverlyHills)
      ("Silver Lake Boutique Inn", 88<quality>, 160m<dd>, silverLake)
      ("Echo Park Urban Hotel", 85<quality>, 180m<dd>, echoPark)
      ("West Hollywood Classic Hotel", 80<quality>, 450m<dd>, westHollywood)
      ("The Peninsula Los Angeles", 75<quality>, 140m<dd>, beverlyHills)
      ("The Wilshire Regency", 70<quality>, 120m<dd>, midWilshire)
      ("Malibu Beachfront Retreat", 65<quality>, 99m<dd>, malibu) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("LA Merch", downtownLA)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

(* -------- Rehearsal --------- *)
let addRehearsalSpaces city =
    [ ("LA Sound Studio", 85<quality>, 100m<dd>, downtownLA)
      ("Hollywood Jam Hub", 90<quality>, 150m<dd>, hollywood)
      ("Venice Beach Sound Waves", 92<quality>, 170m<dd>, veniceBeach)
      ("Silver Lake Rehearsal Room", 80<quality>, 50m<dd>, silverLake)
      ("Echo Park Sound Lab", 88<quality>, 120m<dd>, echoPark)
      ("West Hollywood Rehearsal Studio", 86<quality>, 110m<dd>, westHollywood)
      ("Los Feliz Music Space", 87<quality>, 120m<dd>, losFeliz)
      ("Studio City Sound Sanctuary", 85<quality>, 100m<dd>, studioCity)
      ("Downtown LA Soundstage", 89<quality>, 140m<dd>, downtownLA)
      ("Malibu Coastal Rehearsal", 90<quality>, 150m<dd>, malibu) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let addRestaurants city =
    [ ("Pasta Palace", 90<quality>, Italian, downtownLA)
      ("Taco Haven", 88<quality>, Mexican, hollywood)
      ("Sushi Zen", 85<quality>, Japanese, veniceBeach)
      ("Burger Bliss", 89<quality>, American, beverlyHills)
      ("Pho Fusion", 87<quality>, Vietnamese, silverLake)
      ("Pizza Paradise", 86<quality>, Italian, echoPark)
      ("French Elegance", 92<quality>, French, downtownLA)
      ("BBQ Bonanza", 91<quality>, American, westHollywood)
      ("Kebab Bliss", 86<quality>, Turkish, studioCity) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let addStudios city =
    [ ("Sunset Sound Studios",
       98<quality>,
       870m<dd>,
       hollywood,
       (Character.from
           "John Smith"
           Male
           (Shorthands.Winter 24<days> 1975<years>)))
      ("EastWest Studios",
       90<quality>,
       780m<dd>,
       hollywood,
       (Character.from
           "Eva Johnson"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("The Village Studios",
       92<quality>,
       800m<dd>,
       westLosAngeles,
       (Character.from "Tom Davis" Male (Shorthands.Summer 10<days> 1978<years>)))
      ("United Recording Studios",
       80<quality>,
       560m<dd>,
       hollywood,
       (Character.from
           "Jane Wilson"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("Ocean Way Recording",
       88<quality>,
       670m<dd>,
       santaMonica,
       (Character.from
           "Peter Brown"
           Male
           (Shorthands.Summer 20<days> 1981<years>)))
      ("Capitol Studios",
       86<quality>,
       680m<dd>,
       hollywood,
       (Character.from
           "Elisa Miller"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("The Echo Bar Studios",
       85<quality>,
       690m<dd>,
       echoPark,
       (Character.from
           "Alice Davis"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("Sunset Studio One",
       89<quality>,
       790m<dd>,
       hollywood,
       (Character.from
           "Martin Thompson"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Westlake Recording Studios",
       90<quality>,
       880m<dd>,
       westLosAngeles,
       (Character.from
           "Margaret Taylor"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Radio Studios --------- *)
let addRadioStudios city =
    [ ("KIIS-FM", 95<quality>, "Pop", hollywood)
      ("KLOS", 91<quality>, "Rock", downtownLA)
      ("KJAZZ 88.1 FM", 89<quality>, "Jazz", santaMonica) ]
    |> List.map (PlaceCreators.createRadioStudio city)
    |> List.fold (fun city place -> World.City.addPlace place city) city
