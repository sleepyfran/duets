namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module BandCommands =
    /// Allows the player to change the number of fans the band has.
    let pactWithTheDevil =
        { Name = "pact with the devil"
          Description =
            "Allows you to change how many fans you have... for a price. Nah, not really, go nuts."
          Handler =
            (fun _ ->
                let band = Queries.Bands.currentBand (State.get ())

                let allCities = Queries.World.allCities

                let chosenCity =
                    showChoicePrompt
                        "Where do you want to change your fans?"
                        (fun (city: City) -> Generic.cityName city.Id)
                        allCities

                let fansInCity = Queries.Bands.fansInCity' band chosenCity.Id

                let fans =
                    $"You currently have {fansInCity}, how many fans do you want there?"
                    |> Styles.prompt
                    |> showNumberPrompt
                    |> (*) 1<fans>

                let updatedFans = band.Fans |> Map.add chosenCity.Id fans

                BandFansChanged(band, Diff(band.Fans, updatedFans))
                |> Effect.apply

                Scene.Cheats) }
