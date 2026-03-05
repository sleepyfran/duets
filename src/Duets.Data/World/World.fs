module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let berlin = Cities.Berlin.Root.generate ()
    let london = Cities.London.Root.generate ()
    let losAngeles = Cities.LosAngeles.Root.generate ()
    let madrid = Cities.Madrid.Root.generate ()
    let newYork = Cities.NewYork.Root.generate ()
    let paris = Cities.Paris.Root.generate ()
    let prague = Cities.Prague.Root.generate ()
    let santiago = Cities.Santiago.Root.generate ()
    let seoul = Cities.Seoul.Root.generate ()
    let tokyo = Cities.Tokyo.Root.generate ()
    let toronto = Cities.Toronto.Root.generate ()

    World.create [ berlin; london; losAngeles; madrid; newYork; paris; prague; santiago; seoul; tokyo; toronto ]

/// Returns the game world. The world is initialized when the module is loaded.
let get = generate ()

/// Defines the metadata about the country a certain city belongs to.
let private countryMetadata: Map<CityId, CountryId> =
    [ (Berlin, Germany)
      (London, England)
      (LosAngeles, UnitedStates)
      (Madrid, Spain)
      (NewYork, UnitedStates)
      (Paris, France)
      (Prague, CzechRepublic)
      (Santiago, Chile)
      (Seoul, SouthKorea)
      (Tokyo, Japan)
      (Toronto, Canada) ]
    |> Map.ofList

/// Returns the country of the given city.
let countryOf city = countryMetadata |> Map.find city

/// Defines different metadata about the connections between cities: the
/// distance between them and which connections are available (road, sea or air)
let private connectionMetadata
    : Map<CityId * CityId, CityConnectionDistance * CityConnections> =
    [ ((Berlin, London), (930<km>, [ Road; Air ]))
      ((Berlin, LosAngeles), (9300<km>, [ Air ]))
      ((Berlin, Madrid), (1870<km>, [ Road; Air ]))
      ((Berlin, NewYork), (6380<km>, [ Air ]))
      ((Berlin, Paris), (880<km>, [ Road; Air ]))
      ((Berlin, Prague), (280<km>, [ Road; Air ]))
      ((Berlin, Santiago), (12300<km>, [ Air ]))
      ((Berlin, Seoul), (8600<km>, [ Air ]))
      ((Berlin, Tokyo), (8920<km>, [ Air ]))
      ((Berlin, Toronto), (6500<km>, [ Air ]))
      ((London, LosAngeles), (8800<km>, [ Air ]))
      ((London, Madrid), (1260<km>, [ Road; Air ]))
      ((London, NewYork), (5570<km>, [ Air ]))
      ((London, Paris), (340<km>, [ Road; Air ]))
      ((London, Prague), (1035<km>, [ Road; Air ]))
      ((London, Santiago), (11700<km>, [ Air ]))
      ((London, Seoul), (8900<km>, [ Air ]))
      ((London, Tokyo), (9560<km>, [ Air ]))
      ((London, Toronto), (5700<km>, [ Air ]))
      ((LosAngeles, Madrid), (9120<km>, [ Air ]))
      ((LosAngeles, NewYork), (3930<km>, [ Air; Road ]))
      ((LosAngeles, Paris), (9100<km>, [ Air ]))
      ((LosAngeles, Prague), (9640<km>, [ Air ]))
      ((LosAngeles, Santiago), (8900<km>, [ Air ]))
      ((LosAngeles, Seoul), (9600<km>, [ Air ]))
      ((LosAngeles, Tokyo), (8815<km>, [ Air ]))
      ((LosAngeles, Toronto), (3500<km>, [ Air ]))
      ((Madrid, NewYork), (5768<km>, [ Air ]))
      ((Madrid, Paris), (1050<km>, [ Road; Air ]))
      ((Madrid, Prague), (1780<km>, [ Road; Air ]))
      ((Madrid, Santiago), (10400<km>, [ Air ]))
      ((Madrid, Seoul), (9800<km>, [ Air ]))
      ((Madrid, Tokyo), (10500<km>, [ Air ]))
      ((Madrid, Toronto), (6050<km>, [ Air ]))
      ((NewYork, Paris), (5840<km>, [ Air ]))
      ((NewYork, Prague), (6570<km>, [ Air ]))
      ((NewYork, Santiago), (8250<km>, [ Air ]))
      ((NewYork, Seoul), (11050<km>, [ Air ]))
      ((NewYork, Tokyo), (10850<km>, [ Air ]))
      ((NewYork, Toronto), (550<km>, [ Air; Road ]))
      ((Paris, Prague), (885<km>, [ Road; Air ]))
      ((Paris, Santiago), (11600<km>, [ Air ]))
      ((Paris, Seoul), (8950<km>, [ Air ]))
      ((Paris, Tokyo), (9720<km>, [ Air ]))
      ((Paris, Toronto), (5900<km>, [ Air ]))
      ((Prague, Santiago), (12400<km>, [ Air ]))
      ((Prague, Seoul), (8600<km>, [ Air ]))
      ((Prague, Tokyo), (9100<km>, [ Air ]))
      ((Prague, Toronto), (6900<km>, [ Air ]))
      ((Santiago, Seoul), (18700<km>, [ Air ]))
      ((Santiago, Tokyo), (17200<km>, [ Air ]))
      ((Santiago, Toronto), (8600<km>, [ Air ]))
      ((Seoul, Tokyo), (1160<km>, [ Air ]))
      ((Seoul, Toronto), (10600<km>, [ Air ]))
      ((Tokyo, Toronto), (10340<km>, [ Air ])) ]
    |> Map.ofList

/// Returns the distance between the given cities.
let distanceBetween city1 city2 =
    let key = if city1 < city2 then (city1, city2) else (city2, city1)
    connectionMetadata |> Map.find key |> fst

/// Returns all cities that can be reached by road from the given city.
let citiesReachableByRoadFrom cityId =
    connectionMetadata
    |> Map.toList
    |> List.collect (fun ((city1, city2), (distance, connections)) ->
        if connections |> List.contains Road then
            if city1 = cityId then [ (city2, distance) ]
            elif city2 = cityId then [ (city1, distance) ]
            else []
        else
            [])
