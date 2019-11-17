import createStartupCommand from '@core/commands/init/startup'
import FileDatabase from '@infrastructure/file.database'
import ReduxDatabase from '@persistence/store/database/redux.database'
import Store from '@persistence/store/store'
import createLoadSavegameCommand from '@core/commands/savegame/load'
import ElectronSavegameFetcher from '@infrastructure/electron.savegame-fetcher'
import SavegameParser from '@infrastructure/savegame.parser'
import createLoadChangelogCommand from '@core/commands/init/load-changelog'
import ChangelogFetcher from '@infrastructure/changelogs-fetcher'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import createDownloadDatabaseCommand from '@core/commands/init/download-database'
import GitHubDatabase from '@infrastructure/github.database'

const downloadDatabaseCommand = createDownloadDatabaseCommand(
    GitHubDatabase,
    FileDatabase,
    ReduxDatabase(Store.dispatch),
)
const startupCommand = createStartupCommand(FileDatabase, ReduxDatabase(Store.dispatch))
const loadSavegameCommand = createLoadSavegameCommand(ElectronSavegameFetcher, SavegameParser)
const loadChangelogCommand = createLoadChangelogCommand(ChangelogFetcher, ChangelogsData(Store.dispatch))

export default {
    downloadDatabase: downloadDatabaseCommand,
    startup: startupCommand,
    loadSavegame: loadSavegameCommand,
    loadChangelog: loadChangelogCommand,
}
