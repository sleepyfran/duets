import { CharacterSkill } from '@engine/entities/character-skill'

export const MAX_ASSIGNABLE_LEVEL_POINTS = 40

/**
 * Bounds a level of a skill between 0 and the max assignable points.
 */
export const boundSkillLevel = (skill: CharacterSkill, level: number, assignedPoints: number) => {
    const availablePoints = MAX_ASSIGNABLE_LEVEL_POINTS - assignedPoints
    const isIncrementing = level > skill.level

    return isIncrementing //
        ? availablePoints > 0
            ? level <= MAX_ASSIGNABLE_LEVEL_POINTS
                ? level
                : availablePoints
            : skill.level
        : level > 0
        ? level
        : 0
}
