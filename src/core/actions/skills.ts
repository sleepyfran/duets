import { map, IO, chain } from 'fp-ts/lib/IO'
import { pipe } from 'fp-ts/lib/pipeable'
import { GameData } from '@core/interfaces/gameplay/game.data'
import { updateCharacterSkill } from '@engine/operations/character.operations'
import { CharacterSkill } from '@engine/entities/character-skill'
import { boundSkillLevel } from '@engine/operations/skill.operations'

export interface SkillsActions {
    modifySkillLevel(skill: CharacterSkill, level: number, assignedPoints: number): IO<void>
}

export default (gameData: GameData): SkillsActions => ({
    modifySkillLevel: (skill, level, assignedPoints) =>
        pipe(
            gameData.getGame(),
            map(game => ({ game, level: boundSkillLevel(skill, level, assignedPoints) })),
            map(({ game, level }) => ({
                ...game,
                character: updateCharacterSkill(game.character, { ...skill, level }),
            })),
            chain(gameData.saveGame),
        ),
})
