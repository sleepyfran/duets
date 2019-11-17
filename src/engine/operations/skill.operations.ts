import { CharacterSkill } from '@engine/entities/character-skill'
import Config from '@config'

const maxAssignablePoints = Config.maxAssignablePoints

/**
 * Bounds a level of a skill between 0 and the max assignable points.
 */
export const boundSkillLevel = (skill: CharacterSkill, level: number, assignedPoints: number) => {
    const availablePoints = maxAssignablePoints - assignedPoints
    const isIncrementing = level > skill.level

    return isIncrementing //
        ? availablePoints > 0
            ? level <= maxAssignablePoints
                ? level
                : availablePoints
            : skill.level
        : level > 0
        ? level
        : 0
}
