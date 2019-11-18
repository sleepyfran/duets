import { Gender } from '@engine/entities/gender'
import { CharacterSkill } from '@engine/entities/character-skill'

export type Name = string
export type Mood = number
export type Health = number
export type Fame = number

export type Character = {
    name: Name
    birthday: Date
    gender: Gender
    mood: Mood
    health: Health
    fame: Fame
    skills: ReadonlyArray<CharacterSkill>
}
