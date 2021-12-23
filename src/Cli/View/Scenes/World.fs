module Cli.View.Scenes.World

open Entities
open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Queries

let rec worldScene state =
    let (_, _, positionContent) = World.currentPosition state

    seq {
        yield!
            match positionContent with
            | Place placeContent -> handlePlace placeContent
            | Street streetContent -> handleStreet streetContent
    }

and handlePlace content =
    match content with
    | Place.RehearsalSpace (rehearsalSpace, rooms) ->
        seq {
            Scene.RehearsalRoom(rehearsalSpace, rooms)
            |> Scene
        }
    | Place.Studio (studio, rooms) ->
        [ Literal $"Inside of {studio.Name}, rooms are {rooms}"
          |> Message ]

and handleStreet content =
    [ Literal $"In street {content.Name}" |> Message ]
