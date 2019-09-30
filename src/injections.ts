import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ElectronSavegameFetcher from '@infrastructure/electron.savegame-fetcher'
import SavegameParser from '@infrastructure/savegame.parser'
import GitHubDatabase from '@infrastructure/github.database'
import FileDatabase from '@infrastructure/file.database'
import ReduxDatabase from '@persistence/store/database/redux.database'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import GameData from '@persistence/store/gameplay/game.data'
import CreateChangelogsActions from '@core/actions/changelogs'
import CreateWindowActions from '@core/actions/window'
import CreateSavegameActions from '@core/actions/savegame'
import CreateCreationActions from '@core/actions/creation'
import CreateInitializationActions from '@core/actions/initialization'
import CreateSkillActions from '@core/actions/skills'
import ElectronWindow from '@infrastructure/electron.window'
import { Injections } from '@ui/contexts/injections.context'

const changelogsActions = CreateChangelogsActions(ChangelogsFetcher, ChangelogsData(Store.dispatch))
const savegameActions = CreateSavegameActions(ElectronSavegameFetcher, SavegameParser)
const creationActions = CreateCreationActions()
const initializationActions = CreateInitializationActions(GitHubDatabase, FileDatabase, ReduxDatabase(Store.dispatch))
const skillActions = CreateSkillActions(GameData(Store.dispatch))
const windowActions = CreateWindowActions(ElectronWindow)

const injections: Injections = {
    changelogs: changelogsActions,
    gameplay: {
        skills: skillActions,
    },
    creation: creationActions,
    init: initializationActions,
    savegames: savegameActions,
    window: windowActions,
}

export default injections
