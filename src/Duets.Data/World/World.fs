module Duets.Data.World.World

open Duets.Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let private generate () =
    let london = Cities.London.generate ()
    let madrid = Cities.Madrid.generate ()
    let mexicoCity = Cities.MexicoCity.generate ()
    let newYork = Cities.NewYork.generate ()
    let prague = Cities.Prague.generate ()
    let sydney = Cities.Sydney.generate ()
    let tokyo = Cities.Tokyo.generate ()

    World.create [ london; madrid; mexicoCity; newYork; prague; sydney; tokyo ]

/// Returns the game world. The world is initialized when the module is loaded.
let get = generate ()

/// Defines different metadata about the connections between cities: the
/// distance between them and which connections are available (road, sea or air)
let private connectionMetadata
    : Map<CityId * CityId, CityConnectionDistance * CityConnections> =
    [ ((London, Madrid), (1260<km>, [ Road; Air ]))
      ((London, NewYork), (5570<km>, [ Air ]))
      ((London, MexicoCity), (8904<km>, [ Air ]))
      ((London, Prague), (1035<km>, [ Road; Air ]))
      ((London, Sydney), (16900<km>, [ Air ]))
      ((London, Tokyo), (9600<km>, [ Air ]))
      ((Madrid, MexicoCity), (9066<km>, [ Air ]))
      ((Madrid, NewYork), (5768<km>, [ Air ]))
      ((Madrid, Prague), (1780<km>, [ Road; Air ]))
      ((Madrid, Sydney), (17864<km>, [ Air ]))
      ((Madrid, Tokyo), (10500<km>, [ Air ]))
      ((MexicoCity, NewYork), (3366<km>, [ Air ]))
      ((MexicoCity, Prague), (9907<km>, [ Air ]))
      ((MexicoCity, Sydney), (12982<km>, [ Air ]))
      ((MexicoCity, Tokyo), (11312<km>, [ Air ]))
      ((NewYork, Prague), (6570<km>, [ Air ]))
      ((NewYork, Sydney), (15900<km>, [ Air ]))
      ((NewYork, Tokyo), (10800<km>, [ Air ]))
      ((Prague, Tokyo), (90904<km>, [ Air ]))
      ((Prague, Sydney), (16084<km>, [ Air ]))
      ((Sydney, Tokyo), (7818<km>, [ Air ])) ]
    |> Map.ofList

/// Returns the distance between the given cities.
let distanceBetween city1 city2 =
    let key = if city1 < city2 then (city1, city2) else (city2, city1)
    connectionMetadata |> Map.find key |> fst
