module rec Duets.Data.World.Cities.MexicoCity

open Duets.Entities
open Duets.Entities.Calendar

let private condesa = World.Zone.create "Condesa"
let private roma = World.Zone.create "Roma"
let private polanco = World.Zone.create "Polanco"
let private coyoacán = World.Zone.create "Coyoacán"
let private juarez = World.Zone.create "Juárez"
let private centro = World.Zone.create "Centro"
let private sanAngel = World.Zone.create "San Ángel"
let private santaFe = World.Zone.create "Santa Fe"
let private xochimilco = World.Zone.create "Xochimilco"
let private tlalpan = World.Zone.create "Tlalpan"
let private iztacalco = World.Zone.create "Iztacalco"
let private azcapotzalco = World.Zone.create "Azcapotzalco"
let private nápoles = World.Zone.create "Nápoles"
let private venustianoCarranza = World.Zone.create "Venustiano Carranza"
let private guerrero = World.Zone.create "Guerrero"
let private tabacalera = World.Zone.create "Tabacalera"
let private xoco = World.Zone.create "Xoco"

/// Generates the city of Mexico City.
let generate () =
    let createMexicoCity =
        World.City.create MexicoCity 1.5<costOfLiving> -6<utcOffset>

    createHome
    |> createMexicoCity
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
            "Benito Juárez International Airport"
            85<quality>
            Airport
            Layouts.airportLayout
            iztacalco

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("La Clandestina", 90<quality>, condesa)
      ("Pulquería Los Insurgentes", 92<quality>, roma)
      ("Bar Montejo", 88<quality>, centro)
      ("Cantina El Tio Pepe", 86<quality>, centro)
      ("La Opera Bar", 94<quality>, centro) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("Porrúa", 85<quality>, centro)
      ("El Péndulo Condesa", 90<quality>, condesa)
      ("Librería Rosario Castellanos", 95<quality>, condesa)
      ("Gandhi Mauricio Achar", 88<quality>, polanco)
      ("La Increíble Librería", 92<quality>, roma) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("Café Avellaneda", 92<quality>, coyoacán)
      ("Café Negro", 90<quality>, coyoacán)
      ("Café El Jarocho", 88<quality>, coyoacán)
      ("Cafébrería El Péndulo", 91<quality>, polanco)
      ("Café Toscano", 89<quality>, roma) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos -------- *)
