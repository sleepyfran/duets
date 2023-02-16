module rec Duets.Cli.Scenes.NewGame.WorldSelector

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Setup

/// Shows a wizard that allows the player to customize the game world.
let worldSelector character band initialSkills =
    showSeparator None
    let cities = Queries.World.allCities

    Creator.cityInfo |> showMessage

    let selectedCity =
        showChoicePrompt
            Creator.cityPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            cities

    startGame character band initialSkills selectedCity |> Effect.apply

    clearScreen ()

    Scene.WorldAfterMovement
