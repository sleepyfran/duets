namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

[<RequireQualifiedAccess>]
module MoneyCommands =
    let private giveMoneyToCharacter amount =
        let characterAccount =
            Queries.Bank.playableCharacterAccount (State.get ())

        income (State.get ()) characterAccount amount |> Effect.apply

    /// Command which gives the player 1.000d$.
    let rosebud =
        { Name = "rosebud"
          Description = "Gives you 1.000d$"
          Handler =
            (fun _ ->
                giveMoneyToCharacter 1000m<dd>
                Scene.Cheats) }

    /// Command which gives the player 50.000d$.
    let motherlode =
        { Name = "motherlode"
          Description = "Gives you 50.000d$"
          Handler =
            (fun _ ->
                giveMoneyToCharacter 50000m<dd>
                Scene.Cheats) }

    /// Command which allows setting exact balances for player and band.
    let moneyHeist =
        { Name = "money heist"
          Description = "Set exact money amounts for player and band"
          Handler =
            (fun _ ->
                let state = State.get ()

                let characterAccount =
                    Queries.Bank.playableCharacterAccount state

                let band = Queries.Bands.currentBand state
                let bandAccount = Band band.Id

                let currentPlayerBalance =
                    Queries.Bank.balanceOf state characterAccount

                let currentBandBalance =
                    Queries.Bank.balanceOf state bandAccount

                let newPlayerBalance =
                    $"Current player balance: {currentPlayerBalance}d$. Enter new balance:"
                    |> Styles.prompt
                    |> showDecimalPrompt
                    |> (*) 1m<dd>

                let newBandBalance =
                    $"Current band balance: {currentBandBalance}d$. Enter new balance:"
                    |> Styles.prompt
                    |> showDecimalPrompt
                    |> (*) 1m<dd>

                [ BalanceUpdated(
                      characterAccount,
                      Diff(currentPlayerBalance, newPlayerBalance)
                  )
                  BalanceUpdated(
                      bandAccount,
                      Diff(currentBandBalance, newBandBalance)
                  ) ]
                |> List.iter Effect.apply

                Scene.Cheats) }
