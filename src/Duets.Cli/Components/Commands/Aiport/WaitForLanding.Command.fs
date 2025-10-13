namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.Prompts
open Duets.Simulation.Flights.Airport

[<RequireQualifiedAccess>]
module WaitForLandingCommand =
    /// Command that allows the user to wait until the flight finishes.
    let create flight =
        { Name = "wait"
          Description = Command.waitForLandingDescription
          Handler =
            fun _ ->
                let state = State.get ()

                Flight.createInFlightExperiencePrompt state flight
                |> LanguageModel.streamMessage
                |> streamStyled Styles.event

                lineBreak ()
                lineBreak ()
                wait 2000<millisecond>

                Flight.createAirportExperiencePrompt state flight
                |> LanguageModel.streamMessage
                |> streamStyled Styles.event

                lineBreak ()
                wait 1500<millisecond>

                lineBreak ()

                leavePlane (State.get ()) flight |> Effect.applyMultiple

                Scene.WorldAfterMovement }
