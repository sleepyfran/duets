import SkillsReducer from '../skills.reducer'
import { createSaveSkillAction } from '../skills.actions'
import { Skill, SkillType } from '../../../../../engine/entities/skill'

describe('ChangelogsReducer', () => {
    it('should return a list with one skill when createSaveSkillAction with such skill is given', () => {
        const skill: Skill = {
            type: SkillType.music,
            name: 'test',
            level: 0,
        }
        const result = SkillsReducer([], createSaveSkillAction(skill))

        expect(result).toBeTruthy()
        expect(result).toEqual([skill])
    })

    it('should return a list with one modified skill when createSaveSkillAction with a modified skill is given', () => {
        const previousSkill: Skill = {
            type: SkillType.music,
            name: 'test',
            level: 0,
        }

        const modifiedSkill = { ...previousSkill, level: 1 }

        const result = SkillsReducer([previousSkill], createSaveSkillAction(modifiedSkill))

        expect(result).toBeTruthy()
        expect(result).toEqual([modifiedSkill])
    })

    it('should modify only the skills with the same name when createSaveSkillAction with a pre-existing skill is given', () => {
        const existingSkill: Skill = {
            type: SkillType.music,
            name: 'existing',
            level: 0,
        }

        const modifiableSkill: Skill = {
            type: SkillType.music,
            name: 'modifiable',
            level: 0,
        }

        const modifiedSkill = { ...modifiableSkill, level: 1 }

        const result = SkillsReducer([existingSkill], createSaveSkillAction(modifiedSkill))

        expect(result).toBeTruthy()
        expect(result).toEqual([existingSkill, modifiedSkill])
    })
})
