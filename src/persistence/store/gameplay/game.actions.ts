import { Game } from '@core/entities/game'

export type SaveGameActions = {
    type: 'saveSkillAction'
    game: Game
}

export type GameActions = SaveGameActions

export const createSaveGameAction = (game: Game): SaveGameActions => ({
    type: 'saveSkillAction',
    game,
})
