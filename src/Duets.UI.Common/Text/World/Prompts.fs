module Duets.UI.Common.Text.World.Prompts

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.UI.Common.Text.World.Cities

let private cityName = name

let private wrapPrompt prompt =
    $"""
<start_of_turn>user
{prompt}
<end_of_turn>
<start_of_turn>model
"""

let private roomName (room: RoomType) =
    match room with
    | RoomType.Backstage -> "backstage"
    | RoomType.Bar -> "bar area"
    | RoomType.Bedroom -> "bedroom"
    | RoomType.BoardingGate -> "boarding gate"
    | RoomType.Cafe -> "cafe area"
    | RoomType.CasinoFloor -> "casino floor"
    | RoomType.ChangingRoom -> "changing room"
    | RoomType.Gym -> "gym"
    | RoomType.Kitchen -> "kitchen"
    | RoomType.LivingRoom -> "living room"
    | RoomType.Lobby -> "lobby"
    | RoomType.MasteringRoom -> "mastering room"
    | RoomType.Platform -> "platform"
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.ReadingRoom -> "reading room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant _ -> "restaurant"
    | RoomType.ScreeningRoom -> "screening room"
    | RoomType.SecurityControl -> "security control"
    | RoomType.ShowRoom -> "show room"
    | RoomType.Stage -> "stage"
    | RoomType.Street -> "street"
    | RoomType.Workshop -> "workshop"

let private relationshipType =
    function
    | Friend -> "Friend"
    | Bandmate -> "Bandmate"

let private placeTypeForPrompt (placeType: PlaceType) =
    match placeType with
    | Airport -> "an airport"
    | Bar -> "a bar"
    | Bookstore -> "a bookstore"
    | Cafe -> "a coffee shop"
    | CarDealer _ -> "a car dealer"
    | Casino -> "a casino"
    | Cinema -> "a cinema"
    | ConcertSpace concertSpace ->
        $"a concert space with a capacity for {concertSpace.Capacity} people"
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
            | Interaction.FreeRoam(FreeRoamInteraction.Enter(place)) -> Some(place)
            | _ -> None)
        |> List.concat

    match entrances with
    | [] -> ""
    | entrances ->
        let names = entrances |> List.map placeNameWithType |> String.concat ", "
        $"(Entrances to: {names})"

let private itemNameForPrompt item =
    let mainProperty = item.Properties |> List.head

    match mainProperty with
    | Key(MovieTicket _) -> "movie ticket"
    | Key(EntranceCard _) -> "entrance card"
    | Rideable(RideableItem.Car _) -> $"{item.Brand} {item.Name}"
    | _ -> item.Name |> String.lowercase

let private npcsInRoom state =
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
            npcsWithRelation
            |> List.map (fun (npc, rel) ->
                $"{npc.Name} ({relationshipType rel.RelationshipType})")
            |> String.concat ", "

    $"""
{if knownNpcs.Length > 0 then $"Known people: {knownNames}" else ""}
{if unknownNpcs.Length > 0 then $"Number of unknown people: {unknownNpcs.Length}" else ""}
"""
    |> String.trim

let private roomNameForPrompt place roomType interactions =
    match place.PlaceType, roomType with
    | Street, _ -> entrancesForPrompt interactions
    | Cinema, RoomType.Lobby ->
        "cinema lobby with a ticket counter and a stand for buying pop-corn and other snacks and drinks"
    | Cinema, RoomType.ScreeningRoom ->
        "movie theater auditorium with rows of seats, a large screen, and a projector"
    | _ -> roomName roomType

let private qualityDescription (q: int<quality>) =
    if q < 20<quality> then
        "very poor (dilapidated, neglected, in serious disrepair)"
    elif q < 40<quality> then
        "poor (run-down, worn-out, poorly maintained)"
    elif q < 60<quality> then
        "average (functional but unremarkable, showing signs of wear)"
    elif q < 80<quality> then
        "good (well-maintained, clean, comfortable)"
    else
        "excellent (pristine, luxurious, impeccably maintained)"

