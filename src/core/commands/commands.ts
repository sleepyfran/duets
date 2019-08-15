import { SaveGameCommands } from './savegame'
import { WindowCommands } from './window'
import { InitializationCommands } from '@core/commands/initialization'

export type Commands = {
    init: InitializationCommands
    savegames: SaveGameCommands
    window: WindowCommands
}
