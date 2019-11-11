import Store from '@persistence/store/store'
import ChangelogsFetcher from '@infrastructure/changelogs'
import ChangelogsData from '@persistence/store/changelogs/changelogs.data'
import GameData from '@persistence/store/gameplay/game.data'
import CreateChangelogsActions from '@core/actions/changelogs'
import CreateCreationActions from '@core/actions/creation'
import CreateSkillActions from '@core/actions/skills'
import { Injections } from '@ui/contexts/injections.context'

const changelogsActions = CreateChangelogsActions(ChangelogsFetcher, ChangelogsData(Store.dispatch))
const creationActions = CreateCreationActions()
const skillActions = CreateSkillActions(GameData(Store.dispatch))

const injections: Injections = {
    changelogs: changelogsActions,
    gameplay: {
        skills: skillActions,
    },
    creation: creationActions,
}

export default injections
