import { pipe } from 'fp-ts/lib/pipeable'
import { readFile, duetsDataPath } from './electron.files'
import SavegameFetcher from '@core/interfaces/savegames/savegame.fetcher'

const savegameFetcher: SavegameFetcher = {
    getDefault: pipe(
        duetsDataPath,
        duetsDataPath => `${duetsDataPath}/duets.save`,
        readFile,
    ),
}

export default savegameFetcher
