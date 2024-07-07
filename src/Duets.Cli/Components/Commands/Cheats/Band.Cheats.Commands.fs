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

                let fans =
                    $"You currently have {band.Fans}, how many fans do you want?"
                    |> Styles.prompt
                    |> showNumberPrompt

                BandFansChanged(band, Diff(band.Fans, fans)) |> Effect.apply

                Scene.Cheats) }
