import { IO } from 'fp-ts/lib/IO'
import { pipe } from 'fp-ts/lib/pipeable'
import { Skill } from '@engine/entities/skill'
import { SkillsData } from '@core/interfaces/gameplay/skills.data'

export interface SkillsActions {
    modifySkillLevel(skill: Skill, level: number): IO<void>
}

export default (skillsData: SkillsData): SkillsActions => ({
    modifySkillLevel: (skill, level) =>
        pipe(
            { ...skill, level },
            skillsData.saveSkill,
        ),
})
