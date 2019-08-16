import { SaveGameActions } from './savegame'
import { WindowActions } from './window'
import { InitializationActions } from './initialization'
import { ChangelogsActions } from './changelogs'

export type Actions = {
    changelogs: ChangelogsActions
    init: InitializationActions
    savegames: SaveGameActions
    window: WindowActions
}
