namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module WorldCommands =
    /// Command which teleports the character to a given city.
    let teleport =
        { Name = "teleport"
          Description = ""
          Handler =
            (fun _ ->
                let destination =
                    Queries.World.allCities
                    |> showOptionalChoicePrompt
                        "Where to? :^)"
                        Generic.cancel
                        (fun city -> Generic.cityName city.Id)

                match destination with
                | Some city ->
                    let destinationAirport =
                        Queries.World.placeIdsByTypeInCity
                            city.Id
                            PlaceTypeIndex.Airport
                        |> List.head (* All cities must have an airport. *)

                    Navigation.travelTo
                        city.Id
                        destinationAirport
                        (State.get ())
                    |> Effect.apply

                    Scene.WorldAfterMovement
                | None -> Scene.World) }
