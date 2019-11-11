import { Game } from '@core/entities/game'

export default interface SavegameParser {
    parse(savegame: string): Promise<Game>
}
