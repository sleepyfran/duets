namespace Duets.Cli.Text.Flavor

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module CarDealer =
    let private carName (car: Item) = $"{car.Brand} {car.Name}"

    let private salutation phase =
        match phase with
        | SceneCaptions.Dawn
        | SceneCaptions.Day -> "Good day!"
        | SceneCaptions.Dusk
        | SceneCaptions.Dark -> "Good evening!"

    /// Returns the greeting of the dealer when the player walks into the
    /// dealership.
    let greeting state (carDealer: CarDealer) =
        let currentDate = state |> Queries.Calendar.today
        let currentPlace = state |> Queries.World.currentPlace
        let phase = SceneCaptions.phaseOf currentDate.DayMoment

        let opening =
            Common.pickFrom
                [ [ salutation phase
                    $"Welcome to {currentPlace.Name}!"
                    "Come on in, come on in!" ] ]

        let pitch =
            match carDealer.PriceRange with
            | CarPriceRange.Budget ->
                [ "Everything here is reliable, affordable and ready to drive off today."
                  "Looking for something that gets you from A to B without emptying your wallet? You're in the right place."
                  "No frills, no fuss, just honest cars at honest prices." ]
            | CarPriceRange.MidRange ->
                [ "We've got a great selection of cars with all the comforts, without the luxury price tag."
                  "Whether it's for the daily commute or a weekend trip, we've got something that'll fit you perfectly."
                  "Quality, comfort and a bit of style. Let me know what you're looking for." ]
            | CarPriceRange.Premium ->
                [ "Every vehicle in our showroom is a statement. Please, take your time."
                  "May I offer you an espresso while you browse our collection?"
                  "Our clients expect nothing but the finest, and that is exactly what we offer." ]

        Common.paragraph [ opening; Common.pickFrom [ pitch ] ]

    /// Returns the sales pitch of the dealer for the given car.
    let pitch (carDealer: CarDealer) (car: Item) =
        let name = carName car

        match carDealer.PriceRange with
        | CarPriceRange.Budget ->
            [ $"The {name} is a great choice. Cheap to run, easy to park and it'll never let you down."
              $"Honestly? The {name} is the best value on the lot. You won't find a better deal in town."
              $"The {name} might not win any races, but it'll get your gear to every gig on time." ]
        | CarPriceRange.MidRange ->
            [ $"Ah, the {name}. Comfortable, well equipped and with plenty of space in the back for your instruments."
              $"The {name} is one of our best sellers. Smooth ride, great sound system, and it looks great too."
              $"You've got a good eye. The {name} is the perfect balance between practicality and style." ]
        | CarPriceRange.Premium ->
            [ $"The {name}. Hand-finished interior, breathtaking performance. It's not a car, it's an experience."
              $"Excellent taste. The {name} turns heads everywhere it goes, as it should."
              $"Few people get to drive a {name}. Even fewer deserve to. I suspect you might be one of them." ]
        |> List.singleton
        |> Common.pickFrom

    /// Returns the closing line of the dealer after the player either bought
    /// the given car or decided not to.
    let closing (car: Item) purchased =
        let name = carName car

        if purchased then
            [ $"Congratulations! The {name} is all yours. It's waiting for you right outside."
              $"Pleasure doing business with you! Enjoy the {name}, she's parked right outside."
              $"Here are the keys. Take good care of the {name}, and drive safe!" ]
        else
            [ $"No problem at all. The {name} will be here if you change your mind."
              "Take your time, it's a big decision. Our doors are always open."
              "Not today? That's fine. Come back anytime, we'll have something for you." ]
        |> List.singleton
        |> Common.pickFrom

    /// Returns the reaction of the dealer when the player can't afford the
    /// given car.
    let insufficientFunds (carDealer: CarDealer) (car: Item) =
        let name = carName car

        match carDealer.PriceRange with
        | CarPriceRange.Budget ->
            [ "Ah, it happens to the best of us. Maybe save up a little and come back, the car's not going anywhere."
              $"Don't worry about it. We could look at something a bit cheaper than the {name} if you'd like."
              "Tight month? I get it. Come back when things look better, we'll sort you out." ]
        | CarPriceRange.MidRange ->
            [ "I see. Perhaps we should look at some more... suitable options for your budget."
              $"Well, the {name} might be a little ambitious for now. Do come back when you're ready."
              "Hmm. Maybe the budget dealership down the road would be a better fit for you at the moment." ]
        | CarPriceRange.Premium ->
            [ "I'm afraid this establishment caters to a certain clientele. Perhaps this isn't the right place for you."
              $"Declined. How... unfortunate. The {name} is for people who can actually afford it."
              "I'll ask security to show you the way out. Do mind the cars on the way, please don't touch anything." ]
        |> List.singleton
        |> Common.pickFrom
