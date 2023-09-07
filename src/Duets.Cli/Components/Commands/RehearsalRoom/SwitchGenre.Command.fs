namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Bands
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module SwitchGenreCommand =
    let private switchTo genre =
        match SwitchGenre.switchGenre (State.get ()) genre with
        | Some effect ->
            showProgressBarSync [ "Switching to another genre..." ] 2<second>

            Effect.apply effect
        | None ->
            $"You tried to switch to {genre}, but yet everything still sounds the same..."
            |> showMessage

    /// Command to switch the current band to a different genre. This will
    /// change the genre that every song composed after the change will have,
    /// which eventually determines the fame of the band, how many streams an
    /// album will get, etc.
    let create (genres: GenrePopularity list) =
        { Name = "switch genre"
          Description =
            "Allows you to switch the main genre of the band. This will change the genre that every song composed after the change will have, which eventually determines the fame of the band, how many streams an album will get, etc."
          Handler =
            (fun _ ->
                let sortedGenres = genres |> List.sortByDescending snd

                let selectedGenre =
                    showOptionalChoicePrompt
                        "What genre do you want to switch to?"
                        Generic.cancel
                        (fun (genre, popularity: int) ->
                            $"{genre} ({popularity |> Styles.Level.from}%% popular)")
                        sortedGenres

                match selectedGenre with
                | Some(genre, _) -> switchTo genre
                | None -> ()

                Scene.World) }
