namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module SwitchGenreCommand =
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
                let currentBand = Queries.Bands.currentBand (State.get ())
                let sortedGenres = genres |> List.sortByDescending snd

                let selectedGenre =
                    showOptionalChoicePrompt
                        "What genre do you want to switch to?"
                        Generic.cancel
                        (fun (genre, popularity: int) ->
                            $"{genre} ({popularity |> Styles.Level.from}%% popular)")
                        sortedGenres

                match selectedGenre with
                | Some(genre, _) ->
                    RehearsalRoomSwitchToGenre
                        {| Band = currentBand; Genre = genre |}
                    |> Effect.applyAction
                | None -> ()

                Scene.World) }
