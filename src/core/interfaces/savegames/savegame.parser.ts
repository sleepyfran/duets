import { Game } from '@core/entities/game'
import { Either } from 'fp-ts/lib/Either'

export default interface SavegameParser {
    parse(savegame: string): Either<Error, Game>
}
