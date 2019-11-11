import { readFile, duetsDataPath } from './electron.files'
import SavegameFetcher from '@core/interfaces/savegames/savegame.fetcher'

const savegameFetcher: SavegameFetcher = {
    getDefault: () => readFile(`${duetsDataPath}/duets.save`),
}

export default savegameFetcher
