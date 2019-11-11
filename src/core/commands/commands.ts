import { StartupCommand } from '@core/commands/init/startup'
import { DownloadDatabaseCommand } from '@core/commands/init/download-database'
import { LoadSavegameCommand } from '@core/commands/savegame/load'
import { ExitCommand } from '@core/commands/window/exit'
import { OpenBrowserCommand } from '@core/commands/window/open-browser-command'

/**
 * List of commands available in the game.
 */
export type Commands = {
    downloadDatabase: DownloadDatabaseCommand
    startup: StartupCommand
    loadSavegame: LoadSavegameCommand
    window: {
        exit: ExitCommand
        openBrowser: OpenBrowserCommand
    }
}
