import { SaveGameActions } from './savegame'
import { WindowActions } from './window'
import { SkillsActions } from '@core/actions/skills'
import { InitializationActions } from './initialization'
import { ChangelogsActions } from './changelogs'

export type Actions = {
    changelogs: ChangelogsActions
    gameplay: {
        skills: SkillsActions
    }
    init: InitializationActions
    savegames: SaveGameActions
    window: WindowActions
}
