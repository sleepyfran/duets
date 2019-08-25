import { CharacterSkill } from '@engine/entities/character-skill'

export type SaveSkillAction = {
    type: 'saveSkillAction'
    skill: CharacterSkill
}

export type SkillActions = SaveSkillAction

export const createSaveSkillAction = (skill: CharacterSkill): SaveSkillAction => ({
    type: 'saveSkillAction',
    skill,
})
