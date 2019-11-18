import { readFile, writeFile, duetsSavegamePath } from './electron.files'
import Savegame from '@core/interfaces/savegame'
import { tryParseJson } from '@infrastructure/json.utils'
import { Game } from '@core/entities/game'

const savegame: Savegame = {
    getDefault: () => readFile(duetsSavegamePath()),
    parse: (savegame: string) => tryParseJson(savegame).then(json => json as Game),
    save: (game: Game) => writeFile(duetsSavegamePath(), JSON.stringify(game)),
}

export default savegame
