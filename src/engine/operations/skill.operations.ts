import { CharacterSkill } from '@engine/entities/character-skill'

export const MAX_ASSIGNABLE_LEVEL_POINTS = 40

/**
 * Bounds a level of a skill between 0 and the max assignable points.
 */
export const boundSkillLevel = (skill: CharacterSkill, level: number, assignedPoints: number) => {
    const maxAvailable = MAX_ASSIGNABLE_LEVEL_POINTS - assignedPoints
    const levelUpperBound = Math.min(level, maxAvailable)

    return Math.max(levelUpperBound, 0)
}
