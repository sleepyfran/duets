module rec Duets.Data.World.Cities.Prague

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let private břevnov = World.Zone.create "Břevnov"
let private dejvice = World.Zone.create "Dejvice"
let private holešovice = World.Zone.create "Holešovice"
let private karlín = World.Zone.create "Karlín"
let private libeň = World.Zone.create "Libeň"
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
    let createPrague = World.City.create Prague 1.6

    createHome
    |> createPrague
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
            "Letiště Václava Havla Praha"
            85<quality>
            Airport
            Everywhere.Common.airportLayout
            ruzyně

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("Pivní Vepř", 90<quality>, dejvice)
      ("PourHouse", 92<quality>, vinohrady)
      ("Láhev Sud", 88<quality>, holešovice)
      ("Mug Mountain", 86<quality>, strašnice)
      ("Duchy Spodky", 94<quality>, žižkov) ]
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
    [ ("The Roasted Bean", 92<quality>, dejvice)
      ("Cup of Joy", 90<quality>, holešovice)
      ("Java Palace", 88<quality>, novéMěsto)
      ("Brew and Foam", 91<quality>, strašnice)
      ("Mug Harmony", 89<quality>, smíchov) ]
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
    [ ("Lucerna Music Bar",
       800,
       novéMěsto,
       90<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Palác Akropolis",
       500,
       žižkov,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Rock Café",
       350,
       novéMěsto,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Futurum Music Bar",
       650,
       smíchov,
       89<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Jazz Dock",
       150,
       smíchov,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Café v lese",
       250,
       vršovice,
       80<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Cross Club",
       400,
       holešovice,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Storm Club",
       500,
       libeň,
       84<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Underdogs' Ballroom & Bar",
       200,
       smíchov,
       82<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Roxy",
       900,
       staréMěsto,
       90<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Retro Music Hall",
       1000,
       vinohrady,
       80<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Klub 007 Strahov",
       250,
       břevnov,
       83<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Royal Theatre",
       300,
       vinohrady,
       87<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("MeetFactory",
       500,
       smíchov,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("La Fabrica",
       800,
       staréMěsto,
       86<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Forum Karlín",
       3000,
       karlín,
       91<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("O2 Universum",
       4500,
       libeň,
       92<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Tipsport Arena",
       13000,
       holešovice,
       88<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("O2 Arena",
       18000,
       libeň,
       95<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Eden Aréna",
       21000,
       vršovice,
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
        vinohrady

(* -------- Hospital --------- *)
let addHospital city =
    let lobby = World.Node.create 0 RoomType.Lobby

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "General University Hospital"
            65<quality>
            Hospital
            roomGraph
            novéMěsto

    World.City.addPlace place city

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
    [ ("La Vita Bella", 90<quality>, Italian, vinohrady)
      ("Bistro Prostřeno", 88<quality>, French, žižkov)
      ("The Pražský Grill", 85<quality>, American, dejvice)
      ("El Sabor Mexicano", 89<quality>, Mexican, smíchov)
      ("Sushi Maki", 87<quality>, Japanese, novéMěsto)
      ("Pho Bo", 86<quality>, Vietnamese, novéMěsto)
      ("Trattoria Da Antonio", 92<quality>, Italian, holešovice)
      ("Le Petite Paris", 91<quality>, French, strašnice)
      ("Big Burger", 84<quality>, American, holešovice)
      ("Taco Fiesta", 88<quality>, Mexican, holešovice) ]
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
    [ ("Anděl Studio",
       85<quality>,
       200m<dd>,
       smíchov,
       (Character.from "Jan Novák" Male (December 24 1975)))
      ("Pražské Záznamy",
       90<quality>,
       300m<dd>,
       novéMěsto,
       (Character.from "Eva Svobodová" Female (March 15 1980)))
      ("Vyšehrad Nahrávání",
       92<quality>,
       340m<dd>,
       staréMěsto,
       (Character.from "Tomáš Dvořák" Male (July 10 1978)))
      ("Vinohradský Zvuk",
       80<quality>,
       100m<dd>,
       vinohrady,
       (Character.from "Jana Novotná" Female (September 5 1982)))
      ("Vršovice Rekordy",
       88<quality>,
       260m<dd>,
       vršovice,
       (Character.from "Petr Čech" Male (June 20 1981)))
      ("Žižkov Mix",
       86<quality>,
       220m<dd>,
       žižkov,
       (Character.from "Eliška Křenková" Female (April 1 1990)))
      ("Karlín Melodie",
       87<quality>,
       240m<dd>,
       karlín,
       (Character.from "Martin Václavík" Male (January 30 1976)))
      ("Smíchovský Zvuk",
       85<quality>,
       200m<dd>,
       smíchov,
       (Character.from "Alena Horáková" Female (August 15 1977)))
      ("Holešovické Tóny",
       89<quality>,
       280m<dd>,
       holešovice,
       (Character.from "Ondřej Soukup" Male (February 2 1960)))
      ("Libeňská Harmonie",
       90<quality>,
       300m<dd>,
       libeň,
       (Character.from "Markéta Irglová" Female (May 3 1988))) ]
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
