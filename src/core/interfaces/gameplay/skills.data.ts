import { IO } from 'fp-ts/lib/IO'
import { Skill } from '@engine/entities/skill'

export interface SkillsData {
    saveSkill(skill: Skill): IO<void>
}
