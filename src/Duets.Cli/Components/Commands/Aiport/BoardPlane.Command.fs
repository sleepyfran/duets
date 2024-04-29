namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module BoardPlaneCommand =
    /// Command that allows the user to board the plane and start their trip.
    let create flight =
        { Name = "board plane"
          Description = Command.boardPlaneDescription flight
          Handler =
            fun _ ->
                showProgressBarAsync [ Airport.waitingToBoard ] 5<second>
                AirportBoardPlane flight |> Effect.applyAction
                Scene.World }
