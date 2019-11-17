import Store from '@persistence/store/store'
import GameData from '@persistence/store/gameplay/game.data'
import CreateCreationActions from '@core/actions/creation'
import CreateSkillActions from '@core/actions/skills'
import { Injections } from '@ui/contexts/injections.context'

const creationActions = CreateCreationActions()
const skillActions = CreateSkillActions(GameData(Store.dispatch))

const injections: Injections = {
    gameplay: {
        skills: skillActions,
    },
    creation: creationActions,
}

export default injections
