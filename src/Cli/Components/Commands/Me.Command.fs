namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Simulation

[<RequireQualifiedAccess>]
module MeCommand =
    /// Command which displays the information of the playable character.
    let get =
        { Name = "@me"
          Description = Command.meDescription
          Handler =
            fun _ ->
                let playableCharacter =
                    Queries.Characters.playableCharacter (State.get ())

                let age =
                    Queries.Characters.ageOf (State.get ()) playableCharacter

                showSeparator None

                Command.meName playableCharacter.Name
                |> showMessage

                Command.meBirthdayAge playableCharacter.Birthday age
                |> showMessage

                showSeparator None

                Scene.World }
