[<RequireQualifiedAccess>]
module Duets.Cli.Text.SceneCaptions

open Duets.Entities

/// Part of the day that changes how the street feels, coarser than a
/// `DayMoment`.
type DayPhase =
    | Dawn
    | Day
    | Dusk
    | Dark

/// Returns the phase of the day in which the given day moment falls.
let phaseOf dayMoment =
    match dayMoment with
    | EarlyMorning -> Dawn
    | Morning
    | Midday
    | Afternoon -> Day
    | Evening -> Dusk
    | Night
    | Midnight -> Dark

/// Opening clauses of the caption, describing the sky and the light. They
/// have no punctuation at the end since they're followed by an activity.
let atmosphere weather phase =
    match weather, phase with
    | WeatherCondition.Sunny, Dawn ->
        [ "The first light creeps over the rooftops"
          "A pale sunrise paints the windows orange"
          "The street wakes up under a clear sky" ]
    | WeatherCondition.Sunny, Day ->
        [ "Sunlight warms the façades"
          "The sun bounces off the shop windows"
          "A bright sky hangs over the rooftops" ]
    | WeatherCondition.Sunny, Dusk ->
        [ "The sunset stretches long shadows down the street"
          "Golden light fades from the rooftops"
          "The last of the sun lingers on the windows" ]
    | WeatherCondition.Sunny, Dark ->
        [ "Streetlights hum under a clear night sky"
          "The moon hangs above the rooftops"
          "Stars peek out between the buildings" ]
    | WeatherCondition.Cloudy, Dark ->
        [ "Streetlights glow against a starless sky"
          "Low clouds swallow the glow of the city" ]
    | WeatherCondition.Cloudy, _ ->
        [ "Grey clouds hang low over the street"
          "A flat, grey light settles over the façades"
          "The sky is a sheet of grey" ]
    | WeatherCondition.Rainy, Dark ->
        [ "Rain glistens under the streetlights"
          "Neon signs ripple in the wet pavement" ]
    | WeatherCondition.Rainy, _ ->
        [ "Rain drums on the awnings"
          "Puddles gather along the curb"
          "A steady drizzle slicks the pavement" ]
    | WeatherCondition.Stormy, _ ->
        [ "Thunder rolls over the rooftops"
          "Wind whips the rain down the street"
          "Lightning flashes behind the buildings" ]
    | WeatherCondition.Snowy, Dark ->
        [ "Snow drifts through the glow of the streetlights"
          "The snow muffles the night" ]
    | WeatherCondition.Snowy, _ ->
        [ "A thin layer of snow covers the sidewalk"
          "Snow muffles the sound of the city"
          "Snowflakes swirl between the buildings" ]

/// Closing clauses of the first sentence, describing how busy the street is.
let activity peopleCount =
    match peopleCount with
    | 0 ->
        [ " and nobody else is around."
          ", and the sidewalk is deserted."
          ", and the street is all yours." ]
    | 1 ->
        [ " while someone hurries past."
          " as a lone passer-by strolls along." ]
    | count when count <= 3 ->
        [ " as a few people walk by."
          " as a couple of passers-by stroll along." ]
    | _ ->
        [ " while people bustle along the sidewalk."
          " as a crowd fills the sidewalk."
          " and the street is packed with people." ]

/// Optional second sentence about one of the places in the street.
let nightlife placeName =
    [ $"Music drifts out of {placeName}."
      $"A muffled bassline leaks from {placeName}."
      $"Laughter spills out of {placeName}." ]

let food placeName =
    [ $"The smell of food wafts from {placeName}."
      $"Clinking cutlery echoes out of {placeName}." ]

let coffee placeName =
    [ $"The smell of coffee comes from {placeName}."
      $"A queue for coffee forms outside {placeName}." ]

let allClosed =
    [ "Every shutter on the street is down."
      "All the shops have called it a day." ]

/// Opening clauses of the room caption, by the quality of the place.
let roomCondition (quality: Quality) =
    match quality with
    | q when q < 20<quality> ->
        [ "The paint peels off the walls"
          "A damp smell hangs in the air"
          "The floor creaks with every step" ]
    | q when q < 40<quality> ->
        [ "The furniture has seen better days"
          "Scuff marks cover the floor"
          "A flickering bulb buzzes overhead" ]
    | q when q < 60<quality> ->
        [ "The place looks lived in"
          "Nothing here stands out much"
          "The room is plain but tidy" ]
    | q when q < 80<quality> ->
        [ "Everything looks clean and in its place"
          "The room feels warm and well kept"
          "Fresh paint brightens the walls" ]
    | _ ->
        [ "Everything shines like it's brand new"
          "Polished surfaces gleam all around"
          "Not a speck of dust in sight" ]

/// Closing clauses of the room caption, by the number of people.
let roomActivity peopleCount =
    match peopleCount with
    | 0 ->
        [ ", and the room is quiet."
          " and nobody else is around."
          ", and you have the place to yourself." ]
    | 1 ->
        [ ", and someone else is around."
          " and one other person keeps to themselves." ]
    | count when count <= 3 ->
        [ ", and a few people chat quietly."
          " and a couple of people hang around." ]
    | _ ->
        [ ", and the room is buzzing with people."
          " and it's hard to find a free spot."
          ", and voices fill the room." ]

