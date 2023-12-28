namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
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
