namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module TravelByMetroCommand =
    /// Command that allows the user to travel by metro to a specific destination.
    let create
        (availableConnections: (MetroStationConnections * MetroLine) list)
        =
        { Name = "travel"
          Description =
            "Allows you to travel to another metro station that connects to this one."
          Handler =
            fun _ ->
                let state = State.get ()

                // TODO: Destinations should include ALL available stations.
                let destinations =
                    availableConnections
                    |> List.fold
                        (fun acc (connections, line) ->
                            match connections with
                            | OnlyPreviousCoords previous ->
                                (previous, line) :: acc
                            | PreviousAndNextCoords(previous, next) ->
                                (previous, line) :: (next, line) :: acc
                            | OnlyNextCoords next -> (next, line) :: acc)
                        []

                let selectedDestination =
                    showOptionalChoicePrompt
                        (Styles.prompt
                            "You can travel to these stations, where would you like to go?")
                        Generic.cancel
                        (fun ((place: Place, zone: Zone, _), line: MetroLine) ->
                            $"{place.Name |> Styles.place} in {zone.Name |> Styles.faded} through {line.Id |> Styles.line} line")
                        destinations

                match selectedDestination with
                | Some(option) ->
                    let (targetPlace, _, _), _ = option

                    let currentDayMoment =
                        Queries.Calendar.today state
                        |> Calendar.Query.dayMomentOf

                    showProgressBarSync
                        [ Travel.blindlyStaringAtPhone ]
                        2<second>

                    // Moving to metro stations should not yield any entrance errors,
                    // so ignore the error for now.
                    Navigation.moveTo targetPlace.Id state
                    |> Result.iter Effect.apply

                    Travel.arrivedAtStation currentDayMoment |> showMessage

                    Scene.WorldAfterMovement
                | None ->
                    // TODO: Exit the train!
                    Scene.World }
