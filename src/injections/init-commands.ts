import createStartupCommand from '@core/commands/init/startup'
import FileDatabase from '@infrastructure/file.database'
import createLoadChangelogCommand from '@core/commands/init/load-changelog'
import ChangelogFetcher from '@infrastructure/changelogs-fetcher'
import createDownloadDatabaseCommand from '@core/commands/init/download-database'
import GitHubDatabase from '@infrastructure/github.database'
import Store from '@storage/store'

const downloadDatabaseCommand = createDownloadDatabaseCommand(GitHubDatabase, FileDatabase, Store)
const startupCommand = createStartupCommand(FileDatabase, Store)
const loadChangelogCommand = createLoadChangelogCommand(ChangelogFetcher, Store)

export default {
    downloadDatabase: downloadDatabaseCommand,
    startup: startupCommand,
    loadChangelog: loadChangelogCommand,
}
