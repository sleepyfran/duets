import { Game } from '@core/entities/game'

export interface InMemoryGameData {
    get(): Game
    save: (game: Game) => void
}
