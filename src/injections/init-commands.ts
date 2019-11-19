import createStartupCommand from '@core/commands/init/startup'
import FileDatabase from '@infrastructure/file.database'
import ReduxDatabase from '@persistence/store/database/redux.database'
import Store from '@persistence/store/store'
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
const loadChangelogCommand = createLoadChangelogCommand(ChangelogFetcher, ChangelogsData(Store.dispatch))

export default {
    downloadDatabase: downloadDatabaseCommand,
    startup: startupCommand,
    loadChangelog: loadChangelogCommand,
}
