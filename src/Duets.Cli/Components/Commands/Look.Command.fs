namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module LookCommand =
    /// Lists all people in the room.
    let private listPeople
        (knownPeople: Character list)
        (unknownPeople: Character list)
        roomType
        =
        let peopleDescription people =
            Generic.listOf people (fun person ->
                $"{person.Name |> Styles.person}")

        if knownPeople.IsEmpty |> not then
            $"""{peopleDescription knownPeople} {Generic.pluralOf "is" "are" knownPeople.Length} in the {World.roomName roomType}."""
            |> showMessage

        if unknownPeople.IsEmpty |> not then
            $"""There {Generic.pluralOf "is" "are" unknownPeople.Length} {unknownPeople.Length |> Styles.person} {Generic.pluralOf "person" "people" unknownPeople.Length |> Styles.person} you don't know in the {World.roomName roomType}."""
            |> showMessage

    /// Lists all items in the room.
    let private listItems items =
        match items with
        | [] -> Command.lookNoObjectsAround |> showMessage
        | items ->
            items
            |> List.map (fun item -> Generic.itemName item |> Items.lookItem)
            |> Items.lookItems
            |> showMessage

    let create
        (interactions: InteractionWithMetadata list)
        (items: Item list)
        (knownPeople: Character list)
        (unknownPeople: Character list)
        =
        { Name = "look"
          Description = Command.lookDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentRoom = state |> Queries.World.currentRoom

                Prompts.World.createRoomDescriptionPrompt state interactions
                |> LanguageModel.streamMessage
                |> streamMessage

                lineBreak ()

                showRoomInfo interactions

                listPeople knownPeople unknownPeople currentRoom.RoomType
                listItems items

                Scene.World) }
