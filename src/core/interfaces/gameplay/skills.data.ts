import { IO } from 'fp-ts/lib/IO'
import { CharacterSkill } from '@engine/entities/character-skill'

export interface SkillsData {
    saveSkill(skill: CharacterSkill): IO<void>
}
