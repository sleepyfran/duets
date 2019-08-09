import { SaveGameCommands } from './savegame'
import { WindowCommands } from './window'

export type Commands = {
    savegames: SaveGameCommands
    window: WindowCommands
}
