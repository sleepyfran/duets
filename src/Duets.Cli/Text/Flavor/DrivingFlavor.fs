namespace Duets.Cli.Text.Flavor

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Driving =
    let private carName (car: Item) = $"{car.Brand} {car.Name}"

    let private carPower (car: Item) =
        car.Properties
        |> List.tryPick (fun property ->
            match property with
            | Rideable(RideableItem.Car car) -> Some car.Power
            | _ -> None)
        |> Option.defaultValue 0<horsepower>

    let private carMoment (car: Item) =
        let name = carName car

        match carPower car with
        | power when power < 120<horsepower> ->
            [ $"The {name} wheezes a bit at every green light, but it gets there."
              $"The engine of the {name} hums modestly as it merges into traffic."
              $"Someone honks as the {name} takes its time getting up to speed." ]
        | power when power < 250<horsepower> ->
            [ $"The {name} handles the traffic with ease, gliding from lane to lane."
              $"The {name} purrs along comfortably, the radio playing softly in the background." ]
        | _ ->
            [ $"The {name} growls at every light, turning a few heads on the sidewalk."
              $"Pedestrians stop to stare as the {name} roars past them."
              $"It takes real restraint to keep the {name} under the speed limit." ]

    let private cityMoment weather phase =
        match weather, phase with
        | WeatherCondition.Rainy, _ ->
            [ "The wipers squeak back and forth as rain blurs the traffic lights into smears of red and green."
              "Puddles splash against the curb as the car rolls through the wet streets." ]
        | WeatherCondition.Stormy, _ ->
            [ "Thunder rumbles overhead and the rain hammers so hard on the roof that the radio is barely audible."
              "A gust of wind pushes the car sideways for a moment while lightning flashes over the rooftops." ]
        | WeatherCondition.Snowy, _ ->
            [ "Snowflakes swirl in the headlights and the tires crunch carefully over the slushy road."
              "Traffic crawls along the snowy streets, everyone driving a little more carefully than usual." ]
        | _, SceneCaptions.Dawn ->
            [ "The streets are still half asleep, with only delivery vans and early joggers sharing the road."
              "The first rays of the sun bounce off the windshield as the city slowly wakes up." ]
        | _, SceneCaptions.Day ->
            [ "Traffic stops and starts as cyclists weave between the cars and a bus blocks half the lane."
              "A street musician plays on a corner, the melody drifting in through the open window for a moment."
              "A billboard for a new album catches your eye at a red light, and you can't help wondering what the band paid for it." ]
        | _, SceneCaptions.Dusk ->
            [ "The streetlights flicker on one by one as the evening rush hour slowly thins out."
              "The sunset reflects off the glass towers and everyone on the road seems to be heading home." ]
        | _, SceneCaptions.Dark ->
            [ "Neon signs and late-night bars blur past as the nearly empty streets open up ahead."
              "Taxis and night buses are the only company on the road at this hour." ]

    let private radioMoment =
        [ "The radio plays a song you swear you've heard in every bar in town."
          "A radio host rambles about a local band's upcoming show before cutting to commercials."
          "Flicking through the radio stations lands on an old classic, and singing along is unavoidable." ]

    /// Returns a description of a moment while driving within the current city
    /// towards the given destination.
    let withinCityMoment state destinationName (car: Item) =
        let currentDate = state |> Queries.Calendar.today
        let weather = state |> Queries.World.currentWeather
        let phase = SceneCaptions.phaseOf currentDate.DayMoment

        Common.paragraph
            [ Common.pickFrom [ cityMoment weather phase ]
              Common.pickFrom [ carMoment car; radioMoment ]
              $"Not long now until {destinationName}." ]

    let private departure originName =
        [ $"The skyline of {originName} slowly shrinks in the rear-view mirror."
          $"Leaving the traffic of {originName} behind, the road opens up into the motorway."
          $"After a last roundabout on the outskirts of {originName}, the motorway stretches out ahead." ]

    let private road distance phase =
        let distancePool =
            if distance > 800<km> then
                [ "Hours blur together between service stations, bad coffee and endless kilometres of asphalt."
                  "The landscape changes several times along the way: fields, forests, hills and small towns flash by."
                  "A long stop at a petrol station, a stretch of the legs and a questionable sandwich break up the trip." ]
            else
                [ "Fields and small towns slide past the window as the kilometres tick away."
                  "A quick stop at a service station for petrol and a snack, and back on the road." ]

        let phasePool =
            match phase with
            | SceneCaptions.Dawn ->
                [ "The sun rises slowly over the motorway, painting the road ahead in gold." ]
            | SceneCaptions.Day ->
                [ "Lorries rumble along in the slow lane while the car cruises past them." ]
            | SceneCaptions.Dusk ->
                [ "The sun sets in the side window, stretching the shadows of the trees across the road." ]
            | SceneCaptions.Dark ->
                [ "Only the headlights and the reflective road markings light the way through the dark." ]

        distancePool @ phasePool

    let private arrival destinationName =
        [ $"Finally, a road sign announces {destinationName} and the traffic starts to thicken."
          $"The lights of {destinationName} appear on the horizon after the last bend."
          $"The motorway narrows into busy streets as {destinationName} welcomes you." ]

    /// Returns a description of a moment while driving between the two given
    /// cities.
    let intercityMoment state originCityId destinationCityId (car: Item) =
        let currentDate = state |> Queries.Calendar.today
        let phase = SceneCaptions.phaseOf currentDate.DayMoment
        let originName = Generic.cityName originCityId
        let destinationName = Generic.cityName destinationCityId

        let distance =
            Duets.Data.World.World.distanceBetween
                originCityId
                destinationCityId

        Common.paragraph
            [ Common.pickFrom [ departure originName ]
              Common.pickFrom [ road distance phase; carMoment car ]
              Common.pickFrom [ arrival destinationName ] ]
