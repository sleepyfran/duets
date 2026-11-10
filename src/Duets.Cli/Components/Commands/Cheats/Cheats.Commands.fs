namespace Duets.Cli.Components.Commands.Cheats

open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex

module Index =
    let all =
        [ BandCommands.instantBand
          BandCommands.makeMeABand
          BandCommands.pactWithTheDevil
          ConcertCommands.scheduleNightConcert
          ConcertCommands.rockstar
          ConcertCommands.soldOut
          LifeCommands.happy
          LifeCommands.notMoody
          LifeCommands.roaming
          LifeCommands.spotlight
          LifeCommands.timeTravel
          MerchCommands.sweatshop
          MoneyCommands.moneyHeist
          MoneyCommands.motherlode
          MoneyCommands.rosebud
          SkillCommands.pureSkill
          WorldCommands.teleport
          WorldCommands.beamMeUp
          WorldCommands.weatherman ]

    let enterCommand =
        { Name = "iwanttoskipreality"
          Description = ""
          Handler = fun _ -> Scene.Cheats }
