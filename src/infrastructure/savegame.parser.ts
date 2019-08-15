import { pipe } from 'fp-ts/lib/pipeable'
import { map } from 'fp-ts/lib/Either'
import { Game } from '@core/entities/game'
import SavegameParser from '@core/interfaces/savegames/savegame.parser'
import { tryParseJson } from '@infrastructure/json.utils'

const savegameParser: SavegameParser = {
    parse: (savegame: string) =>
        pipe(
            savegame,
            tryParseJson,
            map(json => json as Game),
        ),
}

export default savegameParser
