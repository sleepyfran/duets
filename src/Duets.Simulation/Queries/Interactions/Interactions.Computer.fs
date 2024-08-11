namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities

module Computer =
    let internal interaction item computer =
        let apps = Union.allCasesOf<App> ()

        match computer.ComputerState with
        | Booting -> []
        | AppRunning _ ->
            [ ComputerInteraction.CloseApp(item, computer)
              |> Interaction.Situational ]
        | AppSwitcher ->
            [ ComputerInteraction.OpenApp(item, computer, apps)
              |> Interaction.Situational ]
