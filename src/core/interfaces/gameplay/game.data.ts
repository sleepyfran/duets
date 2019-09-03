import { IO } from 'fp-ts/lib/IO'
import { Game } from '@core/entities/game'

export interface GameData {
    getGame(): IO<Game>
    saveGame(game: Game): IO<void>
}
