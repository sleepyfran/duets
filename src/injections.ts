import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import CreateChangelogsQuery from '@core/queries/changelogs'
import CreateWindowCommands from '@core/commands/window'
import ElectronWindow from '@infrastructure/electron.window'
import { Injections } from '@ui/contexts/injections.context'

const changelogsQuery = CreateChangelogsQuery(ChangelogsFetcher, ChangelogsData(Store.dispatch))
const windowCommands = CreateWindowCommands(ElectronWindow)

const injections: Injections = {
    queries: {
        changelogs: changelogsQuery,
    },
    commands: {
        window: windowCommands,
    },
}

export default injections
