import { map, IO, chain } from 'fp-ts/lib/IO'
import { pipe } from 'fp-ts/lib/pipeable'
import { Skill } from '@engine/entities/skill'
import { GameData } from '@core/interfaces/gameplay/game.data'
import { updateCharacterSkill } from '@engine/operations/character.operations'

export interface SkillsActions {
    modifySkillLevel(skill: Skill, level: number): IO<void>
}

export default (gameData: GameData): SkillsActions => ({
    modifySkillLevel: (skill, level) =>
        pipe(
            gameData.getGame(),
            map(game => ({
                ...game,
                character: updateCharacterSkill(game.character, { ...skill, level }),
            })),
            chain(gameData.saveGame),
        ),
})
