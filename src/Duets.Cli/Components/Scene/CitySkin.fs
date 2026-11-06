[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.CitySkin

open Duets.Cli.Text
open Duets.Entities

module Colors = SceneStyles.Colors
module Furniture = SceneStyles.Furniture
module Ground = SceneStyles.Ground

/// Defines the art and style of a vehicle.
type Vehicle = string * string

/// Defines the glyphs and styles of a street prop, from the bottom up.
type StreetProp = (char * string) list

/// Defines how a city looks when rendered in a scene: the colors of its buildings
/// and signs and the patterns used for the sidewalk and the road.
type Skin =
    {
        /// Color of the frame of the buildings.
        Wall: string
        /// Colors that can be used for signs.
        Signs: string list
        /// Pattern repeated along the sidewalk.
        Sidewalk: string
        SidewalkColor: string
        /// Pattern repeated along the road.
        Road: string
        RoadColor: string
        /// Vehicles on the road, repeat one to make it more common.
        Vehicles: Vehicle list
        /// Props between the buildings.
        StreetProps: StreetProp list
    }

/// Returns the skin of the given city.
let forCity cityId =
    match cityId with
    | Berlin ->
        { Wall = Colors.grey62
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.tiles
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.ivoryCab
              Furniture.berlinTram
              Furniture.berlinTram
              Furniture.greyCar
              Furniture.whiteCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.trafficLight
              Furniture.uBahnSign
              Furniture.tree ] }
    | London ->
        { Wall = Colors.indianRed
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey62
          Road = Ground.doubleLaneMarkings
          RoadColor = Colors.gold
          Vehicles =
            [ Furniture.blackCab
              Furniture.blackCab
              Furniture.doubleDecker
              Furniture.doubleDecker
              Furniture.greyCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.undergroundSign
              Furniture.phoneBox
              Furniture.tree ] }
    | LosAngeles ->
        { Wall = Colors.navajoWhite
          Signs = [ Colors.orange; Colors.hotPink; Colors.aqua ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.wheat
          Road = Ground.laneMarkings
          RoadColor = Colors.gold
          Vehicles =
            [ Furniture.greyCar
              Furniture.whiteCar
              Furniture.redCar
              Furniture.blueCar
              Furniture.convertible
              Furniture.laMetroBus ]
          StreetProps =
            [ Furniture.palmTree
              Furniture.palmTree
              Furniture.palmTree
              Furniture.trafficLight
              Furniture.streetLamp ] }
    | Madrid ->
        { Wall = Colors.lightSalmon
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.tan
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.madridCab
              Furniture.madridCab
              Furniture.emtBus
              Furniture.greyCar
              Furniture.redCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.madridMetroSign
              Furniture.tree
              Furniture.trafficLight ] }
    | NewYork ->
        { Wall = Colors.rosyBrown
          Signs = [ Colors.gold; Colors.red; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey50
          Road = Ground.heavyLaneMarkings
          RoadColor = Colors.gold
          Vehicles =
            [ Furniture.yellowCab
              Furniture.yellowCab
              Furniture.yellowCab
              Furniture.mtaBus
              Furniture.greyCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.trafficLight
              Furniture.subwaySteam ] }
    | Paris ->
        { Wall = Colors.navajoWhite
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.grey62
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.ratpBus
              Furniture.greyCar
              Furniture.whiteCar
              Furniture.blueCar ]
          StreetProps =
            [ Furniture.gasLamp
              Furniture.parisMetroSign
              Furniture.morrisColumn
              Furniture.tree ] }
    | Prague ->
        { Wall = Colors.khaki
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.grey62
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.pragueTram
              Furniture.pragueTram
              Furniture.greyCar
              Furniture.whiteCar ]
          StreetProps =
            [ Furniture.gasLamp
              Furniture.gasLamp
              Furniture.tree ] }
    | Santiago ->
        { Wall = Colors.lightSalmon
          Signs = [ Colors.gold; Colors.springGreen; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.tan
          Road = Ground.laneMarkings
          RoadColor = Colors.gold
          Vehicles =
            [ Furniture.redBus
              Furniture.redBus
              Furniture.blackCab
              Furniture.greyCar
              Furniture.whiteCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.tree
              Furniture.palmTree
              Furniture.trafficLight ] }
    | Seoul ->
        { Wall = Colors.grey50
          Signs =
            [ Colors.magenta; Colors.aqua; Colors.hotPink; Colors.springGreen ]
          Sidewalk = Ground.tiles
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.orangeCab
              Furniture.orangeCab
              Furniture.seoulBus
              Furniture.whiteCar
              Furniture.greyCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.cherryTree
              Furniture.vendingMachine
              Furniture.trafficLight ] }
    | Tokyo ->
        { Wall = Colors.grey62
          Signs = [ Colors.magenta; Colors.aqua; Colors.hotPink; Colors.gold ]
          Sidewalk = Ground.tiles
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.white
          Vehicles =
            [ Furniture.tokyoCab
              Furniture.tokyoCab
              Furniture.toeiBus
              Furniture.whiteCar
              Furniture.greyCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.vendingMachine
              Furniture.vendingMachine
              Furniture.cherryTree
              Furniture.trafficLight ] }
    | Toronto ->
        { Wall = Colors.grey62
          Signs = [ Colors.red; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.gold
          Vehicles =
            [ Furniture.orangeCab
              Furniture.ttcStreetcar
              Furniture.ttcStreetcar
              Furniture.greyCar
              Furniture.redCar ]
          StreetProps =
            [ Furniture.streetLamp
              Furniture.ttcSign
              Furniture.mapleTree
              Furniture.trafficLight ] }
