module rec Duets.Data.World.Cities.Prague

open Duets.Entities
open Duets.Entities.Calendar

let private břevnov = World.Zone.create "Břevnov"
let private dejvice = World.Zone.create "Dejvice"
let private hradčany = World.Zone.create "Hradčany"
let private holešovice = World.Zone.create "Holešovice"
let private karlín = World.Zone.create "Karlín"
let private libeň = World.Zone.create "Libeň"
let private maláStrana = World.Zone.create "Malá Strana"
let private novéMěsto = World.Zone.create "Nové Město"
let private ruzyně = World.Zone.create "Ruzyně"
let private strašnice = World.Zone.create "Strašnice"
let private smíchov = World.Zone.create "Smíchov"
let private staréMěsto = World.Zone.create "Staré Město"
let private vinohrady = World.Zone.create "Vinohrady"
let private vršovice = World.Zone.create "Vršovice"
let private žižkov = World.Zone.create "Žižkov"

/// Generates the city of Prague.
let generate () =
    let createPrague = World.City.create Prague 1.6<costOfLiving> 1<utcOffset>

    createHome
    |> createPrague
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
    |> addRadioStudios
    |> addRehearsalSpaces
    |> addRestaurants
    |> addStudios

(* -------- Airport --------- *)
let addAirport city =
    let place =
        World.Place.create
            "Letiště Václava Havla Praha"
            85<quality>
            Airport
            Layouts.airportLayout
            ruzyně

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("Pivní Vepř", 90<quality>, dejvice)
      ("PourHouse", 92<quality>, vinohrady)
      ("Láhev Sud", 88<quality>, holešovice)
      ("Mug Mountain", 86<quality>, strašnice)
      ("Duchy Spodky", 94<quality>, žižkov) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("Knihkupectví Academia", 92<quality>, staréMěsto)
      ("Shakespeare a synové", 95<quality>, maláStrana)
      ("Palác Knih Luxor", 90<quality>, novéMěsto)
      ("Big Ben Bookshop", 88<quality>, vinohrady)
      ("Neoluxor", 91<quality>, smíchov) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("The Roasted Bean", 92<quality>, dejvice)
      ("Cup of Joy", 90<quality>, holešovice)
      ("Java Palace", 88<quality>, novéMěsto)
      ("Brew and Foam", 91<quality>, strašnice)
      ("Mug Harmony", 89<quality>, smíchov) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("Lucky Star Casino", 92<quality>, dejvice)
      ("Golden Dice Casino", 90<quality>, holešovice)
      ("High Stakes Haven", 88<quality>, novéMěsto)
      ("Fortuna's Palace", 91<quality>, strašnice)
      ("Royal Flush Resort", 89<quality>, smíchov) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Lucerna Music Bar",
       800,
       novéMěsto,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Palác Akropolis", 500, žižkov, 92<quality>, Layouts.concertSpaceLayout3)
      ("Rock Café", 350, novéMěsto, 88<quality>, Layouts.concertSpaceLayout2)
      ("Futurum Music Bar",
       650,
       smíchov,
       89<quality>,
       Layouts.concertSpaceLayout4)
      ("Jazz Dock", 150, smíchov, 95<quality>, Layouts.concertSpaceLayout2)
      ("Café v lese", 250, vršovice, 80<quality>, Layouts.concertSpaceLayout1)
      ("Cross Club", 400, holešovice, 86<quality>, Layouts.concertSpaceLayout3)
      ("Storm Club", 500, libeň, 84<quality>, Layouts.concertSpaceLayout1)
      ("Underdogs' Ballroom & Bar",
       200,
       smíchov,
       82<quality>,
       Layouts.concertSpaceLayout2)
      ("Roxy", 900, staréMěsto, 90<quality>, Layouts.concertSpaceLayout3)
      ("Retro Music Hall",
       1000,
       vinohrady,
       80<quality>,
       Layouts.concertSpaceLayout4)
      ("Klub 007 Strahov",
       250,
       břevnov,
       83<quality>,
       Layouts.concertSpaceLayout1)
      ("Royal Theatre", 300, vinohrady, 87<quality>, Layouts.concertSpaceLayout4)
      ("MeetFactory", 500, smíchov, 88<quality>, Layouts.concertSpaceLayout2)
      ("La Fabrica", 800, staréMěsto, 86<quality>, Layouts.concertSpaceLayout3)
      ("Forum Karlín", 3000, karlín, 91<quality>, Layouts.concertSpaceLayout1)
      ("O2 Universum", 4500, libeň, 92<quality>, Layouts.concertSpaceLayout3)
      ("Tipsport Arena",
       13000,
       holešovice,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("O2 Arena", 18000, libeň, 95<quality>, Layouts.concertSpaceLayout2)
      ("Eden Aréna", 21000, vršovice, 90<quality>, Layouts.concertSpaceLayout1) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Gyms --------- *)
