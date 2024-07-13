namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities

module Computer =
    let internal interaction item computer =
        let apps = Union.allCasesOf<App> ()

        [ ComputerInteraction.OpenApp(item, computer, apps)
          |> Interaction.Situational ]
