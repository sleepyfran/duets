module rec Duets.Data.World.Cities.London

open Duets.Entities
open Duets.Entities.Calendar

let private soho = World.Zone.create "Soho"
let private coventGarden = World.Zone.create "Covent Garden"
let private mayfair = World.Zone.create "Mayfair"
let private shoreditch = World.Zone.create "Shoreditch"
let private camdenTown = World.Zone.create "Camden Town"
let private nottingHill = World.Zone.create "Notting Hill"
let private brixton = World.Zone.create "Brixton"
let private greenwich = World.Zone.create "Greenwich"
let private islington = World.Zone.create "Islington"
let private hackney = World.Zone.create "Hackney"
let private kensington = World.Zone.create "Kensington"
let private chelsea = World.Zone.create "Chelsea"
let private fulham = World.Zone.create "Fulham"
let private hammersmith = World.Zone.create "Hammersmith"
let private wandsworth = World.Zone.create "Wandsworth"
let private stratford = World.Zone.create "Stratford"
let private tottenham = World.Zone.create "Tottenham"
let private morden = World.Zone.create "Morden"
let private deptford = World.Zone.create "Deptford"
let private lambeth = World.Zone.create "Lambeth"

/// Generates the city of London.
let generate () =
    let createLondon = World.City.create London 4.0<costOfLiving> 0<utcOffset>

    createHome
    |> createLondon
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
            "Heathrow Airport"
            85<quality>
            Airport
            Layouts.airportLayout
            hammersmith

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("The Mayflower", 90<quality>, greenwich)
      ("Ye Olde Cheshire Cheese", 92<quality>, camdenTown)
      ("The Churchill Arms", 88<quality>, nottingHill)
      ("The Spaniards Inn", 86<quality>, camdenTown)
      ("The Dove", 94<quality>, hammersmith) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("Foyles", 92<quality>, greenwich)
      ("Daunt Books", 89<quality>, nottingHill)
      ("Persephone Books", 90<quality>, soho)
      ("Hatchards", 88<quality>, mayfair)
      ("Gay's The Word", 85<quality>, soho) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("Monmouth Coffee Company", 92<quality>, coventGarden)
      ("The Attendant", 90<quality>, shoreditch)
      ("Kaffeine", 88<quality>, mayfair)
      ("The Coffee Jar", 91<quality>, camdenTown)
      ("TAP Coffee", 89<quality>, soho) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("London Luxe Casino", 92<quality>, mayfair)
      ("Covent Garden Gamblers' Den", 90<quality>, coventGarden)
      ("Shoreditch Spin Palace", 88<quality>, shoreditch)
      ("Mayfair Magic Casino", 91<quality>, mayfair)
      ("Kensington Casino Royale", 89<quality>, kensington) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("The O2 Arena",
       20000,
       greenwich,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Royal Albert Hall",
       5272,
       kensington,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("Brixton Academy",
       4921,
       brixton,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Roundhouse", 1700, camdenTown, 89<quality>, Layouts.concertSpaceLayout4)
      ("Hammersmith Apollo",
       5000,
       hammersmith,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("Barbican Centre",
       1943,
       islington,
       80<quality>,
       Layouts.concertSpaceLayout1)
      ("Union Chapel", 900, islington, 86<quality>, Layouts.concertSpaceLayout3)
      ("Scala", 1145, camdenTown, 84<quality>, Layouts.concertSpaceLayout1)
      ("Bush Hall", 350, hammersmith, 82<quality>, Layouts.concertSpaceLayout2)
      ("The Forum", 2300, camdenTown, 90<quality>, Layouts.concertSpaceLayout3)
      ("Electric Ballroom",
       1100,
       camdenTown,
       80<quality>,
       Layouts.concertSpaceLayout4)
      ("The Underworld",
       500,
       camdenTown,
       83<quality>,
       Layouts.concertSpaceLayout1)
      ("The Jazz Cafe",
       420,
       camdenTown,
       87<quality>,
       Layouts.concertSpaceLayout4)
      ("The Garage", 600, islington, 88<quality>, Layouts.concertSpaceLayout2)
      ("The Borderline", 300, soho, 86<quality>, Layouts.concertSpaceLayout3)
      ("The 100 Club", 350, soho, 91<quality>, Layouts.concertSpaceLayout1)
      ("The Lexington", 200, islington, 92<quality>, Layouts.concertSpaceLayout3)
      ("The Dublin Castle",
       200,
       camdenTown,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("The Windmill Brixton",
       150,
       brixton,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("The Half Moon",
       220,
       wandsworth,
       90<quality>,
       Layouts.concertSpaceLayout1) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout soho

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Virgin Active Soho", 90<quality>, soho)
      ("PureGym Camden", 88<quality>, camdenTown)
      ("The Gym Group Kensington", 85<quality>, kensington)
      ("Nuffield Health Islington", 89<quality>, islington)
      ("Fitness First Lambeth", 87<quality>, lambeth)
      ("David Lloyd Fulham", 86<quality>, fulham)
      ("Better Gym Wandsworth", 92<quality>, wandsworth)
      ("Anytime Fitness Greenwich", 85<quality>, greenwich)
      ("GymBox Holborn", 88<quality>, soho)
      ("Bannatyne Health Club", 84<quality>, mayfair) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0
    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "St Thomas' Hospital"
            65<quality>
            Hospital
            roomGraph
            lambeth

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("The Savoy", 90<quality>, 200m<dd>, coventGarden)
      ("The Dorchester", 95<quality>, 220m<dd>, mayfair)
      ("The Ritz London", 98<quality>, 240m<dd>, mayfair)
      ("Claridge's", 92<quality>, 210m<dd>, mayfair)
      ("The Langham", 88<quality>, 190m<dd>, soho)
      ("The Connaught", 85<quality>, 180m<dd>, mayfair)
      ("The Goring", 80<quality>, 160m<dd>, camdenTown)
      ("The Bloomsbury", 75<quality>, 140m<dd>, camdenTown)
      ("The Zetter Townhouse", 70<quality>, 120m<dd>, shoreditch)
      ("The Hoxton", 65<quality>, 100m<dd>, shoreditch) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("London Merch", soho)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Premises Studios", 85<quality>, 100m<dd>, hackney)
      ("Pirate Studios", 90<quality>, 150m<dd>, greenwich)
      ("The Joint", 92<quality>, 170m<dd>, camdenTown)
      ("Bally Studios", 80<quality>, 50m<dd>, tottenham)
      ("Crown Lane Studio", 88<quality>, 120m<dd>, morden)
      ("The Music Complex", 86<quality>, 110m<dd>, deptford)
      ("Bush Studios", 87<quality>, 120m<dd>, camdenTown)
      ("New Rose Studios", 85<quality>, 100m<dd>, islington)
      ("John Henry's", 89<quality>, 140m<dd>, islington)
      ("Resident Studios", 90<quality>, 150m<dd>, shoreditch) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Dabbous", 90<quality>, French, soho)
      ("The Ledbury", 88<quality>, French, nottingHill)
      ("La Burratta", 85<quality>, Italian, chelsea)
      ("Barrafina", 89<quality>, Italian, soho)
      ("Yauatcha", 87<quality>, Japanese, soho)
      ("The Wolseley", 86<quality>, French, mayfair)
      ("Duck & Waffle", 92<quality>, American, camdenTown)
      ("Sketch", 91<quality>, French, mayfair)
      ("Burger & Lobster", 84<quality>, American, soho)
      ("Hawksmoor", 88<quality>, American, camdenTown)
      ("Istanbul Kebab House", 67<quality>, Turkish, islington)
      ("Turkish Delight", 86<quality>, Turkish, camdenTown) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("Abbey Road Studios",
       85<quality>,
       200m<dd>,
       camdenTown,
       (Character.from
           "George Martin"
           Male
           (Shorthands.Winter 3<days> 1926<years>)))
      ("AIR Studios",
       90<quality>,
       300m<dd>,
       camdenTown,
       (Character.from
           "Eva Johnson"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("Metropolis Studios",
       92<quality>,
       340m<dd>,
       tottenham,
       (Character.from "Tom Davis" Male (Shorthands.Summer 10<days> 1978<years>)))
      ("Sarm West Studios",
       80<quality>,
       100m<dd>,
       nottingHill,
       (Character.from
           "Jane Wilson"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("Strongroom",
       88<quality>,
       260m<dd>,
       shoreditch,
       (Character.from
           "Peter Brown"
           Male
           (Shorthands.Summer 20<days> 1981<years>)))
      ("The Church Studios",
       86<quality>,
       220m<dd>,
       camdenTown,
       (Character.from
           "Elisa Miller"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("Trident Studios",
       87<quality>,
       240m<dd>,
       soho,
       (Character.from
           "Martin Thompson"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Olympic Studios",
       85<quality>,
       200m<dd>,
       hackney,
       (Character.from
           "Alice Davis"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("The Pool",
       89<quality>,
       280m<dd>,
       hammersmith,
       (Character.from
           "Andrew Johnson"
           Male
           (Shorthands.Winter 2<days> 1960<years>)))
      ("Rak Studios",
       90<quality>,
       300m<dd>,
       stratford,
       (Character.from
           "Margaret Taylor"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Radio Studios --------- *)
let private addRadioStudios city =
    [ ("Capital FM", 92<quality>, "Pop", soho)
      ("Planet Rock", 89<quality>, "Rock", camdenTown)
      ("Jazz FM", 90<quality>, "Jazz", soho) ]
    |> List.map (PlaceCreators.createRadioStudio city)
    |> List.fold (fun city place -> World.City.addPlace place city) city
