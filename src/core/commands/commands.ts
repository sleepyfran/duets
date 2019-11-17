import { StartupCommand } from '@core/commands/init/startup'
import { DownloadDatabaseCommand } from '@core/commands/init/download-database'
import { LoadSavegameCommand } from '@core/commands/savegame/load'
import { ExitCommand } from '@core/commands/window/exit'
import { OpenBrowserCommand } from '@core/commands/window/open-browser-command'
import { ValidateStartDate } from '@core/commands/forms/creation/validate-start-date'
import { LoadChangelogCommand } from '@core/commands/init/load-changelog'

/**
 * List of commands available in the game.
 */
export type Commands = {
    forms: {
        creation: {
            validateStartDate: ValidateStartDate
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
