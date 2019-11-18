import { StartupCommand } from '@core/commands/init/startup'
import { LoadChangelogCommand } from '@core/commands/init/load-changelog'
import { DownloadDatabaseCommand } from '@core/commands/init/download-database'
import { LoadSavegameCommand } from '@core/commands/savegame/load'
import { ExitCommand } from '@core/commands/window/exit'
import { OpenBrowserCommand } from '@core/commands/window/open-browser-command'
import { ValidateStartDate } from '@core/commands/forms/creation/validate-start-date'
import { CreateGame } from '@core/commands/forms/creation/create-game'
import { SkillUpdate } from '@core/commands/forms/creation/skill-update'

/**
 * List of commands available in the game.
 */
export type Commands = {
    forms: {
        creation: {
            validateStartDate: ValidateStartDate
            createGame: CreateGame
            skillUpdate: SkillUpdate
        }
    }
    init: {
        downloadDatabase: DownloadDatabaseCommand
        startup: StartupCommand
        loadSavegame: LoadSavegameCommand
        loadChangelog: LoadChangelogCommand
    }
    window: {
        exit: ExitCommand
        openBrowser: OpenBrowserCommand
    }
}