let private addCasinos city =
    [ ("Condesa Casino Royale", 92<quality>, condesa)
      ("Roma Riches Casino", 90<quality>, roma)
      ("Polanco Prestige Casino", 88<quality>, polanco)
      ("Coyoacán Charm Casino", 91<quality>, coyoacán)
      ("Juárez Jackpot Casino", 89<quality>, juarez) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Palacio de los Deportes",
       20000,
       iztacalco,
       90<quality>,
       Layouts.concertSpaceLayout1)
      ("Foro Sol", 65000, iztacalco, 92<quality>, Layouts.concertSpaceLayout3)
      ("Auditorio Nacional",
       10000,
       polanco,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Arena Ciudad de México",
       22500,
       azcapotzalco,
       89<quality>,
       Layouts.concertSpaceLayout4)
      ("Teatro Metropolitan",
       3142,
       centro,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("El Plaza Condesa",
       1950,
       condesa,
       80<quality>,
       Layouts.concertSpaceLayout1)
      ("Pepsi Center WTC",
       7000,
       nápoles,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("Teatro de la Ciudad Esperanza Iris",
       1300,
       centro,
       84<quality>,
       Layouts.concertSpaceLayout1)
      ("Foro Indie Rocks", 500, roma, 82<quality>, Layouts.concertSpaceLayout2)
      ("Lunario del Auditorio Nacional",
       1000,
       polanco,
       90<quality>,
       Layouts.concertSpaceLayout3)
      ("Circo Volador",
       1500,
       venustianoCarranza,
       80<quality>,
       Layouts.concertSpaceLayout4)
      ("Foro Alicia", 250, roma, 83<quality>, Layouts.concertSpaceLayout1)
      ("Bajo Circuito", 400, condesa, 87<quality>, Layouts.concertSpaceLayout4)
      ("Salón Los Angeles",
       500,
       guerrero,
       88<quality>,
       Layouts.concertSpaceLayout2)
      ("Frontón México",
       1800,
       tabacalera,
       86<quality>,
       Layouts.concertSpaceLayout3)
      ("Teatro Ángela Peralta",
       1400,
       polanco,
       91<quality>,
       Layouts.concertSpaceLayout1)
      ("Centro Cultural Roberto Cantoral",
       850,
       xoco,
       92<quality>,
       Layouts.concertSpaceLayout3)
      ("Foro Blackberry",
       3200,
       condesa,
       88<quality>,
       Layouts.concertSpaceLayout4)
      ("Teatro de la Danza",
       472,
       centro,
       95<quality>,
       Layouts.concertSpaceLayout2)
      ("Teatro Julio Castillo",
       499,
       polanco,
       90<quality>,
       Layouts.concertSpaceLayout1) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout condesa

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Sportium Miguel Ángel de Quevedo", 90<quality>, coyoacán)
      ("Smart Fit Polanco", 88<quality>, polanco)
      ("Gimnasio Altitude", 85<quality>, roma)
      ("Gimnasio Atlas", 89<quality>, iztacalco)
      ("Total Gym", 87<quality>, juarez)
      ("F45 Roma", 86<quality>, roma)
      ("Gimnasio Activo 20-30", 92<quality>, azcapotzalco)
      ("Body Factory Tlalpan", 85<quality>, tlalpan)
      ("Sports World", 88<quality>, guerrero)
      ("Gym Power Club", 84<quality>, tabacalera) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city


(* -------- Hospital --------- *)
let addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "Hospital General de México"
            65<quality>
            Hospital
            roomGraph
            juarez

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("The St. Regis Mexico City", 98<quality>, 240m<dd>, polanco)
      ("Four Seasons Hotel Mexico City", 95<quality>, 220m<dd>, juarez)
      ("Gran Hotel Ciudad de México", 92<quality>, 210m<dd>, centro)
      ("Las Alcobas", 90<quality>, 200m<dd>, polanco)
      ("Hotel Condesa DF", 88<quality>, 190m<dd>, condesa)
      ("Hotel Carlota", 85<quality>, 180m<dd>, coyoacán)
      ("Hotel Habita", 80<quality>, 160m<dd>, polanco)
      ("Camino Real Polanco", 75<quality>, 140m<dd>, polanco)
      ("Hotel Zocalo Central", 70<quality>, 120m<dd>, centro)
      ("Hotel Geneve", 65<quality>, 100m<dd>, juarez) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("Mexico City Merch", centro)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

(* -------- Rehearsal spaces --------- *)
let private addRehearsalSpaces city =
    [ ("Sala Chopin", 85<quality>, 100m<dd>, roma)
      ("Estudio 13", 90<quality>, 150m<dd>, polanco)
      ("Sala de Ensayo Coyoacán", 92<quality>, 170m<dd>, coyoacán)
      ("Estudio 19", 80<quality>, 50m<dd>, sanAngel)
      ("Sala de Ensayo Santa Fe", 88<quality>, 120m<dd>, santaFe)
      ("Estudio 5", 86<quality>, 110m<dd>, centro)
      ("Estudio 7", 87<quality>, 120m<dd>, juarez)
      ("Sala de Ensayo Condesa", 85<quality>, 100m<dd>, condesa)
      ("Estudio 11", 89<quality>, 140m<dd>, polanco)
      ("Sala de Ensayo Roma", 90<quality>, 150m<dd>, roma) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("Pujol", 90<quality>, Mexican, polanco)
      ("Contramar", 88<quality>, Italian, roma)
      ("El Cardenal", 85<quality>, Mexican, centro)
      ("Quintonil", 89<quality>, Mexican, polanco)
      ("Biko", 87<quality>, Japanese, polanco)
      ("Sud 777", 86<quality>, Mexican, tabacalera)
      ("Rosetta", 92<quality>, Italian, roma)
      ("Maximo Bistrot", 91<quality>, French, roma)
      ("La Docena Oyster Bar & Grill", 84<quality>, French, polanco)
      ("Los Panchos", 88<quality>, Mexican, coyoacán)
      ("Krux Doner Kebab", 83<quality>, Turkish, tabacalera)
      ("Kebab Antalia", 73<quality>, Turkish, roma) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("Estudio Polanco",
       85<quality>,
       200m<dd>,
       polanco,
       (Character.from
           "Juan Pérez"
           Male
           (Shorthands.Winter 24<days> 1975<years>)))
      ("Sonido Condesa",
       90<quality>,
       300m<dd>,
       condesa,
       (Character.from
           "Eva González"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("Audio Azcapotzalco",
       92<quality>,
       340m<dd>,
       azcapotzalco,
       (Character.from
           "Tomás Méndez"
           Male
           (Shorthands.Summer 10<days> 1978<years>)))
      ("Ritmo Roma",
       80<quality>,
       100m<dd>,
       roma,
       (Character.from
           "Ana Martínez"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("Vibración Venustiano",
       88<quality>,
       260m<dd>,
       venustianoCarranza,
       (Character.from
           "Pedro Gómez"
           Male
           (Shorthands.Summer 20<days> 1981<years>)))
      ("Harmonía Guerrero",
       86<quality>,
       220m<dd>,
       guerrero,
       (Character.from
           "Elisa Ramírez"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("Tabacalera Tunes",
       85<quality>,
       200m<dd>,
       tabacalera,
       (Character.from
           "Carlos López"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Xoco Xound",
       89<quality>,
       280m<dd>,
       xoco,
       (Character.from
           "María García"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("Iztacalco Inspiración",
       90<quality>,
       300m<dd>,
       iztacalco,
       (Character.from
           "Luisa Fernández"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Radio Studios --------- *)
let private addRadioStudios city =
    [ ("Los 40 México", 92<quality>, "Pop", condesa)
      ("Reactor 105.7 FM", 88<quality>, "Rock", roma)
      ("Horizonte 107.9 FM", 89<quality>, "Jazz", coyoacán) ]
    |> List.map PlaceCreators.createRadioStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city
