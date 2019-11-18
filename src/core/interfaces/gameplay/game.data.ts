import { Game } from '@core/entities/game'

export interface GameData {
    getGame(): Game
    saveGame: (game: Game) => void
}
