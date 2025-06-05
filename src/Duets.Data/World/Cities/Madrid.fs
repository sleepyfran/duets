module rec Duets.Data.World.Cities.Madrid

open Duets.Entities
open Duets.Entities.Calendar

let private sol = World.Zone.create "Sol"
let private salamanca = World.Zone.create "Salamanca"
let private chamberí = World.Zone.create "Chamberí"
let private chamartín = World.Zone.create "Chamartín"
let private tetuán = World.Zone.create "Tetuán"
let private retiro = World.Zone.create "Retiro"
let private arganzuela = World.Zone.create "Arganzuela"
let private moncloa = World.Zone.create "Moncloa"
let private latina = World.Zone.create "Latina"
let private carabanchel = World.Zone.create "Carabanchel"
let private usera = World.Zone.create "Usera"
let private puenteDeVallecas = World.Zone.create "Puente de Vallecas"
let private moratalaz = World.Zone.create "Moratalaz"
let private ciudadLineal = World.Zone.create "Ciudad Lineal"
let private hortaleza = World.Zone.create "Hortaleza"
let private barajas = World.Zone.create "Barajas"
let private lavapiés = World.Zone.create "Lavapiés"
let private malasaña = World.Zone.create "Malasaña"

/// Generates the city of Madrid.
let generate () =
    let createMadrid = World.City.create Madrid 1.8<costOfLiving> 1<utcOffset>

    createHome
    |> createMadrid
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
            "Aeropuerto Adolfo Suárez Madrid-Barajas"
            85<quality>
            Airport
            Layouts.airportLayout
            barajas

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("El Tigre", 85<quality>, latina)
      ("Mercado de San Miguel", 92<quality>, salamanca)
      ("Corral de la Morería", 90<quality>, lavapiés)
      ("1862 Dry Bar", 88<quality>, malasaña)
      ("Bodegas Rosell", 89<quality>, chamberí) ]
    |> List.map PlaceCreators.createBar
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Bookstores --------- *)
let private addBookstores city =
    [ ("La Central de Callao", 91<quality>, sol)
      ("Casa del Libro", 88<quality>, sol)
      ("Librería Desnivel", 85<quality>, malasaña)
      ("La Fábrica", 87<quality>, malasaña)
      ("Tipos Infames", 90<quality>, chamartín) ]
    |> List.map PlaceCreators.createBookstore
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Cafes --------- *)
let private addCafes city =
    [ ("Toma Café", 92<quality>, malasaña)
      ("HanSo Café", 90<quality>, lavapiés)
      ("Café de Oriente", 88<quality>, salamanca)
      ("La Bicicleta Café", 86<quality>, malasaña)
      ("Monkee Koffee", 89<quality>, chamberí) ]
    |> List.map PlaceCreators.createCafe
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Casinos --------- *)
let private addCasinos city =
    [ ("Madrid Majesty Casino", 92<quality>, sol)
      ("Salamanca Splendor Casino", 90<quality>, salamanca)
      ("Chamberí Charm Casino", 88<quality>, chamberí)
      ("Chamartín Chances Casino", 91<quality>, chamartín)
      ("Tetuán Treasures Casino", 89<quality>, tetuán) ]
    |> List.map PlaceCreators.createCasino
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Concert spaces --------- *)
let private addConcertSpaces city =
    [ ("Wizink Center",
       15000,
       salamanca,
       95<quality>,
       Layouts.concertSpaceLayout1)
      ("La Riviera", 2500, latina, 90<quality>, Layouts.concertSpaceLayout2)
      ("Teatro Real", 1700, sol, 98<quality>, Layouts.concertSpaceLayout3)
      ("Palacio de Vistalegre",
       15000,
       carabanchel,
       90<quality>,
       Layouts.concertSpaceLayout4)
      ("Teatro Barceló", 900, sol, 92<quality>, Layouts.concertSpaceLayout1)
      ("Moby Dick Club", 400, tetuán, 85<quality>, Layouts.concertSpaceLayout2)
      ("Joy Eslava", 800, sol, 90<quality>, Layouts.concertSpaceLayout3)
      ("Sala El Sol", 400, sol, 86<quality>, Layouts.concertSpaceLayout4)
      ("Sala But", 900, arganzuela, 88<quality>, Layouts.concertSpaceLayout1)
      ("Café La Palma", 300, sol, 80<quality>, Layouts.concertSpaceLayout2)
      ("Teatro Lara", 400, sol, 89<quality>, Layouts.concertSpaceLayout3)
      ("Sala Caracol", 500, arganzuela, 85<quality>, Layouts.concertSpaceLayout4)
      ("Galileo Galilei",
       600,
       chamberí,
       87<quality>,
       Layouts.concertSpaceLayout1)
      ("Café Berlín", 400, sol, 83<quality>, Layouts.concertSpaceLayout2)
      ("Sala Clamores", 500, chamberí, 84<quality>, Layouts.concertSpaceLayout3)
      ("Teatro de la Zarzuela",
       1300,
       sol,
       96<quality>,
       Layouts.concertSpaceLayout4)
      ("Sala Mon", 900, tetuán, 87<quality>, Layouts.concertSpaceLayout1)
      ("Teatro Nuevo Apolo", 1200, sol, 88<quality>, Layouts.concertSpaceLayout2)
      ("Wurlitzer Ballroom", 120, sol, 79<quality>, Layouts.concertSpaceLayout3)
      ("Costello Club", 300, sol, 82<quality>, Layouts.concertSpaceLayout4) ]
    |> List.map PlaceCreators.createConcertSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Home --------- *)
