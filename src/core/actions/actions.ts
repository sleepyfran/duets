import { SaveGameActions } from './savegame'
import { WindowActions } from './window'
import { SkillsActions } from '@core/actions/skills'
import { InitializationActions } from './initialization'
import { ChangelogsActions } from './changelogs'
import { CreationActions } from './creation'

export type Actions = {
    changelogs: ChangelogsActions
    gameplay: {
        skills: SkillsActions
    }
    creation: CreationActions
    init: InitializationActions
    savegames: SaveGameActions
    window: WindowActions
}
