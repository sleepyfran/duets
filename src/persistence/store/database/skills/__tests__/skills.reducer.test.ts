import SkillsReducer from '../skills.reducer'
import { createSaveSkillsAction } from '../skills.actions'
import { Skill, SkillType } from '../../../../../engine/entities/skill'
import { Database } from '../../../../../core/entities/database'

const createDatabaseWithSkills = (skills: ReadonlyArray<Skill>) => (({ skills } as unknown) as Database)

describe('SkillsReducer', () => {
    it('should return an empty list when the saveSkillsAction with an empty list is given', () => {
        const database = createDatabaseWithSkills([])
        const result = SkillsReducer([], createSaveSkillsAction(database))

        expect(result).toHaveLength(0)
    })

    it('should return a given list when the saveSkillsAction with such list is given', () => {
        const database = createDatabaseWithSkills([
            {
                name: 'test',
                type: SkillType.music,
                level: 0,
            },
        ])
        const result = SkillsReducer([], createSaveSkillsAction(database))

        expect(result).toEqual(database.skills)
    })
})