let createHome =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout retiro

(* -------- Gyms --------- *)
let private addGyms city =
    [ ("Gymage Lounge Resort", 90<quality>, sol)
      ("VivaGym Moncloa", 88<quality>, moncloa)
      ("Inacua Francisco Fernández Ochoa", 85<quality>, carabanchel)
      ("Holmes Place Chamartín", 89<quality>, chamartín)
      ("McFIT Aluche", 87<quality>, latina)
      ("Metropolitan", 86<quality>, moncloa)
      ("Arsenal Femenino Retiro", 92<quality>, retiro)
      ("Fisico Plaza Castilla", 85<quality>, tetuán)
      ("Basic-Fit Ciudad Lineal", 88<quality>, ciudadLineal)
      ("Curves Salamanca", 84<quality>, salamanca) ]
    |> List.map (PlaceCreators.createGym city)
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Hospital --------- *)
let private addHospital city =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "Hospital General Universitario Gregorio Marañón"
            85<quality>
            Hospital
            roomGraph
            retiro

    World.City.addPlace place city

(* -------- Hotels --------- *)
let private addHotels city =
    [ ("Hotel Ritz", 90<quality>, 200m<dd>, retiro)
      ("Hotel Villa Magna", 95<quality>, 220m<dd>, salamanca)
      ("Hotel Wellington", 88<quality>, 180m<dd>, retiro)
      ("Hotel Santo Mauro", 92<quality>, 210m<dd>, chamberí)
      ("Hotel Orfila", 85<quality>, 170m<dd>, chamberí)
      ("Hotel Urban", 80<quality>, 160m<dd>, sol)
      ("Hotel Hospes Puerta de Alcalá", 75<quality>, 140m<dd>, retiro)
      ("Hotel NH Collection Madrid Suecia", 70<quality>, 120m<dd>, sol)
      ("Hotel Ibis Madrid Centro", 65<quality>, 100m<dd>, sol)
      ("Hotel Room Mate Oscar", 60<quality>, 90m<dd>, sol) ]
    |> List.map PlaceCreators.createHotel
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Merchandise workshops -------- *)
let addMerchandiseWorkshops city =
    ("Madrid Merch", sol)
    |> PlaceCreators.createMerchandiseWorkshop
    |> World.City.addPlace' city