let private createStreetPrompt
    interactions
    cityId
    (currentPlace: Place)
    (currentZone: Zone)
    currentDate
    currentWeather
    (objectsInCurrentRoom: string list)
    npcsPrompt
    =
    let entrances = entrancesForPrompt interactions

    let objectDescriptions =
        if objectsInCurrentRoom.IsEmpty then
            "none"
        else
            objectsInCurrentRoom |> String.concat ", "

    wrapPrompt
        $"""
You are an expert in role-playing and simulation games. You will be asked to generate
the descriptions of outdoor street locations based on a given context.

Rules:
- Keep the descriptions in the style of text-based adventures and do not make them more than a paragraph long.
- Focus on the outdoor environment, architecture, atmosphere and street features.
- Mention visible entrances to establishments as part of the street description.
- Only mention the people that are physically present on the street, as listed below.
- Do not display any text other than the description itself. **Avoid any extra commentary or headers.**
- **Avoid** referring to the player or the character. Describe the environment using objective language.
- Do not include any information that is not directly related to the street being described.
- Do not include sensory details or feelings of the player or character.
- Do not format the text in any way, specially do not add extra spaces or line breaks.
- **Ensure the description's tone and lighting reflect the current Day Moment, Season and Weather.**

--- Context for Description ---
The player is on the street called **{currentPlace.Name}** located in the city of **{cityName cityId}**, in the zone of {currentZone.Name}.
Current date: **{currentDate.Day}** of **{currentDate.Season}**, in the year **{currentDate.Year}**, currently in the **{currentDate.DayMoment}**.
Weather: **{currentWeather}**.

--- Visible entrances on this street ---
{if entrances = "" then "No visible entrances to establishments" else entrances}

--- Context for the street ---
{currentPlace.PromptContext}

--- Items on the street ---
{objectDescriptions}.

{if npcsPrompt = "" then "There are no other people on this street" else $"--- Other people on the street ---\n{npcsPrompt}"}

**Provide the generated description and *only* the description.**
"""

let private createPlacePrompt
    interactions
    (currentPlace: Place)
    currentRoom
    cityId
    (currentZone: Zone)
    currentDate
    currentWeather
    (objectsInCurrentRoom: string list)
    npcsPrompt
    =
    let roomTypeName = roomNameForPrompt currentPlace currentRoom.RoomType interactions

    let objectDescriptions =
        if objectsInCurrentRoom.IsEmpty then
            "none"
        else
            objectsInCurrentRoom |> String.concat ", "

    wrapPrompt
        $"""
You are an expert in role-playing and simulation games. You will be asked to generate
the descriptions of indoor places based on a given context.

Rules:
- Keep the descriptions in the style of text-based adventures and do not make them more than a paragraph long.
- Focus on the interior environment, decor, furnishings and atmosphere.
- Only mention the people that are physically present in the room, as listed below.
- Do not display any text other than the description itself. **Avoid any extra commentary or headers.**
- **Avoid** referring to the player or the character. Describe the environment using objective language.
- Do not include any information that is not directly related to the place being described.
- Do not include sensory details or feelings of the player or character.
- Do not format the text in any way, specially do not add extra spaces or line breaks.
- **Ensure the description's tone and lighting reflect the current Day Moment and Season.**
- **The description must reflect the quality level of the place. Low quality places should be described as shabby, dirty, or in disrepair, while high quality places should be described as clean, well-maintained, or luxurious.**

--- Context for Description ---
The player is in the {roomTypeName} of {placeTypeForPrompt currentPlace.PlaceType} named **{currentPlace.Name}**. The quality of the place is {qualityDescription currentPlace.Quality}. It is
located in the city of **{cityName cityId}**, in the zone of {currentZone.Name}.
Current date: **{currentDate.Day}** of **{currentDate.Season}**, in the year **{currentDate.Year}**, currently in the **{currentDate.DayMoment}**.
Weather outside: **{currentWeather}**.

--- Context for the current place ---
{currentPlace.PromptContext}

--- Items in the place ---
{objectDescriptions}.

{if npcsPrompt = "" then "There are no other people in this place" else $"--- Other people in the place ---\n{npcsPrompt}"}

**Provide the generated description and *only* the description.**
"""

/// Generates a room description prompt for the language model given the current
/// game state and available interactions.
let createRoomDescriptionPrompt state interactions =
    let coords = state |> Queries.World.currentCoordinates
    let cityId, _, _ = coords
    let currentPlace = state |> Queries.World.currentPlace
    let currentZone, _ = state |> Queries.World.currentZoneCoordinates
    let currentRoom = state |> Queries.World.currentRoom
    let currentDate = state |> Queries.Calendar.today
    let currentWeather = state |> Queries.World.currentWeather

    let objectsInCurrentRoom =
        Queries.Items.allIn state coords |> List.map itemNameForPrompt

    let npcsPrompt = npcsInRoom state

    match currentPlace.PlaceType with
    | Street ->
        createStreetPrompt
            interactions
            cityId
            currentPlace
            currentZone
            currentDate
            currentWeather
            objectsInCurrentRoom
            npcsPrompt
    | _ ->
        createPlacePrompt
            interactions
            currentPlace
            currentRoom
            cityId
            currentZone
            currentDate
            currentWeather
            objectsInCurrentRoom
            npcsPrompt
