module Duets.Cli.Text.LanguageModelPrompts

open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation
open System

/// Creates a prompt that improves the response quality of the language model.
/// Currently tuned for Gemma 3, which requires explicit turn markers to get
/// anything useful out of it.
let private createPrompt prompt =
    $"""
<start_of_turn>user
{prompt}
<end_of_turn>
<start_of_turn>model
"""

let private placeTypeForPrompt (placeType: PlaceType) =
    match placeType with
    | Airport -> "an airport"
    | Bar -> "a bar"
    | Bookstore -> "a bookstore"
    | Cafe -> "a coffee shop"
    | Casino -> "a casino"
    | ConcertSpace concertSpace ->
        $"a concert space wich a capacity for {concertSpace.Capacity} people"
    | Gym -> "a gym"
    | Home -> "the character's home"
    | Hotel _ -> "a hotel"
    | Hospital -> "a hospital"
    | MerchandiseWorkshop -> "a place where the player can order merchandise"
    | MetroStation -> "a metro station"
    | RadioStudio radioStudio ->
        $"a radio station for {radioStudio.MusicGenre} music"
    | RehearsalSpace _ -> "a rehearsal space for bands"
    | Restaurant -> "a restaurant"
    | Street -> "a street"
    | Studio studio -> $"a studio whose producer is {studio.Producer.Name}"

let private placeNameWithType (place: Place) =
    match place.PlaceType with
    | Home -> "your home"
    | _ -> $"{place.Name} (which is {placeTypeForPrompt place.PlaceType})"

let private entrancesForPrompt interactions =
    let entrances =
        interactions
        |> List.choose (fun interaction ->
            match interaction.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Enter(place)) ->
                Some(place)
            | _ -> None)
        |> List.concat

    match entrances with
    | [] -> ""
    | entrances ->
        $"""(Entrances to: {Generic.listOf entrances placeNameWithType})"""

let private roomNameForPrompt place roomType interactions =
    match place.PlaceType with
    | Street -> entrancesForPrompt interactions
    | _ -> World.roomName roomType

let private itemName item =
    let mainProperty = item.Properties |> List.head

    match mainProperty with
    | Key(EntranceCard(cityId, placeId)) ->
        let place = Queries.World.placeInCityById cityId placeId
        $"entrance card for {place.Name}"
    | _ -> item.Name

let rec private npcsInRoom state =
    let knownNpcs, unknownNpcs = Queries.World.peopleInCurrentPlace state

    let knownNames =
        let npcsWithRelation =
            knownNpcs
            |> List.choose (fun npc ->
                Queries.Relationship.withCharacter npc.Id state
                |> Option.map (fun rel -> npc, rel))

        match npcsWithRelation with
        | [] -> "none"
        | _ ->
            Generic.listOf npcsWithRelation (fun (npc, rel) ->
                $"{npc.Name} ({Social.relationshipType rel.RelationshipType})")

    $"""
Known NPCs here: {knownNames}.
{unknownNpcs.Length} unknown NPCs here.
"""

let createRoomDescriptionPrompt state interactions =
    let coords = state |> Queries.World.currentCoordinates
    let cityId, _, _ = coords
    let currentPlace = state |> Queries.World.currentPlace
    let currentZone, street = state |> Queries.World.currentZoneCoordinates
    let currentRoom = state |> Queries.World.currentRoom
    let currentDate = state |> Queries.Calendar.today
    let currentWeather = state |> Queries.World.currentWeather

    let roomTypeSection =
        roomNameForPrompt currentPlace currentRoom.RoomType interactions

    let objectsInCurrentRoom =
        Queries.Items.allIn state coords |> List.map itemName

    let objectDescriptions =
        if objectsInCurrentRoom.IsEmpty then
            "none"
        else
            String.Join(", ", objectsInCurrentRoom)

    createPrompt
        $"""
You are an expert in role-playing and simulation games. You will be asked to generate
the descriptions of places based on a given context.

Rules:
- Keep it short and concise. Follow the style of classic text-based adventure games.
- Only mention the people that are physically present in the room, as listed below.
- Do not display any text other than the description itself. **Avoid any extra commentary or headers.**
- **Avoid** referring to the player or the character. Describe the environment using objective language (third person, or second person only for static features, e.g., 'A bar stands before you').
- Do not include any information that is not directly related to the place being described.
- Do not include sensory details or feelings of the player or character.
- Do not format the text in any way, specially do not add extra spaces or line breaks unless there's a new paragraph.
- **Crucially, ensure the description's tone and lighting reflect the current Day Moment, Season and Weather.**

--- Context for Description ---
The player is in the {roomTypeSection} of **{currentPlace.Name}** (which is a **{placeTypeForPrompt currentPlace.PlaceType}**) (quality: {currentPlace.Quality}), which is
located in the city of **{cityId}**, in the zone of {currentZone.Name}.
Current date: **{currentDate.Day}** of **{currentDate.Season}**, in the year **{currentDate.Year}**, currently in the **{currentDate.DayMoment}**.
Weather: **{currentWeather}** outside.
     
--- Context for the current place ---
{currentPlace.PromptContext}

--- Items in the place ---
{objectDescriptions}.
        
--- NPCs in the place ---
{npcsInRoom state}

**Provide the generated description and *only* the description.**
"""

let createDuberDriverConversationPrompt
    state
    (driverName: string)
    (destinationName: string)
    =
    let currentPlace = state |> Queries.World.currentPlace
    let currentCity = state |> Queries.World.currentCity
    let currentDate = state |> Queries.Calendar.today
    let currentWeather = state |> Queries.World.currentWeather
    let character = state |> Queries.Characters.playableCharacter

    createPrompt
        $"""
You are {driverName}, a friendly Duber (ride-sharing) driver in {currentCity.Id}. You're currently driving a passenger (a musician named {character.Name}) to {destinationName}.

Rules:
- Generate a single, short line of casual conversation (1-2 sentences max).
- Stay in character as a driver - you might comment on traffic, the city, music, weather, or make small talk.
- Keep it natural and conversational, like real driver small talk.
- **Do not** use quotation marks, asterisks, or any formatting.
- **Do not** include the driver's name or any labels like "Driver:" - just the dialogue itself.
- Match the tone to the current context (time of day, weather, etc).

Context:
- Time: {currentDate.DayMoment} in {currentDate.Season}
- Weather: {currentWeather}
- Pickup location: {currentPlace.Name}
- Destination: {destinationName}
- City: {currentCity.Id}

Generate the driver's conversational line:
"""
