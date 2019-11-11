import { StartupCommand } from '@core/commands/init/startup'
import { DownloadDatabaseCommand } from '@core/commands/init/download-database'
import { LoadSavegameCommand } from '@core/commands/savegame/load'

/**
 * List of commands available in the game.
 */
export type Commands = {
    downloadDatabase: DownloadDatabaseCommand
    startup: StartupCommand
    loadSavegame: LoadSavegameCommand
}
