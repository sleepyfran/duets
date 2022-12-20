namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation

[<RequireQualifiedAccess>]
module MeCommand =
    let private showCharacterInfo character =
        let age = Queries.Characters.ageOf (State.get ()) character

        Command.meName character.Name |> showMessage

        Command.meBirthdayAge character.Birthday age |> showMessage

    let private showCharacterAttributes _ =
        Queries.Characters.allPlayableCharacterAttributes (State.get ())
        |> List.map (fun (attr, amount) ->
            (amount, Character.attributeName attr))
        |> showBarChart

    let private showCharacterSkills (character: Character) =
        let skills =
            Queries.Skills.characterSkillsWithLevel (State.get ()) character.Id
            |> List.ofMapValues

        skills
        |> List.map (fun (skill, level) -> (level, Skill.skillName skill.Id))
        |> showBarChart

    /// Command which displays the information of the playable character.
    let get =
        { Name = "@me"
          Description = Command.meDescription
          Handler =
            fun _ ->
                let playableCharacter =
                    Queries.Characters.playableCharacter (State.get ())

                Some "Character info" |> showSeparator

                showCharacterInfo playableCharacter

                Some "Attributes" |> showSeparator

                showCharacterAttributes playableCharacter

                Some "Skills" |> showSeparator

                showCharacterSkills playableCharacter

                showSeparator None

                Scene.World }
