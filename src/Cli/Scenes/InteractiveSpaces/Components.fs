module Cli.Scenes.InteractiveSpaces.Components

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
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
        |> List.map
            (fun object ->
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

let private createLookCommand entrances exit description objects =
    { Name = "look"
      Description = I18n.translate (CommandText CommandLookDescription)
      Handler =
          (fun _ ->
              showMessage description
              showObjects objects
              lineBreak ()
              showRoomConnections entrances exit

              Scene.World) }

let private createOutCommand coordinates =
    { Name = "out"
      Description = I18n.translate (CommandText CommandOutDescription)
      Handler =
          (fun _ ->
              State.get ()
              |> World.Navigation.moveTo coordinates
              |> Cli.Effect.apply

              Scene.World) }

let private getPlaceName nodeContent =
    match nodeContent with
    | ConcertPlace place -> Literal place.Space.Name
    | RehearsalPlace place -> Literal place.Space.Name
    | StudioPlace place -> Literal place.Space.Name
    | OutsideNode node -> Literal node.Name

/// Returns the coordinates and name of any exit linked with this node id, if
/// any.
let exitOfNode city nodeId exits =
    Queries.World.Common.exitsOfNode nodeId exits
    |> Option.bind
        (fun exitNodeId ->
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
    |> Cli.Effect.apply

    Scene.World

/// Renders a command prompt that accepts all default commands on top of the
/// given command list, commands associated with the objects in the room, a
/// look command that shows what's around the user and a set of commands to
/// navigate the world.
let showWorldCommandPrompt entrances exit description objects commands =
    let character =
        Queries.Characters.playableCharacter (State.get ())

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        commands
        @ objectCommands
          @ [ (createLookCommand entrances exit description objects) ]
            @ NavigationCommand.create entrances
              @ [ match exit with
                  | Some (coordinates, _) -> yield createOutCommand coordinates
                  | None -> () ]

    showMessage description
    showRoomConnections entrances exit

    showCommandPrompt
        (CommandCommonPrompt(character.Status)
         |> CommandText
         |> I18n.translate)
        commands

/// Renders a command prompt similar to `showWorldCommandPrompt` but without
/// any reference to navigation commands.
let showWorldCommandPromptWithoutMovement description objects commands =
    let character =
        Queries.Characters.playableCharacter (State.get ())

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        commands
        @ objectCommands
          @ [ (createLookCommand [] None description objects) ]

    showMessage description

    showCommandPrompt
        (CommandCommonPrompt(character.Status)
         |> CommandText
         |> I18n.translate)
        commands