(* -------- Rehearsal spaces -------- *)
let private addRehearsalSpaces city =
    [ ("Ritmo y Compás", 90<quality>, 70m<dd>, tetuán)
      ("Rock Palace", 85<quality>, 60m<dd>, arganzuela)
      ("Sala de Ensayo Chamberí", 80<quality>, 50m<dd>, chamberí)
      ("Estudio de Ensayo Sol", 88<quality>, 66m<dd>, sol)
      ("Práctica Malasaña", 84<quality>, 58m<dd>, malasaña)
      ("Rehearsal Latina", 86<quality>, 62m<dd>, latina)
      ("Sala Moncloa", 87<quality>, 64m<dd>, moncloa)
      ("Ensayo Salamanca", 82<quality>, 54m<dd>, salamanca)
      ("Espacio de Práctica Retiro", 89<quality>, 68m<dd>, retiro)
      ("Tocar en Chamartín", 85<quality>, 60m<dd>, chamartín) ]
    |> List.map PlaceCreators.createRehearsalSpace
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Restaurants --------- *)
let private addRestaurants city =
    [ ("La Mesa Dorada", 90<quality>, Italian, chamberí)
      ("Le Petit Madrid", 88<quality>, French, malasaña)
      ("The Madrid Grill", 85<quality>, American, salamanca)
      ("El Sabor de Francia", 89<quality>, French, lavapiés)
      ("Tokyo Fusion", 87<quality>, Japanese, latina)
      ("Pho Madrid", 86<quality>, Vietnamese, arganzuela)
      ("Trattoria Bella Napoli", 92<quality>, Italian, retiro)
      ("Le Bistro Parisien", 91<quality>, French, sol)
      ("Burger City", 84<quality>, American, moncloa)
      ("Taco Loco", 88<quality>, Mexican, latina)
      ("Doner Kebab Malasaña", 63<quality>, Turkish, malasaña)
      ("Kebab Antalia", 73<quality>, Turkish, sol) ]
    |> List.map PlaceCreators.createRestaurant
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Studios --------- *)
let private addStudios city =
    [ ("Estudio Ángel",
       85<quality>,
       200m<dd>,
       chamberí,
       (Character.from
           "Carlos García"
           Male
           (Shorthands.Winter 24<days> 1975<years>)))
      ("Registros Madrileños",
       90<quality>,
       300m<dd>,
       salamanca,
       (Character.from
           "Ana Martínez"
           Female
           (Shorthands.Spring 15<days> 1980<years>)))
      ("Grabación El Retiro",
       92<quality>,
       340m<dd>,
       retiro,
       (Character.from
           "Fernando López"
           Male
           (Shorthands.Summer 10<days> 1978<years>)))
      ("Sonido Sol",
       80<quality>,
       100m<dd>,
       sol,
       (Character.from
           "Isabel Gómez"
           Female
           (Shorthands.Autumn 5<days> 1982<years>)))
      ("Malasaña Discos",
       88<quality>,
       260m<dd>,
       malasaña,
       (Character.from
           "Pedro Torres"
           Male
           (Shorthands.Summer 20<days> 1981<years>)))
      ("Mezcla Latina",
       86<quality>,
       220m<dd>,
       latina,
       (Character.from
           "Elena Sánchez"
           Female
           (Shorthands.Spring 1<days> 1990<years>)))
      ("Melodías Chamberí",
       87<quality>,
       240m<dd>,
       chamberí,
       (Character.from
           "Javier Pérez"
           Male
           (Shorthands.Winter 30<days> 1976<years>)))
      ("Sonido Moncloa",
       85<quality>,
       200m<dd>,
       moncloa,
       (Character.from
           "Patricia Díaz"
           Female
           (Shorthands.Summer 15<days> 1977<years>)))
      ("Tones de Tetuán",
       89<quality>,
       280m<dd>,
       tetuán,
       (Character.from
           "Ricardo Aguilera"
           Male
           (Shorthands.Winter 2<days> 1960<years>)))
      ("Armonía de Arganzuela",
       90<quality>,
       300m<dd>,
       arganzuela,
       (Character.from
           "María Fernández"
           Female
           (Shorthands.Spring 3<days> 1988<years>))) ]
    |> List.map PlaceCreators.createStudio
    |> List.fold (fun city place -> World.City.addPlace place city) city

(* -------- Radio Studios --------- *)
let private addRadioStudios city =
    [ ("Los40 Classic", 93<quality>, "Pop", sol)
      ("RockFM", 90<quality>, "Rock", moncloa)
      ("Radio Clásica", 88<quality>, "Jazz", retiro) ]
    |> List.map (PlaceCreators.createRadioStudio city)
    |> List.fold (fun city place -> World.City.addPlace place city) city