let addGyms city =
    [ ("Sportovní Centrum Arel", 90<quality>, vinohrady)
      ("Fitpark Žižkov", 88<quality>, žižkov)
      ("Athletic Club Anděl", 85<quality>, smíchov)
      ("Praha Fitness Vinohrady", 89<quality>, vinohrady)
      ("Gym Palace Karlín", 86<quality>, karlín)
      ("PowerGym Hlavní nádraží", 92<quality>, novéMěsto)
      ("MaxiGym Holešovice", 85<quality>, holešovice)
      ("Forma Fitness Dejvice", 88<quality>, dejvice)
      ("IronWorks Smíchov", 84<quality>, smíchov) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout vinohrady

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "General University Hospital"
            65<quality>
            Hospital
            roomGraph
            novéMěsto

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("Hotel Praha", 60<quality>, 50m<dd>, dejvice)
      ("Hotel Slavia", 55<quality>, 40m<dd>, žižkov)
      ("Hotel Opatov", 65<quality>, 60m<dd>, strašnice)
      ("Hotel Don Giovanni", 75<quality>, 80m<dd>, vinohrady)
      ("Hotel Majestic Plaza", 80<quality>, 90m<dd>, novéMěsto)
      ("Hotel Duo", 70<quality>, 70m<dd>, holešovice)
      ("Hotel Ambassador", 90<quality>, 120m<dd>, staréMěsto)
      ("Hotel Savoy", 95<quality>, 150m<dd>, hradčany)
      ("Hotel Kings Court", 92<quality>, 130m<dd>, novéMěsto)
      ("Hotel Corinthia", 98<quality>, 180m<dd>, vinohrady) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("Prague Merch", staréMěsto)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

(* -------- Radio Studios --------- *)
let private addRadioStudios city =
    [ ("Evropa 2", 98<quality>, "Pop", novéMěsto)
      ("Radio Beat", 86<quality>, "Rock", žižkov)
      ("ČRo Jazz", 92<quality>, "Jazz", staréMěsto) ]
    |> List.map PlaceCreators.createRadioStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Hudební Sklep", 85<quality>, 100m<dd>, žižkov)
      ("Pokoje Prostor", 90<quality>, 150m<dd>, novéMěsto)
      ("Nahrávací Studio Anděl", 92<quality>, 170m<dd>, smíchov)
      ("Skleněná Louka", 80<quality>, 50m<dd>, vršovice)
      ("Místnost Prove", 88<quality>, 120m<dd>, holešovice)
      ("Zkušebna Křižík", 86<quality>, 110m<dd>, libeň)
      ("Rocková Nahrávka", 87<quality>, 120m<dd>, staréMěsto)
      ("Zvukový Štěstí", 85<quality>, 100m<dd>, vinohrady)
      ("Hudební Galerie", 89<quality>, 140m<dd>, karlín)
      ("Staroměstská Zkušebna", 90<quality>, 150m<dd>, staréMěsto) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("La Vita Bella", 90<quality>, Italian, vinohrady)
      ("Bistro Prostřeno", 88<quality>, French, žižkov)
      ("The Pražský Grill", 85<quality>, American, dejvice)
      ("El Sabor Mexicano", 89<quality>, Mexican, smíchov)
      ("Sushi Maki", 87<quality>, Japanese, novéMěsto)
      ("Pho Bo", 86<quality>, Vietnamese, novéMěsto)
      ("Trattoria Da Antonio", 92<quality>, Italian, holešovice)
      ("Le Petite Paris", 91<quality>, French, strašnice)
      ("Big Burger", 84<quality>, American, holešovice)
      ("Taco Fiesta", 88<quality>, Mexican, holešovice)
      ("Royal Kebab Grill", 67<quality>, Turkish, žižkov)
      ("Istanbul Kebab", 86<quality>, Turkish, staréMěsto) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("Anděl Studio",
       85<quality>,
       200m<dd>,
       smíchov,
       (Character.from "Jan Novák" Male (Shorthands.Winter 24<days> 1975<years>)))
      ("Pražské Záznamy",
       90<quality>,
       300m<dd>,
       novéMěsto,
       (Character.from
           "Eva Svobodová"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("Vyšehrad Nahrávání",
       92<quality>,
       340m<dd>,
       staréMěsto,
       (Character.from
           "Tomáš Dvořák"
           Male
           (Shorthands.Summer 10<days> 1978<years>)))
      ("Vinohradský Zvuk",
       80<quality>,
       100m<dd>,
       vinohrady,
       (Character.from
           "Jana Novotná"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("Vršovice Rekordy",
       88<quality>,
       260m<dd>,
       vršovice,
       (Character.from "Petr Čech" Male (Shorthands.Summer 20<days> 1981<years>)))
      ("Žižkov Mix",
       86<quality>,
       220m<dd>,
       žižkov,
       (Character.from
           "Eliška Křenková"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("Karlín Melodie",
       87<quality>,
       240m<dd>,
       karlín,
       (Character.from
           "Martin Václavík"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Smíchovský Zvuk",
       85<quality>,
       200m<dd>,
       smíchov,
       (Character.from
           "Alena Horáková"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("Holešovické Tóny",
       89<quality>,
       280m<dd>,
       holešovice,
       (Character.from
           "Ondřej Soukup"
           Male
           (Shorthands.Winter 2<days> 1960<years>)))
      ("Libeňská Harmonie",
       90<quality>,
       300m<dd>,
       libeň,
       (Character.from
           "Markéta Irglová"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city
