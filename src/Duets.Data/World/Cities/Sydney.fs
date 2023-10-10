module rec Duets.Data.World.Cities.Sydney

open Fugit.Months
open Duets.Entities

let private cbd = World.Zone.create "CBD"
let private surryHills = World.Zone.create "Surry Hills"
let private newtown = World.Zone.create "Newtown"
let private bondi = World.Zone.create "Bondi"
let private darlinghurst = World.Zone.create "Darlinghurst"
let private parramatta = World.Zone.create "Parramatta"
let private pottsPoint = World.Zone.create "Potts Point"

/// Generates the city of Sydney.
let generate () =
    let createSydney = World.City.create Sydney 3.4<costOfLiving> 9<utcOffset>

    createHome
    |> createSydney
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
            "Sydney Kingsford Smith Airport"
            85<quality>
            Airport
            Layouts.airportLayout
            cbd

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("Opera Bar", 90<quality>, cbd)
      ("The Baxter Inn", 92<quality>, cbd)
      ("Shady Pines Saloon", 88<quality>, darlinghurst)
      ("The Wild Rover", 86<quality>, surryHills)
      ("Earl's Juke Joint", 94<quality>, newtown) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("The Grounds of Alexandria", 92<quality>, newtown)
      ("Reuben Hills", 90<quality>, surryHills)
      ("Single O", 88<quality>, surryHills)
      ("Paramount Coffee Project", 91<quality>, surryHills)
      ("Brewtown Newtown", 89<quality>, newtown) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("Sydney Central Casino", 92<quality>, cbd)
      ("Surry Hills Spectacle Casino", 90<quality>, surryHills)
      ("Newtown Nexus Casino", 88<quality>, newtown)
      ("Bondi Beach Bliss Casino", 91<quality>, bondi)
      ("Darlinghurst Delight Casino", 89<quality>, darlinghurst) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Sydney Opera House", 5700, cbd, 90<quality>, Layouts.concertSpaceLayout1)
      ("Enmore Theatre", 2500, newtown, 92<quality>, Layouts.concertSpaceLayout3)
      ("The Metro Theatre", 1200, cbd, 88<quality>, Layouts.concertSpaceLayout2)
      ("The Factory Theatre",
       800,
       newtown,
       89<quality>,
       Layouts.concertSpaceLayout4)
      ("The Basement", 600, cbd, 95<quality>, Layouts.concertSpaceLayout2)
      ("The State Theatre", 2000, cbd, 90<quality>, Layouts.concertSpaceLayout1)
      ("The Hordern Pavilion",
       5500,
       cbd,
       88<quality>,
       Layouts.concertSpaceLayout3)
      ("The Roundhouse", 2200, cbd, 87<quality>, Layouts.concertSpaceLayout2)
      ("The Bald Faced Stag", 500, cbd, 86<quality>, Layouts.concertSpaceLayout4)
      ("The Lansdowne", 800, cbd, 85<quality>, Layouts.concertSpaceLayout1)
      ("The Vanguard", 150, newtown, 84<quality>, Layouts.concertSpaceLayout2)
      ("The Bridge Hotel", 1000, cbd, 83<quality>, Layouts.concertSpaceLayout3)
      ("The Manning Bar", 1200, cbd, 82<quality>, Layouts.concertSpaceLayout4)
      ("The Oxford Art Factory",
       500,
       cbd,
       81<quality>,
       Layouts.concertSpaceLayout1)
      ("The Hi-Fi", 1500, cbd, 80<quality>, Layouts.concertSpaceLayout2)
      ("The Annandale Hotel", 400, cbd, 79<quality>, Layouts.concertSpaceLayout3)
      ("The Sandringham Hotel",
       300,
       newtown,
       78<quality>,
       Layouts.concertSpaceLayout4)
      ("The Gaelic Club", 800, cbd, 77<quality>, Layouts.concertSpaceLayout1)
      ("The Marquee", 2000, cbd, 76<quality>, Layouts.concertSpaceLayout2)
      ("The Qantas Credit Union Arena",
       18000,
       cbd,
       75<quality>,
       Layouts.concertSpaceLayout3) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Fitness First", 90<quality>, cbd)
      ("Anytime Fitness Darlinghurst", 88<quality>, darlinghurst)
      ("Sydney Gym Parramatta", 85<quality>, parramatta)
      ("24/7 Fitness Newtown", 89<quality>, newtown)
      ("SweatBox Surry Hills", 87<quality>, surryHills)
      ("PowerFit", 86<quality>, parramatta)
      ("FlexGym", 92<quality>, cbd)
      ("Urban Athlete", 85<quality>, cbd)
      ("PumpUp Gym Bondi", 88<quality>, bondi)
      ("J-Strong Liverpool", 84<quality>, pottsPoint) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout surryHills

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create "Sydney Hospital" 65<quality> Hospital roomGraph cbd

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("Park Hyatt Sydney", 98<quality>, 240m<dd>, cbd)
      ("The Langham Sydney", 95<quality>, 220m<dd>, cbd)
      ("Shangri-La Hotel Sydney", 92<quality>, 210m<dd>, cbd)
      ("InterContinental Sydney", 90<quality>, 200m<dd>, cbd)
      ("Four Seasons Hotel Sydney", 88<quality>, 190m<dd>, cbd)
      ("The Westin Sydney", 85<quality>, 180m<dd>, cbd)
      ("Sofitel Sydney Darling Harbour", 80<quality>, 160m<dd>, newtown)
      ("Hyatt Regency Sydney", 75<quality>, 140m<dd>, cbd)
      ("Novotel Sydney on Darling Harbour", 70<quality>, 120m<dd>, surryHills)
      ("Ibis Sydney Darling Harbour", 65<quality>, 100m<dd>, darlinghurst) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("JamHub", 85<quality>, 100m<dd>, newtown)
      ("Surry Hills Studios", 90<quality>, 150m<dd>, surryHills)
      ("Rock Central", 92<quality>, 170m<dd>, darlinghurst)
      ("Sound Hub", 80<quality>, 50m<dd>, cbd)
      ("The Music Space", 88<quality>, 120m<dd>, parramatta) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Quay", 90<quality>, Italian, cbd)
      ("Tetsuya's", 88<quality>, Japanese, cbd)
      ("Rockpool Bar & Grill", 85<quality>, American, cbd)
      ("El Loco", 89<quality>, Mexican, surryHills)
      ("Sake Restaurant & Bar", 87<quality>, Japanese, cbd)
      ("Red Lantern", 86<quality>, Vietnamese, darlinghurst)
      ("Pilu at Freshwater", 92<quality>, Italian, bondi)
      ("Bistro Rex", 91<quality>, French, pottsPoint)
      ("Burger Project", 84<quality>, American, cbd)
      ("El Camino Cantina", 88<quality>, Mexican, cbd) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("301 Studios",
       85<quality>,
       200m<dd>,
       cbd,
       (Character.from "Daniel Johns" Male (April 22 1979)))
      ("Australian Studios",
       90<quality>,
       300m<dd>,
       cbd,
       (Character.from "Sia Furler" Female (December 18 1975)))
      ("The Grove Studios",
       92<quality>,
       340m<dd>,
       cbd,
       (Character.from "Keith Urban" Male (October 26 1967)))
      ("BJB Studios",
       80<quality>,
       100m<dd>,
       surryHills,
       (Character.from "Nick Cave" Male (September 22 1957)))
      ("Electric Avenue Studios",
       88<quality>,
       260m<dd>,
       cbd,
       (Character.from "Kylie Minogue" Female (May 28 1968))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city
