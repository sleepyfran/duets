import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ElectronSavegameFetcher from '@infrastructure/electron.savegame-fetcher'
import SavegameParser from '@infrastructure/savegame.parser'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import CreateChangelogsQuery from '@core/queries/changelogs'
import CreateWindowCommands from '@core/commands/window'
import CreateSavegameCommands from '@core/commands/savegame'
import ElectronWindow from '@infrastructure/electron.window'
import { Injections } from '@ui/contexts/injections.context'

/* Commands. */
const savegameCommands = CreateSavegameCommands(ElectronSavegameFetcher, SavegameParser)
const windowCommands = CreateWindowCommands(ElectronWindow)

/* Queries. */
const changelogsQuery = CreateChangelogsQuery(ChangelogsFetcher, ChangelogsData(Store.dispatch))

const injections: Injections = {
    queries: {
        changelogs: changelogsQuery,
    },
    commands: {
        savegames: savegameCommands,
        window: windowCommands,
    },
}

export default injections
