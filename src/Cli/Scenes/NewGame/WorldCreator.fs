module rec Cli.Scenes.NewGame.WorldCreator

open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Setup

/// Shows a wizard that allows the player to customize the game world.
let worldCreator character band =
    showSeparator None
    let cities = Queries.World.allCities
    
    Creator.cityInfo |> showMessage

    let selectedCity =
        showChoicePrompt
            Creator.cityPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            cities

    startGame character band selectedCity |> Effect.apply

    clearScreen ()

    Scene.WorldAfterMovement
