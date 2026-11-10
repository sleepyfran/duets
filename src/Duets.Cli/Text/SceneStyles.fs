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

    let waterTowerRoof = [ "           ◢◣ "; "           ╨╨ " ]
    let mansardRoof = [ "  ▄   ▄   ▄   ▄  "; " ▟█▄▄▄█▄▄▄█▄▄▄█▙ " ]
    let chimneyPotsRoof = [ " ▐▌▐▌        ▐▌▐▌ " ]
    let gableRoof = [ "▗▄▖"; "▄▟███▙▄" ]
    let tiledRoof = [ "▁▂▂▂▂▂▂▂▂▂▂▂▂▂▂▂▁" ]

    let defaultFloor = " WW  WW  WW  WW "
    let fireEscapeFloor = " WW╫WW    WW╫WW "
    let haussmannFloor = " W W W W W W W W"
    let plattenbauFloor = "W W W W W W W W "
    let balconyFloor = "╟W╢ ╟W╢ ╟W╢ ╟W╢ "
    let glassFloor = " WWWWWWWWWWWWWW "
    let gridFloor = " WW WW WW WW WW "
    let colonialFloor = "  W   W   W   W "

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

/// Vehicles and street props of the cities.
module Furniture =
    let lamp = "$lamp"

    let greyCar = "▄██▄", Colors.grey62
    let whiteCar = "▄██▄", Colors.white
    let redCar = "▄██▄", Colors.indianRed
    let blueCar = "▄██▄", Colors.steelBlue
    let convertible = "▄▆▄▄", Colors.hotPink

    let yellowCab = " TAXI ", "bold black on yellow"
    let blackCab = " TAXI ", "bold yellow on grey11"
    let ivoryCab = " TAXI ", "bold black on navajowhite1"
    let madridCab = " TAXI ", "bold red3 on white"
    let tokyoCab = " TAXI ", "bold white on navyblue"
    let orangeCab = " TAXI ", "bold black on orange1"

    let mtaBus = " ▪▪ MTA ▪▪▪ ", "bold white on steelblue"
    let doubleDecker = " ▪▪▪ 73 ▪▪▪ ", "bold white on red3"
    let berlinTram = " ▪▪ M10 ▪▪▪ ", "bold black on yellow"
    let laMetroBus = " ▪▪ METRO ▪▪ ", "bold black on orange1"
    let emtBus = " ▪▪ EMT ▪▪▪ ", "bold white on blue"
    let ratpBus = " ▪▪ RATP ▪▪ ", "bold white on darkgreen"
    let pragueTram = " ▪▪ 22 ▪▪▪▪ ", "bold white on red3"
    let redBus = " ▪▪ RED ▪▪▪ ", "bold white on red3"
    let seoulBus = " ▪▪ 7016 ▪▪ ", "bold white on green3"
    let toeiBus = " ▪▪ TOEI ▪▪ ", "bold black on grey85"
    let ttcStreetcar = " ▪▪ 504 ▪▪▪ ", "bold white on red3"

    let streetLamp =
        [ '│', Colors.grey50; '│', Colors.grey50; '●', lamp ]

    let gasLamp = [ '│', Colors.grey50; '│', Colors.grey50; '♦', lamp ]

    let trafficLight =
        [ '│', Colors.grey50
          '●', Colors.springGreen
          '●', Colors.gold
          '●', Colors.red ]

    let tree = [ '│', Colors.tan; '♣', "green3" ]
    let cherryTree = [ '│', Colors.tan; '♣', "pink1" ]
    let mapleTree = [ '│', Colors.tan; '♣', Colors.red ]

    let palmTree =
        [ '│', Colors.tan; '│', Colors.tan; '│', Colors.tan; '✱', "green3" ]

    let subwaySteam =
        [ '≈', Colors.grey70; '~', Colors.grey62; '·', Colors.grey50 ]

    let uBahnSign = [ '│', Colors.grey50; 'U', "bold white on blue" ]
    let undergroundSign = [ '│', Colors.grey50; 'Θ', "bold red3" ]
    let parisMetroSign = [ '│', Colors.grey50; 'M', "bold black on yellow" ]
    let madridMetroSign = [ '│', Colors.grey50; 'M', "bold white on red3" ]
    let ttcSign = [ '│', Colors.grey50; 'T', "bold white on red3" ]
    let phoneBox = [ '█', Colors.red; '▀', Colors.red ]
    let morrisColumn = [ '█', "darkgreen"; '█', "darkgreen"; '▲', "darkgreen" ]
    let vendingMachine = [ '▥', "bold white on blue"; '▥', "bold white on blue" ]

    let skylineUnlitColor = "grey30"

