[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.CitySkin

open Duets.Cli.Text
open Duets.Entities

module Colors = SceneStyles.Colors
module Facade = SceneStyles.Facade
module Furniture = SceneStyles.Furniture
module Ground = SceneStyles.Ground

/// Defines the art and style of a vehicle.
type Vehicle = string * string

/// Defines the glyphs and styles of a street prop, from the bottom up.
type StreetProp = (char * string) list

/// Defines how the buildings of a city differ from the default façades.
type FacadeStyle =
    {
        /// Pattern of the windows of each floor, where W is a window.
        FloorPattern: string
        /// Roof of the buildings that don't have their own.
        Roof: string list
        RoofColor: string option
        /// Floors added to every building that has floors.
        ExtraFloors: int
        VerticalSigns: bool
        /// Chance of graffiti on the empty parts of the storefronts.
        Graffiti: float
    }

let private defaultFacade =
    { FloorPattern = Facade.defaultFloor
      Roof = []
      RoofColor = None
      ExtraFloors = 0
      VerticalSigns = false
      Graffiti = 0.0 }

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
        Facade: FacadeStyle
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
              Furniture.tree ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.plattenbauFloor
                Graffiti = 0.3 } }
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
              Furniture.tree ]
          Facade =
            { defaultFacade with
                Roof = Facade.chimneyPotsRoof } }
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
              Furniture.streetLamp ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.glassFloor
                ExtraFloors = -1 } }
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
              Furniture.trafficLight ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.balconyFloor
                Roof = Facade.tiledRoof
                RoofColor = Some Colors.indianRed } }
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
              Furniture.subwaySteam ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.fireEscapeFloor
                Roof = Facade.waterTowerRoof
                ExtraFloors = 1 } }
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
              Furniture.tree ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.haussmannFloor
                Roof = Facade.mansardRoof
                RoofColor = Some Colors.grey50 } }
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
              Furniture.tree ]
          Facade =
            { defaultFacade with
                Roof = Facade.gableRoof } }
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
              Furniture.trafficLight ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.colonialFloor
                Roof = Facade.tiledRoof
                RoofColor = Some Colors.indianRed
                ExtraFloors = -1 } }
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
              Furniture.trafficLight ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.gridFloor
                ExtraFloors = 1
                VerticalSigns = true } }
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
              Furniture.trafficLight ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.glassFloor
                ExtraFloors = 1
                VerticalSigns = true } }
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
              Furniture.trafficLight ]
          Facade =
            { defaultFacade with
                FloorPattern = Facade.glassFloor
                ExtraFloors = 1 } }
