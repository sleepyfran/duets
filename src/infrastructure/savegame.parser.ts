import { Game } from '@core/entities/game'
import SavegameParser from '@core/interfaces/savegames/savegame.parser'
import { tryParseJson } from '@infrastructure/json.utils'

const savegameParser: SavegameParser = {
    parse: (savegame: string) => tryParseJson(savegame).then(json => json as Game),
}

export default savegameParser
