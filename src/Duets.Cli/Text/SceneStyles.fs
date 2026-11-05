[<RequireQualifiedAccess>]
module Duets.Cli.Text.SceneStyles

open Duets.Entities

/// Spectre color names available to scenes.
module Colors =
    [<Literal>]
    let aqua = "aqua"

    [<Literal>]
    let darkOrange = "darkorange"

    [<Literal>]
    let deepSkyBlue = "deepskyblue1"

    [<Literal>]
    let gold = "gold1"

    [<Literal>]
    let grey35 = "grey35"

    [<Literal>]
    let grey50 = "grey50"

    [<Literal>]
    let grey62 = "grey62"

    [<Literal>]
    let grey70 = "grey70"

    [<Literal>]
    let hotPink = "hotpink"

    [<Literal>]
    let indianRed = "indianred"

    [<Literal>]
    let khaki = "khaki1"

    [<Literal>]
    let lightGreen = "lightgreen_1"

    [<Literal>]
    let lightSalmon = "lightsalmon1"

    [<Literal>]
    let lightSkyBlue = "lightskyblue1"

    [<Literal>]
    let magenta = "magenta1"

    [<Literal>]
    let navajoWhite = "navajowhite1"

    [<Literal>]
    let orange = "orange1"

    [<Literal>]
    let red = "red3"

    [<Literal>]
    let rosyBrown = "rosybrown"

    [<Literal>]
    let springGreen = "springgreen2"

    [<Literal>]
    let steelBlue = "steelblue"

    [<Literal>]
    let tan = "tan"

    [<Literal>]
    let wheat = "wheat4"

    [<Literal>]
    let white = "white"

    [<Literal>]
    let yellow = "yellow"

/// Makes the given color bold.
let bold color = $"bold {color}"

module Sky =
    let sun = '☼'
    let moon = '☾'
    let cloud = "░▒▓▒░"
    let overcast = '▀'

    let sunriseColor = Colors.orange
    let morningSunColor = Colors.gold
    let middaySunColor = Colors.yellow
    let sunsetColor = Colors.darkOrange
    let moonColor = Colors.white
    let cloudColor = Colors.grey62
    let overcastColor = Colors.grey35

module Weather =
    let rain = [| '\''; ',' |]
    let rainColor = Colors.steelBlue

    let storm = [| '/'; '/'; '\'' |]
    let stormColor = Colors.deepSkyBlue

    let snow = [| '*'; '·' |]
    let snowColor = Colors.white

    let stars = [| '·'; '.' |]
    let starsColor = Colors.grey70

module People =
    let player = '@'
    let playerStyle = bold Colors.white

    let known = '☻'
    let knownStyle = bold Colors.lightGreen

    let unknown = '☺'
    let unknownStyle = Colors.grey70

module Facade =
    /// Corners, top edge and sides of each type of frame, in the order:
    /// top-left, top, top-right, side.
    let singleFrame = '┌', '─', '┐', '│'
    let doubleFrame = '╔', '═', '╗', '║'
    let heavyFrame = '┏', '━', '┓', '┃'
    let roundedFrame = '╭', '─', '╮', '│'

    let flatAwning = '▀'
    let thinAwning = '▔'
    let stripedAwning = '▚'
    let thickAwning = '▄'

    let pitchedRoof = [ "▁▂▃▄▅▆▆▅▄▃▂▁" ]
    let marqueeRoof = [ "*  *  *  *  *  *" ]
    let antennaRoof = [ "((•))"; "╱╳╲"; "╱╳╳╳╲" ]
    let controlTowerRoof = [ "▄██▄"; "▐▌" ]
    let chimneyRoof = [ "▐▌          " ]
    let crossRoof = [ "✚" ]
    let sawtoothRoof = [ "◢◣◢◣◢◣◢◣◢◣◢◣" ]

    let shopWindow = '▒'
    let shopWindowDayColor = Colors.lightSkyBlue
    let shopWindowNightColor = Colors.gold

    let closedShopWindow = '░'
    let closedShopWindowColor = Colors.grey35

    let doorLeft = '▐'
    let doorRight = '▌'
    let doorColor = Colors.tan

    let shutter = '▬'
    let shutterColor = Colors.grey50

    let graffiti = '~'
    let graffitiColor = Colors.magenta

    let stairs = '▄'

    let window = '▪'
    let litWindowColor = Colors.khaki
    let unlitWindowColor = Colors.grey50

    let closedSignColor = Colors.grey50
    let hospitalCrossStyle = bold Colors.red

module Highlight =
    let rented = '♥'
    let rentedColor = bold Colors.hotPink

    let concert = '✦'
    let concertTodayColor = bold Colors.gold
    let concertScheduledColor = Colors.orange

    let workplace = '⚑'
    let workplaceColor = bold Colors.lightGreen

module Ground =
    let cobblestone = "▪·"
    let pavement = "─"
    let tiles = "▫"

    let laneMarkings = "──  "
    let doubleLaneMarkings = "══  "
    let heavyLaneMarkings = "━━  "

/// Returns an arrow pointing to the given direction.
let directionArrow direction =
    match direction with
    | North -> "↑"
    | NorthEast -> "↗"
    | East -> "→"
    | SouthEast -> "↘"
    | South -> "↓"
    | SouthWest -> "↙"
    | West -> "←"
    | NorthWest -> "↖"

/// Returns the icon that represents the given place type.
let placeIcon placeType =
    match placeType with
    | PlaceTypeIndex.Airport -> '✈'
    | PlaceTypeIndex.Bar -> '♪'
    | PlaceTypeIndex.Bookstore -> '≡'
    | PlaceTypeIndex.CarDealer -> '◆'
    | PlaceTypeIndex.Cafe -> '¤'
    | PlaceTypeIndex.Casino -> '$'
    | PlaceTypeIndex.Cinema -> '▶'
    | PlaceTypeIndex.ConcertSpace -> '♫'
    | PlaceTypeIndex.Gym -> '╪'
    | PlaceTypeIndex.Home -> '⌂'
    | PlaceTypeIndex.Hotel -> '★'
    | PlaceTypeIndex.Hospital -> '✚'
    | PlaceTypeIndex.MerchandiseWorkshop -> '#'
    | PlaceTypeIndex.MetroStation -> 'M'
    | PlaceTypeIndex.RadioStudio -> '≈'
    | PlaceTypeIndex.RehearsalSpace -> '♩'
    | PlaceTypeIndex.Restaurant -> 'Ψ'
    | PlaceTypeIndex.Studio -> '●'
    | PlaceTypeIndex.Street -> ' '
