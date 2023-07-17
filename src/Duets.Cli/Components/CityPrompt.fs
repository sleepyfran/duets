[<AutoOpen>]
module rec Duets.Cli.Components.CityPrompt

open Duets.Agents
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

/// <summary>
/// Shows a prompt to select a city, with the current city at the top and the
/// rest ordered alphabetically.
/// </summary>
/// <param name="prompt">Prompt to show the user</param>
let showCityPrompt prompt =
    let currentCity = Queries.World.currentCity (State.get ())

    showOptionalChoicePrompt
        prompt
        Generic.cancel
        (fun (city: City) ->
            if city.Id = currentCity.Id then
                $"{Generic.cityName city.Id} (Current)" |> Styles.highlight
            else
                Generic.cityName city.Id)
        (sortedCities currentCity)

/// Lists all available cities with the current one at the top.
let private sortedCities currentCity =
    let allButCurrentCity =
        Queries.World.allCities
        |> List.filter (fun city -> city.Id <> currentCity.Id)
        |> List.sortBy (fun city -> Generic.cityName city.Id)

    currentCity :: allButCurrentCity
