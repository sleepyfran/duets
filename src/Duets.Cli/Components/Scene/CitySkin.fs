[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.CitySkin

open Duets.Cli.Text
open Duets.Entities

module Colors = SceneStyles.Colors
module Ground = SceneStyles.Ground

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
          RoadColor = Colors.white }
    | London ->
        { Wall = Colors.indianRed
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey62
          Road = Ground.doubleLaneMarkings
          RoadColor = Colors.gold }
    | LosAngeles ->
        { Wall = Colors.navajoWhite
          Signs = [ Colors.orange; Colors.hotPink; Colors.aqua ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.wheat
          Road = Ground.laneMarkings
          RoadColor = Colors.gold }
    | Madrid ->
        { Wall = Colors.lightSalmon
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.tan
          Road = Ground.laneMarkings
          RoadColor = Colors.white }
    | NewYork ->
        { Wall = Colors.rosyBrown
          Signs = [ Colors.gold; Colors.red; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey50
          Road = Ground.heavyLaneMarkings
          RoadColor = Colors.gold }
    | Paris ->
        { Wall = Colors.navajoWhite
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.grey62
          Road = Ground.laneMarkings
          RoadColor = Colors.white }
    | Prague ->
        { Wall = Colors.khaki
          Signs = [ Colors.gold; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.grey62
          Road = Ground.laneMarkings
          RoadColor = Colors.white }
    | Santiago ->
        { Wall = Colors.lightSalmon
          Signs = [ Colors.gold; Colors.springGreen; Colors.white ]
          Sidewalk = Ground.cobblestone
          SidewalkColor = Colors.tan
          Road = Ground.laneMarkings
          RoadColor = Colors.gold }
    | Seoul ->
        { Wall = Colors.grey50
          Signs =
            [ Colors.magenta; Colors.aqua; Colors.hotPink; Colors.springGreen ]
          Sidewalk = Ground.tiles
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.white }
    | Tokyo ->
        { Wall = Colors.grey62
          Signs = [ Colors.magenta; Colors.aqua; Colors.hotPink; Colors.gold ]
          Sidewalk = Ground.tiles
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.white }
    | Toronto ->
        { Wall = Colors.grey62
          Signs = [ Colors.red; Colors.white ]
          Sidewalk = Ground.pavement
          SidewalkColor = Colors.grey50
          Road = Ground.laneMarkings
          RoadColor = Colors.gold }