module Room =
    let exitColor = bold Colors.aqua
    let streetExit = '⇩'

    let crowd = [| '☺'; 'o'; 'ö' |]
    let crowdColor = Colors.grey70

    let boredCrowd = [| '.'; 'o' |]
    let boredCrowdColor = Colors.grey35

    let armsUp = 'Y'
    let hypedCrowdColor = Colors.white

    let crowdLights = [| '✦'; '♥' |]
    let ecstaticCrowdColor = bold Colors.white

    let songProgressDone = '━'
    let songProgressLeft = '─'
    let songProgressColor = Colors.grey50

    /// How much of a bored crowd stays in front of the stage.
    let boredCrowdRatio = 0.6

    /// Style of `$lamp`, for lights that turn on at night.
    let lampOnColor = bold Colors.khaki
    let lampOffColor = Colors.grey50

    /// Style of `$stage`, for lights that turn on during concerts.
    let stageOnColor = bold Colors.magenta
    let stageOffColor = Colors.grey35

    /// Colors that the stage lights go through while a song plays, by the
    /// energy of the song.
    let energeticLights =
        [| bold Colors.red; bold Colors.magenta; bold Colors.white; bold Colors.orange |]

    let normalLights =
        [| bold Colors.magenta; bold Colors.deepSkyBlue; bold Colors.hotPink |]

    let limitedLights = [| Colors.steelBlue; Colors.deepSkyBlue; Colors.lightSkyBlue |]

    let daySky = '░'
    let daySkyColor = Colors.lightSkyBlue
    let sunsetSkyColor = Colors.orange
    let cloudySkyColor = Colors.grey62
    let nightSky = ' '

    /// Returns how the given item looks in a scene.
    let itemVisual (item: Item) =
        match item.Properties |> List.tryHead with
        | Some Cookware -> "◙◙", Colors.grey70
        | Some(Drinkable { DrinkType = Beer _ }) -> "¡", Colors.gold
        | Some(Drinkable { DrinkType = Coffee _ }) -> "c", Colors.tan
        | Some(Drinkable _) -> "¡", Colors.red
        | Some(Edible _) -> "•", Colors.orange
        | Some FitnessEquipment -> "╪═╪", Colors.grey70
        | Some(Key _) -> "⌐", Colors.gold
        | Some(Listenable _) -> "◉", Colors.grey70
        | Some(Playable Darts) -> "◎", bold Colors.red
        | Some(Playable Billiard) -> "▓▓▓▓▓", Colors.springGreen
        | Some(Playable VideoGame) -> "▪", Colors.white
        | Some(Readable _) -> "▤", Colors.tan
        | Some(Rideable Metro) -> "▐■■■■■■■■■■■■■■■■■■■■■■▌", Colors.steelBlue
        | Some(Rideable(Car _)) -> "▄▆▆▄", Colors.indianRed
        | Some Sleepable -> "■▄▄▄▄", Colors.navajoWhite
        | Some(Storage(Fridge, _)) -> "▐█▌", Colors.white
        | Some(Storage(Shelf, _)) -> "╞═╡", Colors.tan
        | Some Watchable -> "[▓▓]", Colors.lightSkyBlue
        | Some(Wearable _) -> "⊤", Colors.hotPink
        | _ -> "•", Colors.grey62

    /// Returns the sign of a restaurant of the given cuisine.
    let cuisineSign cuisine =
        match cuisine with
        | American -> "★ DINER ★"
        | Canadian -> "POUTINE"
        | Chilean -> "EMPANADAS"
        | Czech -> "HOSPODA"
        | German -> "BIERHAUS"
        | Italian -> "PIZZA·PASTA"
        | French -> "BISTROT"
        | Japanese -> "SUSHI·RAMEN"
        | Korean -> "KOREAN BBQ"
        | Mexican -> "TACOS"
        | Turkish -> "KEBAP"
        | Vietnamese -> "PHỞ"
        | Spanish -> "TAPAS"

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
