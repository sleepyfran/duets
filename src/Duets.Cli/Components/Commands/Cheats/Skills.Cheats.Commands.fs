namespace Duets.Cli.Components.Commands.Cheats

open Duets
open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module SkillCommands =
    /// Command which allows to modify the skills of the character freely.
    let pureSkill =
        { Name = "pure skill"
          Description = "Allows you to freely modify your skills"
          Handler =
            (fun _ ->
                let state = State.get ()
                let character = Queries.Characters.playableCharacter state
                let band = Queries.Bands.currentBand state
                let characterMember = Queries.Bands.currentPlayableMember state

                let allSkills =
                    Data.Skills.allFor band.Genre characterMember.Role
                    |> List.map (fun skill ->
                        Queries.Skills.characterSkillWithLevel
                            state
                            character.Id
                            skill.Id)

                let selectedSkill =
                    showOptionalChoicePrompt
                        "Which skill do you want to modify?"
                        Generic.back
                        (fun ((skill, level): SkillWithLevel) ->
                            match skill, level with
                            | skill, level when level > 0 ->
                                $"""{Skill.skillName skill.Id} {Styles.faded $"(Currently {Styles.Level.from level})"}"""
                            | _ -> Skill.skillName skill.Id)
                        allSkills

                match selectedSkill with
                | Some(selectedSkill, previousLevel) ->
                    let selectedLevel =
                        $"What level do you want to have on {Skill.skillName selectedSkill.Id |> Styles.highlight}?"
                        |> showRangedNumberPrompt 0 100

                    SkillImproved(
                        character,
                        Diff(
                            (selectedSkill, previousLevel),
                            (selectedSkill, selectedLevel)
                        )
                    )
                    |> Effect.apply

                    Scene.Cheats
                | _ -> Scene.WorldAfterMovement) }
