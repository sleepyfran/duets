namespace Duets.Cli.Text.Flavor

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Flight =
    let private cabin phase =
        match phase with
        | SceneCaptions.Dawn ->
            [ "The cabin lights come on slowly as the crew hands out coffee to half-asleep passengers."
              "A thin line of orange light creeps through the window shades as the plane climbs."
              "Most passengers doze under thin blankets while the crew prepares a quiet breakfast service." ]
        | SceneCaptions.Day ->
            [ "Sunlight pours through the windows and someone in the row behind keeps asking for more peanuts."
              "The drinks trolley rattles down the aisle while a toddler a few rows ahead narrates every cloud."
              "Below the wing, fields and rivers stitch together into a patchwork that slowly drifts by."
              "The passenger in the middle seat has claimed both armrests and doesn't seem willing to negotiate." ]
        | SceneCaptions.Dusk ->
            [ "The sun sinks below the wing and paints the clouds in shades of pink and orange."
              "The cabin dims as the evening meal is served, the smell of reheated pasta filling the aisle."
              "Reading lights flick on one by one as the sky outside turns violet." ]
        | SceneCaptions.Dark ->
            [ "The cabin is dark except for the glow of a dozen seat-back screens playing a dozen different films."
              "Outside the window there's nothing but blackness and the blinking light on the tip of the wing."
              "Snoring drifts from somewhere in the back while the engines hum a steady lullaby." ]

    let private turbulence =
        [ "A patch of turbulence makes the seatbelt sign ping on, and a few drinks wobble dangerously."
          "The captain announces smooth skies ahead, which is immediately followed by a sharp bump."
          "The plane shudders through a layer of clouds before settling back into a gentle hum."
          "" ]

    let private passengers destination =
        match destination with
        | Berlin ->
            [ "A group of friends a few rows ahead are already planning which techno clubs to hit tonight, debating in German."
              "The couple next to you swaps tips about the best currywurst stand near the station." ]
        | London ->
            [ "The man across the aisle apologizes three times for brushing your elbow, in a very polite British accent."
              "Someone behind you is explaining the difference between the Tube lines to a confused tourist." ]
        | LosAngeles ->
            [ "The passenger next to you mentions, unprompted, that they're in the industry and have a meeting with a producer."
              "A row of surfers in flip-flops compares notes on which beaches will have the best waves this week." ]
        | Madrid ->
            [ "Rapid Spanish fills the cabin as a big family shares snacks across three rows."
              "Someone is already planning a late dinner, as in starting at eleven at night, for when they land." ]
        | NewYork ->
            [ "The passenger next to you has opinions about every borough and is more than happy to share them."
              "A businessman on a call before takeoff promised someone he'd be in Midtown by the time they got to the office." ]
        | Paris ->
            [ "A couple in front of you is softly speaking French while sharing a croissant they smuggled on board."
              "A student next to you practices French phrases from a pocket guide, mouthing each word carefully." ]
        | Prague ->
            [ "A group of tourists debates whether to visit the castle or the beer halls first, settling on both."
              "An elderly couple speaks quietly in Czech, pointing at the river on a paper map." ]
        | Santiago ->
            [ "Somebody pulls up the window shade to catch a glimpse of the Andes, and half the cabin leans over to look."
              "Chilean Spanish, fast and full of slang, bounces between the rows." ]
        | Seoul ->
            [ "The person beside you is watching a Korean drama with the volume just loud enough to follow the plot."
              "A group of students compares notes on the best spots for late-night fried chicken in Seoul." ]
        | Tokyo ->
            [ "The cabin crew bows slightly as they pass, and announcements alternate between Japanese and English."
              "A passenger next to you carefully folds their blanket into a perfect square before landing." ]
        | Toronto ->
            [ "A hockey game is playing on several screens, and a few passengers quietly cheer whenever something happens."
              "The flight attendant switches effortlessly between English and French with each passenger." ]

    let private landing destination =
        [ $"The captain announces the descent into {Generic.cityName destination}, and the city lights slowly come into view."
          $"The wheels touch down with a firm thud and a few passengers clap as the plane taxis towards the gate in {Generic.cityName destination}."
          $"After a smooth landing, everyone stands up at the same time and waits in the aisle for the doors to open in {Generic.cityName destination}."
          $"The plane banks one last time over {Generic.cityName destination} before landing smoothly on the runway." ]

    /// Returns a description of what happens during the given flight and its
    /// landing.
    let inFlightMoment state (flight: Flight) =
        let currentDate = state |> Queries.Calendar.today
        let phase = SceneCaptions.phaseOf currentDate.DayMoment

        Common.paragraph
            [ Common.pickFrom [ cabin phase ]
              Common.pickFrom [ passengers flight.Destination; turbulence ]
              Common.pickFrom [ landing flight.Destination ] ]

    let private passportControl destination =
        match destination with
        | Berlin ->
            [ "The officer at passport control flips through every page of the passport in complete silence before stamping it with a decisive thump." ]
        | London ->
            [ "The queue for passport control snakes back and forth for what feels like an eternity, but the officer is surprisingly cheerful." ]
        | LosAngeles ->
            [ "The immigration officer asks about the purpose of the trip, and answering 'music' earns a long, skeptical look." ]
        | Madrid ->
            [ "Passport control is quick, with the officer barely looking up before waving everyone through with a bored 'vale'." ]
        | NewYork ->
            [ "The immigration officer takes fingerprints, a photo and asks three questions in rapid succession before waving the next person over." ]
        | Paris ->
            [ "Signs in French point in every direction and passport control takes a while, as only two booths out of ten are open." ]
        | Prague ->
            [ "Passport control is quiet and efficient, and the officer even manages a small smile while stamping the passport." ]
        | Santiago ->
            [ "Before leaving, every bag has to go through a scanner to make sure nobody is bringing fruit or seeds into the country." ]
        | Seoul ->
            [ "The automated gates at passport control scan the passport in seconds, and the airport is spotless and impressively organized." ]
        | Tokyo ->
            [ "Everything at passport control runs with clockwork precision, and the suitcases are already neatly lined up on the belt." ]
        | Toronto ->
            [ "A kiosk at the customs hall asks a few questions on a touchscreen before an officer gives a quick, friendly nod." ]

    let private airportExit destination =
        let cityName = Generic.cityName destination

        [ $"After grabbing the luggage, the automatic doors slide open into the arrivals hall of {cityName}."
          $"A crowd of drivers holding name signs waits at the exit, and the air outside already smells like {cityName}."
          $"Signs for taxis, trains and buses compete for attention on the way out into {cityName}."
          $"A short walk past duty-free shops and coffee stands leads to the exit and the streets of {cityName}." ]

    /// Returns a description of going through passport control and leaving the
    /// airport at the destination of the given flight.
    let arrivalMoment (flight: Flight) =
        Common.paragraph
            [ Common.pickFrom [ passportControl flight.Destination ]
              Common.pickFrom [ airportExit flight.Destination ] ]
