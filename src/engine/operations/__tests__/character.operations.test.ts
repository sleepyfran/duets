import { updateCharacterSkill } from '@engine/operations/character.operations'
import { CharacterSkill } from '@engine/entities/character-skill'
import { SkillType } from '@engine/entities/skill'
import { Character } from '@engine/entities/character'
import { createPartialOf } from '@utils/test.utils'

describe('updateCharacterSkill', () => {
    it('should return the character with one skill if the initial skill list is empty', () => {
        const character = createPartialOf<Character>('skills', [])

        const skill: CharacterSkill = {
            type: SkillType.Music,
            name: 'test',
            level: 0,
        }

        const result = updateCharacterSkill(character, skill)

        expect(result.skills).toBeTruthy()
        expect(result.skills).toEqual([skill])
    })

    it('should return the character with a modified skill if the initial skill list contains the given skill', () => {
        const initialSkill: CharacterSkill = {
            type: SkillType.Music,
            name: 'test',
            level: 0,
        }

        const modifiedSkill = { ...initialSkill, level: 1 }

        const character = createPartialOf<Character>('skills', [initialSkill])

        const result = updateCharacterSkill(character, modifiedSkill)

        expect(result).toBeTruthy()
        expect(result.skills).toEqual([modifiedSkill])
    })

    it('should modify only the skills with the same name when the initial skill list contains a given skill', () => {
        const existingSkill: CharacterSkill = {
            type: SkillType.Music,
            name: 'existing',
            level: 0,
        }

        const modifiableSkill: CharacterSkill = {
            type: SkillType.Music,
            name: 'modifiable',
            level: 0,
        }

        const character = createPartialOf<Character>('skills', [existingSkill, modifiableSkill])
        const modifiedSkill = { ...modifiableSkill, level: 1 }

        const result = updateCharacterSkill(character, modifiedSkill)

        expect(result).toBeTruthy()
        expect(result.skills).toEqual([existingSkill, modifiedSkill])
    })
})
