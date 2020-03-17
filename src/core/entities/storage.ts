import { Game } from './game'
import { ChangelogList } from './changelog'
import { Database } from './database'
import { Dialog } from './dialog'

export type Storage = {
    database: Database
    game: Game
    ui: {
        changelogList: 'loading' | ChangelogList | Error
        dialog: Dialog
    }
}