/// Room caption on stage during a concert, by how full the venue is.
let crowd (fill: float) =
    match fill with
    | fill when fill < 0.2 ->
        [ "Only a handful of people stand in front of the stage."
          "Your voice echoes across a mostly empty room." ]
    | fill when fill < 0.6 ->
        [ "A decent crowd gathers in front of the stage."
          "Heads bob along across half of the room." ]
    | fill when fill < 0.9 ->
        [ "The crowd fills the room, hands in the air."
          "A sea of faces looks up at the stage." ]
    | _ ->
        [ "The venue is packed wall to wall."
          "The crowd roars from the front row to the back." ]

/// Second sentence of the room caption for rooms with windows.
let throughWindow weather phase =
    match weather, phase with
    | WeatherCondition.Rainy, _ ->
        [ "Rain taps against the windows."
          "Raindrops race down the glass." ]
    | WeatherCondition.Stormy, _ ->
        [ "Thunder rattles the windows."
          "Lightning flashes through the glass." ]
    | WeatherCondition.Snowy, _ ->
        [ "Snowflakes drift past the windows."
          "Frost creeps along the edges of the glass." ]
    | _, Dawn -> [ "The first light of the day comes through the windows." ]
    | WeatherCondition.Sunny, Day -> [ "Sunlight pours through the windows." ]
    | _, Dusk -> [ "The windows glow orange with the sunset." ]
    | _, Dark -> [ "City lights twinkle outside the windows." ]
    | _ -> []

/// Optional second sentence of the street caption about the city itself.
let city cityId phase =
    match cityId, phase with
    | Berlin, Dark ->
        [ "A bass drum thumps behind a metal door somewhere."
          "A bike with a crate of Club-Mate leans on a lamp post." ]
    | Berlin, _ ->
        [ "A tram bell rings at the corner."
          "Fresh posters cover last week's on the wall."
          "Bikes are chained to every railing." ]
    | London, Dark ->
        [ "Last orders get called in a pub down the road."
          "A night bus rumbles past, half empty." ]
    | London, _ ->
        [ "A red bus squeezes past a parked van."
          "A newspaper flaps around in the wind."
          "A pigeon struts along the curb like it owns it." ]
    | LosAngeles, Dark ->
        [ "A helicopter sweeps its light over the rooftops."
          "Bass rolls out of a car stuck at the light." ]
    | LosAngeles, _ ->
        [ "Traffic crawls bumper to bumper down the boulevard."
          "Palm trees sway over a billboard for a new show."
          "Sprinklers hiss on a patch of lawn." ]
    | Madrid, Dark ->
        [ "Chatter comes from the terraces around the corner."
          "A waiter somewhere shouts an order of cañas." ]
    | Madrid, _ ->
        [ "The smell of fried churros drifts down the street."
          "Neighbours chat from balcony to balcony."
          "A scooter buzzes past a row of terraces." ]
    | NewYork, Dark ->
        [ "A siren wails a few blocks uptown."
          "The bodega on the corner is still open, as always." ]
    | NewYork, _ ->
        [ "A cab honks at nobody in particular."
          "Steam rises from a grate in the sidewalk."
          "A hot dog cart sets up on the corner." ]
    | Paris, Dark ->
        [ "Glasses clink on a terrace that refuses to close."
          "A scooter zips through the empty street." ]
    | Paris, _ ->
        [ "The smell of fresh bread comes from a boulangerie."
          "Wicker chairs line up on a café terrace."
          "A bakery window is packed with baguettes." ]
    | Prague, Dark ->
        [ "Singing comes from a pub down the street."
          "The gas lamps flicker over the cobblestones." ]
    | Prague, _ ->
        [ "A red tram rattles over the cobblestones."
          "The smell of trdelník drifts from a stand."
          "Church bells ring somewhere in the old town." ]
    | Santiago, Dark ->
        [ "Music spills out of a picada on the corner."
          "A street dog naps under a lamp post." ]
    | Santiago, _ ->
        [ "The snowy Andes stand behind the city."
          "A completo stand fills the air with the smell of avocado."
          "A street dog trots along, minding its own business." ]
    | Seoul, Dark ->
        [ "Neon signs climb up the buildings floor by floor."
          "Laughter and clinking soju glasses come from a tent bar." ]
    | Seoul, _ ->
        [ "A delivery scooter weaves between the cars."
          "K-pop plays from a cosmetics shop."
          "A café sign promises the best bingsu in town." ]
    | Tokyo, Dark ->
        [ "Red lanterns glow over the door of a tiny bar."
          "Vending machines glow on every corner." ]
    | Tokyo, _ ->
        [ "The pedestrian crossing chirps a little tune."
          "A vending machine hums next to a doorway."
          "The entrance of a shop has been freshly swept." ]
    | Toronto, Dark ->
        [ "The streetcar's bell rings through the quiet street."
          "A hockey game blares from the TV of a bar." ]
    | Toronto, _ ->
        [ "A streetcar rolls by, packed with commuters."
          "A maple leaf flag hangs over a shop door."
          "The smell of coffee drifts out of a café." ]
