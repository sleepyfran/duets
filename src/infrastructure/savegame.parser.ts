import { pipe } from 'fp-ts/lib/pipeable'
import { map, tryCatch } from 'fp-ts/lib/Either'
import { Game } from '@core/entities/game'
import SavegameParser from '@core/interfaces/savegames/savegame.parser'

const tryParseJson = (json: string) => {
    return tryCatch(() => JSON.parse(json), error => new Error(String(error)))
}

const savegameParser: SavegameParser = {
    parse: (savegame: string) =>
        pipe(
            savegame,
            tryParseJson,
            map(json => json as Game),
        ),
}

export default savegameParser
