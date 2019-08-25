import { Skill } from '@engine/entities/skill'

type SkillLevel = {
    level: number
}

export type CharacterSkill = Skill & SkillLevel
