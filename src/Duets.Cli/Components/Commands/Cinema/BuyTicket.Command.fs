namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Cinema

[<RequireQualifiedAccess>]
module BuyTicketCommand =
    /// Command to purchase a ticket to watch a movie at the cinema.
    let create (movie: Movie) ticketPrice =
        { Name = "buy ticket"
          Description =
            $"Buy a ticket to watch {movie.Title} ({Styles.money ticketPrice})"
          Handler =
            (fun _ ->
                let confirmed =
                    $"A ticket costs {Styles.money ticketPrice}. Buy a ticket to watch {movie.Title}?"
                    |> showConfirmationPrompt

                if confirmed then
                    let result = Ticket.pay (State.get ()) movie ticketPrice

                    match result with
                    | Ok effects -> effects |> Effect.applyMultiple
                    | Error(NotEnoughFunds _) ->
                        Shop.notEnoughFunds |> showMessage

                Scene.World) }
