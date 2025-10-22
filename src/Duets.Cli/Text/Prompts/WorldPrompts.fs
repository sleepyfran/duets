namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Common
open Duets.Entities
open Duets.Simulation
open System

[<RequireQualifiedAccess>]
module World =
    let private placeTypeForPrompt (placeType: PlaceType) =
        match placeType with
        | Airport -> "an airport"
        | Bar -> "a bar"
        | Bookstore -> "a bookstore"
        | Cafe -> "a coffee shop"
        | CarDealer _ -> "a car dealer"
        | Casino -> "a casino"
        | ConcertSpace concertSpace ->
            $"a concert space wich a capacity for {concertSpace.Capacity} people"
        | Gym -> "a gym"
        | Home -> "the character's home"
        | Hotel _ -> "a hotel"
        | Hospital -> "a hospital"
        | MerchandiseWorkshop ->
            "a place where the player can order merchandise"
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
    {if knownNpcs.Length > 0 then
         $"Known people: {knownNames}"
     else
         ""}
    {if unknownNpcs.Length > 0 then
         $"Number of unknown people: {unknownNpcs.Length}"
     else
         ""}
    """
        |> String.trim

    let createRoomDescriptionPrompt state interactions =
        let coords = state |> Queries.World.currentCoordinates
        let cityId, _, _ = coords
        let currentPlace = state |> Queries.World.currentPlace
        let currentZone, _ = state |> Queries.World.currentZoneCoordinates
        let currentRoom = state |> Queries.World.currentRoom
        let currentDate = state |> Queries.Calendar.today
        let currentWeather = state |> Queries.World.currentWeather

        let roomTypeSection =
            roomNameForPrompt currentPlace currentRoom.RoomType interactions

        let objectsInCurrentRoom =
            Queries.Items.allIn state coords |> List.map itemName

        let npcsPrompt = npcsInRoom state

        let objectDescriptions =
            if objectsInCurrentRoom.IsEmpty then
                "none"
            else
                String.Join(", ", objectsInCurrentRoom)

        let qualityDescription =
            match currentPlace.Quality with
            | q when q < 20<quality> ->
                "very poor (dilapidated, neglected, in serious disrepair)"
            | q when q < 40<quality> ->
                "poor (run-down, worn-out, poorly maintained)"
            | q when q < 60<quality> ->
                "average (functional but unremarkable, showing signs of wear)"
            | q when q < 80<quality> ->
                "good (well-maintained, clean, comfortable)"
            | _ -> "excellent (pristine, luxurious, impeccably maintained)"

        Common.createPrompt
            $"""
    You are an expert in role-playing and simulation games. You will be asked to generate
    the descriptions of places based on a given context.
    
    Rules:
    - Keep the descriptions in the style of text-based adventures and do not make them more than a paragraph long.
    - Only mention the people that are physically present in the room, as listed below.
    - Do not display any text other than the description itself. **Avoid any extra commentary or headers.**
    - **Avoid** referring to the player or the character. Describe the environment using objective language.
    - Do not include any information that is not directly related to the place being described.
    - Do not include sensory details or feelings of the player or character.
    - Do not format the text in any way, specially do not add extra spaces or line breaks.
    - **Ensure the description's tone and lighting reflect the current Day Moment, Season and Weather.**
    - **The description must reflect the quality level of the place. Low quality places should be described as shabby, dirty, or in disrepair, while high quality places should be described as clean, well-maintained, or luxurious.**
    
    --- Context for Description ---
    The player is in the {roomTypeSection} of {placeTypeForPrompt currentPlace.PlaceType} named **{currentPlace.Name}**. The quality of the place is {qualityDescription}. It is
    located in the city of **{cityId |> Generic.cityName}**, in the zone of {currentZone.Name}.
    Current date: **{currentDate.Day}** of **{currentDate.Season}**, in the year **{currentDate.Year}**, currently in the **{currentDate.DayMoment}**.
    Weather: **{currentWeather}** outside.
         
    --- Context for the current place ---
    {currentPlace.PromptContext}
    
    --- Items in the place ---
    {objectDescriptions}.
            
    {if npcsPrompt = "" then
         "There are no other people in this place"
     else
         $"--- Other people in the place ---\n{npcsPrompt}"}
    
    **Provide the generated description and *only* the description.**
    """
