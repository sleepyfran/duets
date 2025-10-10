namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module CarDealer =
    let createGreetingPrompt
        state
        (carDealer: CarDealer)
        (availableCars: PurchasableItem list)
        =
        let currentCity = state |> Queries.World.currentCity
        let currentPlace = state |> Queries.World.currentPlace
        let currentDate = state |> Queries.Calendar.today

        let priceRangeDescription =
            match carDealer.PriceRange with
            | CarPriceRange.Budget -> "budget-friendly, practical vehicles"
            | CarPriceRange.MidRange ->
                "quality mid-range vehicles with premium features"
            | CarPriceRange.Premium ->
                "luxury, high-performance premium vehicles"

        let formattedItems =
            availableCars
            |> List.map (fun (item, _) -> $"{item.Brand} {item.Name}")
            |> String.join ","

        Common.createPrompt
            $"""
    You are {carDealer.Dealer.Name}, an enthusiastic car salesperson at a dealership called {currentPlace.Name} in {currentCity.Id |> Generic.cityName}
    that specializes in {priceRangeDescription}. A customer has just walked into the showroom.
    
    Rules:
    - Generate a single, natural greeting and opening line (2-3 sentences max).
    - Be professional but friendly - you're trying to make a sale.
    - You might comment on the vehicles, ask about their needs, or make them feel welcome.
    - Keep it conversational and realistic for a car salesperson.
    - **Do not** use quotation marks, asterisks, or any formatting.
    - **Do not** include the salesperson's name or any labels like "Salesperson:" - just the dialogue itself.
    - Match the tone to selling {priceRangeDescription}.
    
    Context:
    - Time: {currentDate.DayMoment} in {currentDate.Season}
    - Dealership type: {priceRangeDescription}
    - City: {currentCity.Id |> Generic.cityName}
    - Available models: {formattedItems}
    
    Generate the salesperson's greeting:
    """

    let createPitchPrompt
        state
        (carDealer: CarDealer)
        (car: Item)
        (price: Amount)
        =
        let character = state |> Queries.Characters.playableCharacter
        let currentCity = state |> Queries.World.currentCity

        Common.createPrompt
            $"""
    You are {carDealer.Dealer.Name}, a car salesperson in {currentCity.Id |> Generic.cityName}. The customer ({character.Name}) is interested in a {car.Brand} {car.Name} priced at {price}.
    
    Rules:
    - Generate a brief sales pitch (2-3 sentences) highlighting the car's appeal.
    - Be persuasive but not pushy - focus on features, style, performance, or lifestyle fit.
    - Keep it natural and conversational.
    - **Do not** use quotation marks, asterisks, or any formatting.
    - **Do not** include the salesperson's name or labels - just the dialogue itself.
    
    Generate the sales pitch for this specific car:
    """

    let createClosingPrompt
        (carDealer: CarDealer)
        (car: Item)
        (accepted: bool)
        =
        let context =
            if accepted then
                "The customer has decided to purchase the vehicle"
            else
                "The customer has decided not to purchase the vehicle today"

        Common.createPrompt
            $"""
    You are {carDealer.Dealer.Name}, a car salesperson. {context} - the {car.Brand} {car.Name}.
    
    Rules:
    - Generate a brief, natural closing line (1-2 sentences).
    - If they bought: be congratulatory and professional, mention they'll find the car outside.
    - If they didn't buy: be understanding and leave the door open for future business.
    - Keep it short and realistic.
    - **Do not** use quotation marks, asterisks, or any formatting.
    - **Do not** include the salesperson's name or labels - just the dialogue itself.
    
    Generate the closing line:
    """

    let createInsufficientFundsPrompt
        state
        (carDealer: CarDealer)
        (car: Item)
        (price: Amount)
        =
        let character = state |> Queries.Characters.playableCharacter
        let currentCity = state |> Queries.World.currentCity

        let dealershipTone =
            match carDealer.PriceRange with
            | CarPriceRange.Budget -> "understanding but slightly disappointed"
            | CarPriceRange.MidRange ->
                "professional but noticeably condescending"
            | CarPriceRange.Premium ->
                "highly condescending and subtly shaming, making it clear this customer doesn't belong here"

        Common.createPrompt
            $"""
    You are {carDealer.Dealer.Name}, a car salesperson in {currentCity.Id |> Generic.cityName}. The customer ({character.Name}) wanted to purchase a {car.Brand} {car.Name} priced at {price}, but their payment was declined - they don't have enough money.
    
    Rules:
    - Generate a brief response (2-3 sentences) to this awkward situation.
    - Tone should be {dealershipTone}.
    - For budget dealerships: be sympathetic, suggest saving up or looking at financing options.
    - For mid-range dealerships: be politely condescending, perhaps suggest they look at "more suitable options" or come back when they're "ready."
    - For premium dealerships: be cutting and subtly cruel - imply they're wasting your time, question why they're even here, make them feel out of place. Use phrases like "perhaps this isn't the right establishment for you" or "we cater to a certain clientele."
    - Keep it realistic for a salesperson (they won't be overtly rude, but the disdain should be clear in premium stores).
    - **Do not** use quotation marks, asterisks, or any formatting.
    - **Do not** include the salesperson's name or labels - just the dialogue itself.
    
    Generate the salesperson's response to the declined payment:
    """
