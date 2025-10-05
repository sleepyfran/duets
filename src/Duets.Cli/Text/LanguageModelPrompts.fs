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

let private placeNameWithType (place: Place) =
    match place.PlaceType with
    | Home -> "your home"
    | _ -> $"{place.Name} ({place |> World.placeTypeName'})"

let private entrancesForPrompt interactions =
    let entrances =
        interactions
        |> List.choose (fun interaction ->
            match interaction.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Enter(place)) ->
                Some(place)
            | _ -> None)

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
    let currentRoom = state |> Queries.World.currentRoom
    let currentDate = state |> Queries.Calendar.today

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
- Keep it short and concise, no more than a paragraph. Follow the style of classic text-based adventure games.
- Only mention the people and items that are physically present in the room, as listed below.
- Do not display any text other than the description itself. **Avoid any extra commentary or headers.**
- **Avoid** referring to the player or the character. Describe the environment using objective language (third person, or second person only for static features, e.g., 'A bar stands before you').
- Do not include any information that is not directly related to the place being described.
- Do not include sensory details or feelings of the player or character.
- Do not format the text in any way, specially do not add extra spaces or line breaks unless there's a new paragraph.
- **Crucially, ensure the description's tone and lighting reflect the current Day Moment and Season.**

--- Context for Description ---
The player is inside **{currentPlace.Name}** (quality: {currentPlace.Quality}) {roomTypeSection}, which is
located in the city of **{cityId}**. It's the day **{currentDate.Day}** of **{currentDate.Season}**,
in the year **{currentDate.Year}**, currently in the **{currentDate.DayMoment}**.
     
--- Context for the current place ---
{currentPlace.PromptContext}

--- Items in the place ---
{objectDescriptions}.
        
--- NPCs in the place ---
{npcsInRoom state}

**Provide the generated description and *only* the description.**
"""
