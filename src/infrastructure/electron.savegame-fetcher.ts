import { pipe } from 'fp-ts/lib/pipeable'
import { remote } from 'electron'
import { readFile } from './electron.files'
import SavegameFetcher from '@core/interfaces/savegames/savegame.fetcher'

const savegameFetcher: SavegameFetcher = {
    getDefault: pipe(
        remote.app.getPath('userData'),
        duetsDataPath => `${duetsDataPath}/duets.save`,
        readFile,
    ),
}

export default savegameFetcher
