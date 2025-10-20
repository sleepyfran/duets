module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let london = Cities.London.Root.generate ()
    let losAngeles = Cities.LosAngeles.Root.generate ()
    let madrid = Cities.Madrid.Root.generate ()
    let newYork = Cities.NewYork.Root.generate ()
    let prague = Cities.Prague.Root.generate ()

    World.create [ london; losAngeles; madrid; newYork; prague ]

/// Returns the game world. The world is initialized when the module is loaded.
let get = generate ()

/// Defines the metadata about the country a certain city belongs to.
let private countryMetadata: Map<CityId, CountryId> =
    [ (London, England)
      (LosAngeles, UnitedStates)
      (Madrid, Spain)
      (NewYork, UnitedStates)
      (Prague, CzechRepublic) ]
    |> Map.ofList

/// Returns the country of the given city.
let countryOf city = countryMetadata |> Map.find city

/// Defines different metadata about the connections between cities: the
/// distance between them and which connections are available (road, sea or air)
let private connectionMetadata
    : Map<CityId * CityId, CityConnectionDistance * CityConnections> =
    [ ((London, LosAngeles), (8800<km>, [ Air ]))
      ((London, Madrid), (1260<km>, [ Road; Air ]))
      ((London, NewYork), (5570<km>, [ Air ]))
      ((London, Prague), (1035<km>, [ Road; Air ]))
      ((LosAngeles, Madrid), (9120<km>, [ Air ]))
      ((LosAngeles, NewYork), (3930<km>, [ Air; Road ]))
      ((LosAngeles, Prague), (9640<km>, [ Air ]))
      ((Madrid, NewYork), (5768<km>, [ Air ]))
      ((Madrid, Prague), (1780<km>, [ Road; Air ]))
      ((NewYork, Prague), (6570<km>, [ Air ])) ]
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
