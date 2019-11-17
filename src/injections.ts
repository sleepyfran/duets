import Store from '@persistence/store/store'
import GameData from '@persistence/store/gameplay/game.data'
import CreateSkillActions from '@core/actions/skills'
import { Injections } from '@ui/contexts/injections.context'

const skillActions = CreateSkillActions(GameData(Store.dispatch))

const injections: Injections = {
    gameplay: {
        skills: skillActions,
    },
}

export default injections
