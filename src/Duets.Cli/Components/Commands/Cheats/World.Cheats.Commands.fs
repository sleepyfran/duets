namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Common
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
          Description = "Teleports you to another city instantly"
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
                | None -> Scene.Cheats) }

    /// Command which changes the weather of the current city.
    let weatherman =
        { Name = "weatherman"
          Description = "Changes the weather in the current city"
          Handler =
            (fun _ ->
                let state = State.get ()
                let cityId, _, _ = Queries.World.currentCoordinates state
                let currentWeather = Queries.World.currentWeather state

                let selectedWeather =
                    Union.allCasesOf<WeatherCondition> ()
                    |> showOptionalChoicePrompt
                        "What weather do you want? :^)"
                        Generic.cancel
                        (fun weather ->
                            let name = $"{Emoji.weather weather} %A{weather}"

                            if weather = currentWeather then
                                $"{name} (Current)" |> Styles.highlight
                            else
                                name)

                match selectedWeather with
                | Some weather when weather <> currentWeather ->
                    WeatherChanged(cityId, Diff(currentWeather, weather))
                    |> Effect.apply
                | _ -> ()

                Scene.Cheats) }
