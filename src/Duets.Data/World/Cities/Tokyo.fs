module rec Duets.Data.World.Cities.Tokyo

open Fugit.Months
open Duets.Entities

let private chuo = World.Zone.create "Chuo"
let private minato = World.Zone.create "Minato"
let private shinjuku = World.Zone.create "Shinjuku"
let private shibuya = World.Zone.create "Shibuya"
let private ota = World.Zone.create "Ota"
let private bunkyo = World.Zone.create "Bunkyo"
let private taito = World.Zone.create "Taito"
let private nakano = World.Zone.create "Nakano"
let private chiyoda = World.Zone.create "Chiyoda"
let private shinagawa = World.Zone.create "Shinagawa"

/// Generates the city of Tokyo.
let generate () =
    let createTokyo = World.City.create Tokyo 3.4<costOfLiving> 9<utcOffset>

    createHome
    |> createTokyo
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
            "Haneda Airport"
            85<quality>
            Airport
            Layouts.airportLayout
            ota

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("Golden Gai", 90<quality>, shinjuku)
      ("High Five", 92<quality>, chuo)
      ("New York Stand", 88<quality>, shinjuku)
      ("Ben Fiddich", 86<quality>, shinjuku)
      ("Trench", 94<quality>, shibuya) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("L'Ambre", 92<quality>, chuo)
      ("Fuglen", 90<quality>, shibuya)
      ("Blue Bottle", 88<quality>, shinjuku)
      ("Streamer", 91<quality>, shibuya)
      ("Kitsune", 89<quality>, minato) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("Chuo Casino Royale", 92<quality>, chuo)
      ("Minato Magic Casino", 90<quality>, minato)
      ("Shinjuku Spin Palace", 88<quality>, shinjuku)
      ("Shibuya Splendor Casino", 91<quality>, shibuya)
      ("Ota Oasis Casino", 89<quality>, ota) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Tokyo Dome",
       55000,
       bunkyo,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Nippon Budokan",
       14471,
       chuo,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("Zepp Tokyo",
       2712,
       minato,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Shinjuku Loft",
       500,
       shinjuku,
       89<quality>,
       Layouts.concertSpaceLayout4)
      ("Blue Note Tokyo",
       300,
       minato,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("Liquidroom",
       900,
       shibuya,
       80<quality>,
       Layouts.concertSpaceLayout1)
      ("Club Quattro",
       700,
       shibuya,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("Akasaka Blitz",
       1300,
       minato,
       84<quality>,
       Layouts.concertSpaceLayout1)
      ("Unit", 650, shibuya, 82<quality>, Layouts.concertSpaceLayout2)
      ("Shibuya O-East",
       1300,
       shibuya,
       90<quality>,
       Layouts.concertSpaceLayout3)
      ("Shibuya O-West",
       600,
       shibuya,
       80<quality>,
       Layouts.concertSpaceLayout4)
      ("Shibuya O-Nest",
       300,
       shibuya,
       83<quality>,
       Layouts.concertSpaceLayout1)
      ("Shibuya O-Crest",
       500,
       shibuya,
       87<quality>,
       Layouts.concertSpaceLayout4)
      ("Shibuya WWW",
       600,
       shibuya,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Shibuya WWW X",
       300,
       shibuya,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("Shibuya Club Asia",
       500,
       shibuya,
       91<quality>,
       Layouts.concertSpaceLayout1)
      ("Shibuya Milkyway",
       200,
       shibuya,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("Shibuya Star Lounge",
       200,
       shibuya,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("Shibuya Glad",
       200,
       shibuya,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("Shibuya Lush",
       200,
       shibuya,
       90<quality>,
       Layouts.concertSpaceLayout1) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Gold's Gym Shibuya", 90<quality>, shibuya)
      ("Anytime Fitness Shinjuku", 88<quality>, shinjuku)
      ("Tokyo Fitness Roppongi", 85<quality>, minato)
      ("24/7 Fitness", 89<quality>, shibuya)
      ("Muscle House Taito", 87<quality>, taito)
      ("PowerFit Chiyoda", 86<quality>, chiyoda)
      ("FlexGym", 92<quality>, nakano)
      ("Urban Athlete Bunkyo", 85<quality>, bunkyo)
      ("PumpUp Gym", 88<quality>, shinagawa)
      ("J-Strong Adachi", 84<quality>, chiyoda) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Home --------- *)
let createHome =
    World.Place.create
        "Home"
        100<quality>
        Home
        Layouts.homeLayout
        shinjuku

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "Tokyo Metropolitan Komagome Hospital"
            65<quality>
            Hospital
            roomGraph
            bunkyo

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("The Ritz-Carlton Tokyo", 98<quality>, 240m<dd>, minato)
      ("Aman Tokyo", 95<quality>, 220m<dd>, chiyoda)
      ("The Peninsula Tokyo", 92<quality>, 210m<dd>, chiyoda)
      ("Park Hyatt Tokyo", 90<quality>, 200m<dd>, shinjuku)
      ("Mandarin Oriental Tokyo", 88<quality>, 190m<dd>, chuo)
      ("The Prince Park Tower Tokyo", 85<quality>, 180m<dd>, minato)
      ("The Tokyo Station Hotel", 80<quality>, 160m<dd>, chiyoda)
      ("Hotel New Otani Tokyo", 75<quality>, 140m<dd>, chiyoda)
      ("Hotel Okura Tokyo", 70<quality>, 120m<dd>, minato)
      ("Shinjuku Granbell Hotel", 65<quality>, 100m<dd>, shinjuku) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Tanta Space", 85<quality>, 100m<dd>, shibuya)
      ("NOAH Room", 90<quality>, 150m<dd>, chuo)
      ("Bayd Spot", 92<quality>, 170m<dd>, minato)
      ("246 Area", 80<quality>, 50m<dd>, chuo)
      ("Plan-B Zone", 88<quality>, 120m<dd>, shinjuku)
      ("Penta Place", 86<quality>, 110m<dd>, ota)
      ("246 Spot", 87<quality>, 120m<dd>, chuo)
      ("Z'd Space", 85<quality>, 100m<dd>, shinjuku)
      ("Sound Dali", 89<quality>, 140m<dd>, nakano)
      ("Watts Room", 90<quality>, 150m<dd>, shibuya) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Sukiyabashi Jiro", 90<quality>, Japanese, chuo)
      ("Le Sputnik", 88<quality>, French, minato)
      ("The Oak Door", 85<quality>, American, minato)
      ("El Torito", 89<quality>, Mexican, shinjuku)
      ("Tsuta", 87<quality>, Japanese, taito)
      ("Pho Dragon", 86<quality>, Vietnamese, shinjuku)
      ("Elio Locanda Italiana", 92<quality>, Italian, chiyoda)
      ("Le Petit Bedon", 91<quality>, French, shinagawa)
      ("The Great Burger", 84<quality>, American, shibuya)
      ("El Quixico", 88<quality>, Mexican, minato) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("Sound City",
       85<quality>,
       200m<dd>,
       shinjuku,
       (Character.from "Tetsuya Komuro" Male (November 27 1958)))
      ("Victor Studio",
       90<quality>,
       300m<dd>,
       chuo,
       (Character.from "Yoshiki Hayashi" Male (November 20 1965)))
      ("King Records",
       92<quality>,
       340m<dd>,
       shibuya,
       (Character.from "Haruki Komuro" Male (November 27 1958)))
      ("Sony Music Studios",
       80<quality>,
       100m<dd>,
       minato,
       (Character.from "Hikaru Utada" Female (January 19 1983)))
      ("Warner Music Recording Studio",
       88<quality>,
       260m<dd>,
       chuo,
       (Character.from "Kazutoshi Sakurai" Male (March 8 1970)))
      ("Universal Music Studios",
       86<quality>,
       220m<dd>,
       minato,
       (Character.from "Tak Matsumoto" Male (March 27 1961)))
      ("Avex Trax Studios",
       87<quality>,
       240m<dd>,
       shibuya,
       (Character.from "Max Matsuura" Male (January 1 1964)))
      ("Sunrise Music Studio",
       85<quality>,
       200m<dd>,
       shinjuku,
       (Character.from "Ayumi Hamasaki" Female (October 2 1978)))
      ("Epic Records Studio",
       89<quality>,
       280m<dd>,
       chuo,
       (Character.from "Masaharu Fukuyama" Male (February 6 1969)))
      ("Zapuni Studio",
       90<quality>,
       300m<dd>,
       shibuya,
       (Character.from "Ryuichi Sakamoto" Male (January 17 1952))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city
