namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
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

                Some "Character info" |> showSeparator

                Command.meName playableCharacter.Name |> showMessage

                Command.meBirthdayAge playableCharacter.Birthday age
                |> showMessage

                Some "Skills" |> showSeparator

                let skills =
                    Queries.Skills.characterSkillsWithLevel
                        (State.get ())
                        playableCharacter.Id
                    |> List.ofMapValues

                skills
                |> List.map (fun (skill, level) ->
                    (level, Skill.skillName skill.Id))
                |> showBarChart

                showSeparator None

                Scene.World }
