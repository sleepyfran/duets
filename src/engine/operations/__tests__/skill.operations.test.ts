import { boundSkillLevel, MAX_ASSIGNABLE_LEVEL_POINTS } from '@engine/operations/skill.operations'
import { CharacterSkill } from '@engine/entities/character-skill'
import { SkillType } from '@engine/entities/skill'

describe('boundSkillLevel', () => {
    it('should allow decrementing the level even when all the points have been assigned', () => {
        const skill: CharacterSkill = {
            name: 'test',
            type: SkillType.Music,
            level: 1,
        }

        const result = boundSkillLevel(skill, 0, 40)
        expect(result).toEqual(0)
    })

    it('should not allow decrementing a level if that will make it go below zero', () => {
        const skill: CharacterSkill = {
            name: 'test',
            type: SkillType.Music,
            level: 0,
        }

        const result = boundSkillLevel(skill, -1, 0)
        expect(result).toEqual(0)
    })

    it('should bound the level to the maximum available', () => {
        const skill: CharacterSkill = {
            name: 'test',
            type: SkillType.Music,
            level: 0,
        }

        let result = boundSkillLevel(skill, 90, 0)
        expect(result).toEqual(40)

        result = boundSkillLevel(skill, 90, 20)
        expect(result).toEqual(20)
    })

    it('should not allow incrementing a level if all the points have been assigned', () => {
        const skill: CharacterSkill = {
            name: 'test',
            type: SkillType.Music,
            level: 0,
        }

        const result = boundSkillLevel(skill, 1, MAX_ASSIGNABLE_LEVEL_POINTS)
        expect(result).toEqual(0)
    })

    it('should allow incrementing until all the assignable points have been assigned', () => {
        const skill: CharacterSkill = {
            name: 'test',
            type: SkillType.Music,
            level: 20,
        }

        const result = boundSkillLevel(skill, 21, 20)
        expect(result).toEqual(21)
    })
})
