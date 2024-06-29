namespace Duets.Cli.Components.Commands.Cheats

open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex

module Index =
    let all =
        [ BandCommands.pactWithTheDevil
          LifeCommands.happy
          LifeCommands.notMoody
          LifeCommands.timeTravel
          MoneyCommands.motherlode
          MoneyCommands.rosebud
          WorldCommands.teleport ]

    let enterCommand =
        { Name = "iwanttoskipreality"
          Description = ""
          Handler = fun _ -> Scene.Cheats }
