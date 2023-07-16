module rec Duets.Data.World.Cities.Madrid.Root

open Fugit.Months
open Duets.Entities
open Duets.Data.World

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
    let createMadrid = World.City.create Madrid 4.7

    createHome
    |> createMadrid
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
            "Aeropuerto Adolfo Suárez Madrid-Barajas"
            85<quality>
            Airport
            Everywhere.Common.airportLayout
            barajas

    World.City.addPlace place city

(* -------- Bars --------- *)
let private addBars city =
    [ ("El Tigre", 85<quality>, latina)
      ("Mercado de San Miguel", 92<quality>, salamanca)
      ("Corral de la Morería", 90<quality>, lavapiés)
      ("1862 Dry Bar", 88<quality>, malasaña)
      ("Bodegas Rosell", 89<quality>, chamberí) ]
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
    [ ("Toma Café", 92<quality>, malasaña)
      ("HanSo Café", 90<quality>, lavapiés)
      ("Café de Oriente", 88<quality>, salamanca)
      ("La Bicicleta Café", 86<quality>, malasaña)
      ("Monkee Koffee", 89<quality>, chamberí) ]
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
    [ ("Wizink Center",
       15000,
       salamanca,
       95<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("La Riviera",
       2500,
       latina,
       90<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Teatro Real",
       1700,
       sol,
       98<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Palacio de Vistalegre",
       15000,
       carabanchel,
       90<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Teatro Barceló",
       900,
       sol,
       92<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Moby Dick Club",
       400,
       tetuán,
       85<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Joy Eslava",
       800,
       sol,
       90<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Sala El Sol",
       400,
       sol,
       86<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Sala But",
       900,
       arganzuela,
       88<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Café La Palma",
       300,
       sol,
       80<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Teatro Lara",
       400,
       sol,
       89<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Sala Caracol",
       500,
       arganzuela,
       85<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Galileo Galilei",
       600,
       chamberí,
       87<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Café Berlín",
       400,
       sol,
       83<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Sala Clamores",
       500,
       chamberí,
       84<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Teatro de la Zarzuela",
       1300,
       sol,
       96<quality>,
       Everywhere.Common.concertSpaceLayout4)
      ("Sala Mon",
       900,
       tetuán,
       87<quality>,
       Everywhere.Common.concertSpaceLayout1)
      ("Teatro Nuevo Apolo",
       1200,
       sol,
       88<quality>,
       Everywhere.Common.concertSpaceLayout2)
      ("Wurlitzer Ballroom",
       120,
       sol,
       79<quality>,
       Everywhere.Common.concertSpaceLayout3)
      ("Costello Club",
       300,
       sol,
       82<quality>,
       Everywhere.Common.concertSpaceLayout4) ]
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
        retiro

(* -------- Hospital --------- *)
let private addHospital city =
    let lobby = World.Node.create 0 RoomType.Lobby

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            "Hospital General Universitario Gregorio Marañón"
            85<quality>
            Hospital
            roomGraph
            retiro

    World.City.addPlace place city


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
    [ ("La Mesa Dorada", 90<quality>, Italian, chamberí)
      ("Le Petit Madrid", 88<quality>, French, malasaña)
      ("The Madrid Grill", 85<quality>, American, salamanca)
      ("El Sabor de Francia", 89<quality>, French, lavapiés)
      ("Tokyo Fusion", 87<quality>, Japanese, latina)
      ("Pho Madrid", 86<quality>, Vietnamese, arganzuela)
      ("Trattoria Bella Napoli", 92<quality>, Italian, retiro)
      ("Le Bistro Parisien", 91<quality>, French, sol)
      ("Burger City", 84<quality>, American, moncloa)
      ("Taco Loco", 88<quality>, Mexican, latina) ]
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
    [ ("Estudio Ángel",
       85<quality>,
       200m<dd>,
       chamberí,
       (Character.from "Carlos García" Male (December 24 1975)))
      ("Registros Madrileños",
       90<quality>,
       300m<dd>,
       salamanca,
       (Character.from "Ana Martínez" Female (March 15 1980)))
      ("Grabación El Retiro",
       92<quality>,
       340m<dd>,
       retiro,
       (Character.from "Fernando López" Male (July 10 1978)))
      ("Sonido Sol",
       80<quality>,
       100m<dd>,
       sol,
       (Character.from "Isabel Gómez" Female (September 5 1982)))
      ("Malasaña Discos",
       88<quality>,
       260m<dd>,
       malasaña,
       (Character.from "Pedro Torres" Male (June 20 1981)))
      ("Mezcla Latina",
       86<quality>,
       220m<dd>,
       latina,
       (Character.from "Elena Sánchez" Female (April 1 1990)))
      ("Melodías Chamberí",
       87<quality>,
       240m<dd>,
       chamberí,
       (Character.from "Javier Pérez" Male (January 30 1976)))
      ("Sonido Moncloa",
       85<quality>,
       200m<dd>,
       moncloa,
       (Character.from "Patricia Díaz" Female (August 15 1977)))
      ("Tones de Tetuán",
       89<quality>,
       280m<dd>,
       tetuán,
       (Character.from "Ricardo Aguilera" Male (February 2 1960)))
      ("Armonía de Arganzuela",
       90<quality>,
       300m<dd>,
       arganzuela,
       (Character.from "María Fernández" Female (May 3 1988))) ]
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
