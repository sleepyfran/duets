import { lens } from 'lens.ts'
import { CharacterSkill } from '@engine/entities/character-skill'
import { Game } from '@core/entities/game'

export const GameLenses = lens<Game>()
export const CharacterSkillLenses = lens<CharacterSkill>()
