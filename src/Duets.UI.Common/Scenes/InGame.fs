module Duets.UI.Common.Scenes.InGame

open Duets.Agents
open Duets.Entities
open Duets.Simulation
open Duets.UI.Common

let private attributeName =
    function
    | CharacterAttribute.Drunkenness -> "Drunkenness"
    | CharacterAttribute.Energy -> "Energy"
    | CharacterAttribute.Fame -> "Fame"
    | CharacterAttribute.Health -> "Health"
    | CharacterAttribute.Hunger -> "Hunger"
    | CharacterAttribute.Mood -> "Mood"

let private interactionLabel
    (displayLabel: InteractionWithMetadata -> string)
    (item: InteractionWithMetadata)
    : string =
    let baseLabel = displayLabel item

    match item.State with
    | InteractionState.Enabled -> baseLabel
    | InteractionState.Disabled(InteractionDisabledReason.NotEnoughAttribute(attr, amount)) ->
        $"{baseLabel} (needs {attributeName attr}: {amount})"

let private showRoomInfo state interactions : Scene<unit> =
    scene {
        do! showText (Text.World.Places.roomConnections state interactions)

        match Text.World.Places.roomEntrances interactions with
        | Some msg -> do! showText msg
        | None -> ()

        match Text.World.Places.roomExits state interactions with
        | Some msg -> do! showText msg
        | None -> ()
    }

let private lookHandler
    state
    interactions
    (items: Item list)
    (knownPeople: Character list)
    (unknownPeople: Character list)
    : Scene<unit> =
    scene {
        let prompt = Text.World.Prompts.createRoomDescriptionPrompt state interactions
        let stream = LanguageModel.streamMessage prompt
        do! show (ShowContent.LLMStream stream)

        do! showRoomInfo state interactions

        let room = Queries.World.currentRoom state

        if not knownPeople.IsEmpty then
            let names = knownPeople |> List.map (fun p -> p.Name) |> String.concat ", "
            do! showText $"{names} are in the {Text.World.Places.roomName room.RoomType}."

        if not unknownPeople.IsEmpty then
            do! showText $"There are {unknownPeople.Length} people you don't know here."

        match items with
        | [] -> do! showText "There are no objects around."
        | items ->
            let names = items |> List.map (fun i -> i.Name) |> String.concat ", "
            do! showText $"You can see: {names}."
    }

let private handleInteraction
    state
    interactions
    (item: InteractionWithMetadata)
    : Scene<Navigate option> =
    match item.Interaction with
    | Interaction.FreeRoam(FreeRoamInteraction.Look(items, knownPeople, unknownPeople)) ->
        scene {
            do! lookHandler state interactions items knownPeople unknownPeople
            return None
        }
    | Interaction.Airport _ -> scene { return None }
    | Interaction.Career _ -> scene { return None }
    | Interaction.Cinema _ -> scene { return None }
    | Interaction.Concert _ -> scene { return None }
    | Interaction.FreeRoam _ -> scene { return None }
    | Interaction.Gym _ -> scene { return None }
    | Interaction.Item _ -> scene { return None }
    | Interaction.MerchandiseWorkshop _ -> scene { return None }
    | Interaction.MiniGame _ -> scene { return None }
    | Interaction.Rehearsal _ -> scene { return None }
    | Interaction.Shop _ -> scene { return None }
    | Interaction.Social _ -> scene { return None }
    | Interaction.Studio _ -> scene { return None }
    | Interaction.Travel _ -> scene { return None }

let scene (displayLabel: InteractionWithMetadata -> string) : Scene<Navigate> =
    let labelFn = interactionLabel displayLabel

    scene {
        let mutable next: Navigate option = None

        while next.IsNone do
            let state = State.get ()
            let place = Queries.World.currentPlace state
            let room = Queries.World.currentRoom state

            do! showSep None
            do! showText (Text.World.Places.youAreIn state place room.RoomType)
            let interactions = Queries.Interactions.availableCurrently state
            do! showRoomInfo state interactions

            let! picked = askChoice interactions labelFn
            let! nav = handleInteraction state interactions picked
            next <- nav

        return next.Value
    }
