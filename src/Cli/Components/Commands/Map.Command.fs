module rec Cli.Components.Commands.Map

open Agents
open Cli
open Cli.Components
open Cli.Text
open Cli.SceneIndex
open Common
open Entities
open Simulation
open Simulation.Navigation

let private showPlaceChoice placesInCity places =
    let selectedPlace =
        showOptionalChoicePrompt
            Command.mapChoosePlace
            Generic.back
            World.placeWithZone
            places

    match selectedPlace with
    | Some place -> Some place
    | None -> showPlaceTypeChoice placesInCity

let private showPlaceTypeChoice
    (placesInCity: Map<PlaceTypeIndex, Place list>)
    =
    let availablePlaceTypes =
        placesInCity |> List.ofSeq |> List.map (fun kvp -> kvp.Key)

    showOptionalChoicePrompt
        Command.mapChoosePlaceTypePrompt
        Generic.cancel
        World.placeTypeName
        availablePlaceTypes
    |> Option.bind (fun placeType ->
        placesInCity |> Map.find placeType |> showPlaceChoice placesInCity)

[<RequireQualifiedAccess>]
module MapCommand =
    /// Creates a command that allows the player to navigate to different
    /// parts of the game world inside the current city.
    let get =
        { Name = "map"
          Description = Command.mapDescription
          Handler =
            fun _ ->
                Queries.World.currentCity (State.get ())
                |> fun city -> city.Id
                |> Command.mapCurrentCity
                |> showMessage

                Command.mapTip |> showMessage
                lineBreak ()

                let selectedPlace =
                    Queries.World.allPlacesInCurrentCity (State.get ())
                    |> showPlaceTypeChoice

                match selectedPlace with
                | Some place ->
                    Navigation.moveTo place.Id (State.get ()) |> Effect.apply
                | None -> ()

                lineBreak ()

                Scene.WorldAfterMovement }
