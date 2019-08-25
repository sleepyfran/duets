import { Skill } from '@engine/entities/skill'
import { Database } from '@core/entities/database'

export type SaveSkillsAction = {
    type: 'saveSkillsAction'
    skills: ReadonlyArray<Skill>
}

export type SkillsActions = SaveSkillsAction

export const createSaveSkillsAction = (database: Database): SaveSkillsAction => ({
    type: 'saveSkillsAction',
    skills: database.skills,
})
