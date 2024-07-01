module rec Duets.Cli.Scenes.NewGame.WorldSelector

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

/// Shows a wizard that allows the player to customize the game world.
let worldSelector character =
    showSeparator None
    let cities = Queries.World.allCities

    Creator.cityInfo |> showMessage

    let selectedCity =
        showChoicePrompt
            Creator.cityPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            cities

    Scene.BandCreator(character, selectedCity)
