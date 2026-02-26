namespace Duets.Cli.Components.Commands

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Interactions

[<RequireQualifiedAccess>]
module WatchMovieCommand =
    /// Command to watch the currently showing movie, which improves mood
    /// and passes time.
    let create (movie: Movie) =
        { Name = "watch movie"
          Description = $"Watch {movie.Title}"
          Handler =
            (fun _ ->
                (Cinema.watchMovieSteps, 1<second>) ||> showProgressBarSync

                Cinema.watchMovie (State.get ()) movie
                |> Effect.applyMultiple

                Scene.World) }
