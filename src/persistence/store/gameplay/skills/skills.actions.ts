import { Skill } from '@engine/entities/skill'

export type SaveSkillAction = {
    type: 'saveSkillAction'
    skill: Skill
}

export type SkillActions = SaveSkillAction

export const createSaveSkillAction = (skill: Skill): SaveSkillAction => ({
    type: 'saveSkillAction',
    skill,
})
