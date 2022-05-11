module Cli.Scenes.InteractiveSpaces.Components

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation

/// Displays the given objects and the actions that can be done with them.
let showObjects objects =
    if List.isEmpty objects then
        I18n.translate (CommandText CommandLookNoObjectsAround)
        |> showMessage
    else
        I18n.translate (CommandText CommandLookVisibleObjectsPrefix)
        |> showMessage

        objects
        |> List.map (fun object ->
            let commandNames =
                object.Commands
                |> List.map (fun command -> command.Name)

            (object.Type, commandNames))
        |> List.iter (
            CommandLookObjectEntry
            >> CommandText
            >> I18n.translate
            >> showMessage
        )

/// Displays all the rooms that are connected to the current one and the exit
/// if it's available in the room.
let showRoomConnections entrances exit =
    entrances
    |> List.map (fun (a, b, _) -> a, b)
    |> CommandLookEntrances
    |> CommandText
    |> I18n.translate
    |> showMessage

    match exit with
    | Some (_, exitName) ->
        CommandLookExit exitName
        |> CommandText
        |> I18n.translate
        |> showMessage
    | _ -> ()

let private createOutCommand coordinates =
    { Name = "out"
      Description = I18n.translate (CommandText CommandOutDescription)
      Handler =
        (fun _ ->
            State.get ()
            |> World.Navigation.moveTo coordinates
            |> Result.switch Cli.Effect.apply showEntranceError

            Scene.World) }

let private getPlaceName nodeContent =
    match nodeContent with
    | CityNode.Place place ->
        match place.Space with
        | ConcertSpace space -> space.Name
        | RehearsalSpace space -> space.Name
        | Studio space -> space.Name
    | CityNode.OutsideNode node -> node.Name
    |> Literal

/// Returns the coordinates and name of any exit linked with this node id, if
/// any.
let exitOfNode city nodeId exits =
    Queries.World.Common.exitsOfNode nodeId exits
    |> Option.bind (fun exitNodeId ->
        match exitNodeId with
        | Node id ->
            Queries.World.Common.contentOf city.Graph id
            |> getPlaceName
            |> fun name -> Some(Node id, name)
        | _ -> None)

/// Moves a character to the given coordinates and shows the world scene again.
let moveCharacter coordinates =
    State.get ()
    |> World.Navigation.moveTo coordinates
    |> Result.switch Cli.Effect.apply showEntranceError

    Scene.World

/// Renders a command prompt that accepts all default commands on top of the
/// given command list, commands associated with the objects in the room, a
/// look command that shows what's around the user and a set of commands to
/// navigate the world.
let showWorldCommandPrompt entrances exit description objects commands =
    let character =
        Queries.Characters.playableCharacter (State.get ())

    let today =
        Queries.Calendar.today (State.get ())

    let currentDayMoment =
        Calendar.Query.dayMomentOf today

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        commands
        @ objectCommands
          @ NavigationCommand.create entrances
            @ [ match exit with
                | Some (coordinates, _) -> yield createOutCommand coordinates
                | None -> () ]

    showMessage description
    showRoomConnections entrances exit

    showCommandPrompt
        (CommandCommonPrompt(today, currentDayMoment, character.Status)
         |> CommandText
         |> I18n.translate)
        commands

/// Renders a command prompt similar to `showWorldCommandPrompt` but without
/// any reference to navigation commands.
let showWorldCommandPromptWithoutMovement description objects commands =
    let character =
        Queries.Characters.playableCharacter (State.get ())

    let today =
        Queries.Calendar.today (State.get ())

    let currentDayMoment =
        Calendar.Query.dayMomentOf today

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands = commands @ objectCommands

    showMessage description

    showCommandPrompt
        (CommandCommonPrompt(today, currentDayMoment, character.Status)
         |> CommandText
         |> I18n.translate)
        commands
