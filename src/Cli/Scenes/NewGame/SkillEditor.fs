module rec Cli.Scenes.NewGame.SkillEditor

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities

let skillEditor
    (character: Character)
    (bandMember: CurrentMember)
    (band: Band)
    =
    showSeparator None

    let skills =
        Data.Skills.allFor band.Genre bandMember.Role
        |> List.groupBy (fun skill -> skill.Category)
        |> Map.ofList

    $"""You will now be able to select the initial level of your character's skills
There's no total limit of points you can assign, so feel free to tailor the levels to the experience you want to have
    """
    |> Styles.faded
    |> showMessage

    showSkillCategoryPrompt character band Map.empty skills

let private showSkillCategoryPrompt character band modifiedSkills allSkills =
    let doneOptionText =
        if Map.isEmpty modifiedSkills then
            Generic.skipOption
        else
            Generic.doneOption

    let selectedCategory =
        showOptionalChoicePrompt
            $"""Select a skill {Styles.highlight "category"}"""
            doneOptionText
            (fun (category, _) -> Skill.categoryName category)
            (allSkills |> List.ofMap)

    match selectedCategory with
    | Some category ->
        showSkillPrompt character band modifiedSkills allSkills category
    | None ->
        Scene.WorldSelector(character, band, modifiedSkills |> List.ofMapValues)

let private showSkillPrompt
    character
    band
    modifiedSkills
    allSkills
    selectedCategory
    =
    let selectedSkill =
        showOptionalChoicePrompt
            $"""Select a {Styles.highlight "skill"}"""
            Generic.back
            (fun (skill: Skill) ->
                let alreadySelectedSkill =
                    modifiedSkills |> Map.tryFind skill.Id

                match alreadySelectedSkill with
                | Some (_, level) ->
                    $"""{Skill.skillName skill.Id} {Styles.faded $"(Currently {Styles.Level.from level})"}"""
                | None -> Skill.skillName skill.Id)
            (selectedCategory |> snd)

    match selectedSkill with
    | Some skill ->
        showSkillLevelPrompt
            character
            band
            modifiedSkills
            allSkills
            selectedCategory
            skill
    | None -> showSkillCategoryPrompt character band modifiedSkills allSkills

let private showSkillLevelPrompt
    character
    band
    modifiedSkills
    allSkills
    selectedCategory
    selectedSkill
    =
    Skill.skillDescription selectedSkill.Id |> showMessage

    let selectedLevel =
        $"What level do you want to have on {Skill.skillName selectedSkill.Id |> Styles.highlight}?"
        |> showRangedNumberPrompt 0 100

    let modifiedSkillsWithNewLevel =
        modifiedSkills
        |> Map.add
            selectedSkill.Id
            (Skill.createWithLevel selectedSkill.Id selectedLevel)

    showSkillPrompt
        character
        band
        modifiedSkillsWithNewLevel
        allSkills
        selectedCategory
