import Store from '@persistence/store/store'
import GitHubDatabase from '@infrastructure/github.database'
import FileDatabase from '@infrastructure/file.database'
import ElectronSavegameFetcher from '@infrastructure/electron.savegame-fetcher'
import SavegameParser from '@infrastructure/savegame.parser'
import ReduxDatabase from '@persistence/store/database/redux.database'
import createDownloadDatabaseCommand from '@core/commands/init/download-database'
import createStartupCommand from '@core/commands/init/startup'
import createLoadSavegameCommand from '@core/commands/savegame/load'
import { Commands } from '@core/commands/commands'

const downloadDatabaseCommand = createDownloadDatabaseCommand(
    GitHubDatabase,
    FileDatabase,
    ReduxDatabase(Store.dispatch),
)

const startupCommand = createStartupCommand(FileDatabase, ReduxDatabase(Store.dispatch))

const loadSavegameCommand = createLoadSavegameCommand(ElectronSavegameFetcher, SavegameParser)

const commands: Commands = {
    downloadDatabase: downloadDatabaseCommand,
    startup: startupCommand,
    loadSavegame: loadSavegameCommand,
}

export default commands
