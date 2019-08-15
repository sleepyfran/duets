import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ElectronSavegameFetcher from '@infrastructure/electron.savegame-fetcher'
import SavegameParser from '@infrastructure/savegame.parser'
import GitHubDatabase from '@infrastructure/github.database'
import FileDatabase from '@infrastructure/file.database'
import ReduxDatabase from '@persistence/store/database/redux.database'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import CreateChangelogsQuery from '@core/queries/changelogs'
import CreateWindowCommands from '@core/commands/window'
import CreateSavegameCommands from '@core/commands/savegame'
import CreateInitializationCommands from '@core/commands/initialization'
import ElectronWindow from '@infrastructure/electron.window'
import { Injections } from '@ui/contexts/injections.context'

/* Commands. */
const savegameCommands = CreateSavegameCommands(ElectronSavegameFetcher, SavegameParser)
const initializationCommands = CreateInitializationCommands(GitHubDatabase, FileDatabase, ReduxDatabase(Store.dispatch))
const windowCommands = CreateWindowCommands(ElectronWindow)

/* Queries. */
const changelogsQuery = CreateChangelogsQuery(ChangelogsFetcher, ChangelogsData(Store.dispatch))

const injections: Injections = {
    queries: {
        changelogs: changelogsQuery,
    },
    commands: {
        init: initializationCommands,
        savegames: savegameCommands,
        window: windowCommands,
    },
}

export default injections
