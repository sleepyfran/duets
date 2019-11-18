import { CharacterSkill } from '@engine/entities/character-skill'
import { boundSkillLevel } from '@engine/operations/skill.operations'
import { createOrUpdate } from '@utils/utils'
import { CharacterSkillLenses } from '@core/lenses'

export type SkillUpdateInput = {
    pointsLeft: number
    level: number
    skills: ReadonlyArray<CharacterSkill>
    skill: CharacterSkill
}

export type SkillUpdateResult = {
    pointsLeft: number
    skills: ReadonlyArray<CharacterSkill>
}

export type SkillUpdate = (input: SkillUpdateInput) => SkillUpdateResult

/**
 * Limits the amount of points assignable to the different skills and returns a clone of the input with the new
 * values.
 */
export default (): SkillUpdate => input => {
    const assignedPoints = input.skills.reduce((acc, skill) => acc + skill.level, 0)
    const boundedLevel = boundSkillLevel(input.skill, input.level, assignedPoints)

    const levelDifference = input.skill.level - boundedLevel
    const pointsLeft = input.pointsLeft + levelDifference
    const updatedSkill = CharacterSkillLenses.level.set(boundedLevel)(input.skill)

    return {
        ...input,
        pointsLeft: pointsLeft,
        skills: createOrUpdate(input.skills, updatedSkill, 'name'),
    }
}
